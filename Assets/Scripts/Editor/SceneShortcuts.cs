﻿using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    public class SceneShortcuts : UnityEditor.Editor
    {
        [MenuItem("Tools/Go to Cloud Scene _F1")]
        public static void GoToCloudScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/CloudAnchors.unity");
            }
        }

        [MenuItem("Tools/Go to Quick Start Scene _F3")]
        public static void GoToQuickStartScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/QuickStart Scene.unity");
            }
        }

        [MenuItem("Tools/Go to Initial Scene _F4")]
        public static void GoToInitialScreen()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/Initial Screens.unity");
            }
        }
    }
}
