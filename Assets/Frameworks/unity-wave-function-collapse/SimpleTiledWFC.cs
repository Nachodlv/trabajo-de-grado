using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class SimpleTiledWFC : MonoBehaviour{

	public TextAsset xml = null;
	private string subset = "";

	[SerializeField] protected float gridsize = 1;
	public int width = 20;
	public int depth = 20;

	public int seed = 0;
	public bool periodic = false;
	public int iterations = 0;
	public bool incremental;

	public SimpleTiledModel model = null;
	public GameObject[,] rendering;
	public GameObject output;
	private Transform group;
	public Dictionary<string, GameObject> obmap = new Dictionary<string, GameObject>();
	protected Transform ObstacleParent => group ? group : group = transform.Find("output-tiled");
	[NonSerialized]
	public bool started;

	private bool undrawn = true;

	public void destroyChildren (){
		foreach (Transform child in this.transform) {
     		GameObject.DestroyImmediate(child.gameObject);
 		}
 	}

	void Update(){
		if (started && incremental){
			Run();
		}
	}


	public bool Run(){
		if (model == null || !undrawn){return false;}
		if (!model.Run(seed, iterations)) return false;
		Draw();
        return true;
	}

	public void OnDrawGizmos(){
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(new Vector3(width*gridsize/2f-gridsize*0.5f, depth*gridsize/2f-gridsize*0.5f, 0f),new Vector3(width*gridsize, depth*gridsize, gridsize));
	}

	public void Generate(){
		obmap = new  Dictionary<string, GameObject>();

		if (output == null){
			Transform ot = transform.Find("output-tiled");
			if (ot != null){output = ot.gameObject;}}
		if (output == null){
			output = new GameObject("output-tiled");
			output.transform.parent = transform;
			output.transform.position = this.gameObject.transform.position;
			output.transform.rotation = this.gameObject.transform.rotation;}

		for (int i = 0; i < output.transform.childCount; i++){
			GameObject go = output.transform.GetChild(i).gameObject;
			if (Application.isPlaying){Destroy(go);} else {DestroyImmediate(go);}
		}
		group = new GameObject(xml.name).transform;
		group.parent = output.transform;
		group.position = output.transform.position;
		group.rotation = output.transform.rotation;
        group.localScale = new Vector3(1f, 1f, 1f);
        rendering = new GameObject[width, depth];
		this.model = new SimpleTiledModel(xml.text, subset, width, depth, periodic);
        undrawn = true;
    }

	public void Draw(){
		if (output == null){return;}
		if (group == null){return;}
        undrawn = false;
		for (int y = 0; y < depth; y++){
			for (int x = 0; x < width; x++){
				if (rendering[x,y] == null){
					string v = model.Sample(x, y);
					int rot = 0;
					if (v != "?"){
						rot = int.Parse(v.Substring(0,1));
						Vector3 pos = new Vector3(x*gridsize, y*gridsize, 0f);
						GameObject tile = InstantiateGameObject(v, pos, new Vector3(0, 0, 360-(rot*90)));
						if(tile == null) continue;
						rendering[x,y] = tile;
					} else
                    {
                        undrawn = true;
                    }
				}
			}
  		}
	}


	protected virtual GameObject InstantiateGameObject(string v, Vector3 position, Vector3 localEulerAngles)
	{
		v = v.Substring(1);
		GameObject fab;
		if (!obmap.ContainsKey(v)){
			fab = (GameObject)Resources.Load(v, typeof(GameObject));
			obmap[v] = fab;
		} else {
			fab = obmap[v];
		}

		if (fab == null) return null;
		var newGameObject = Instantiate(fab, Vector3.zero, Quaternion.identity, group);
		newGameObject.transform.localPosition = position;
		var fscale = newGameObject.transform.localScale;
		newGameObject.transform.localEulerAngles = localEulerAngles;
		newGameObject.transform.localScale = fscale;
		return newGameObject;
	}
}

#if UNITY_EDITOR
[CustomEditor (typeof(SimpleTiledWFC))]
public class TileSetEditor : Editor {
	public override void OnInspectorGUI () {
		SimpleTiledWFC me = (SimpleTiledWFC)target;
		if (me.xml != null){
			if(GUILayout.Button("generate")){
				me.Generate();
			}
			if (me.model != null){
				if(GUILayout.Button("RUN"))
				{
					me.started = true;
					me.model.Run(me.seed, me.iterations);
					me.Draw();
				}
			}
		}
		DrawDefaultInspector ();
	}
}
#endif
