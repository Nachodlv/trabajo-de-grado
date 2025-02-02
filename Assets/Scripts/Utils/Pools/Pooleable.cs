﻿using UnityEngine;

namespace Utils.Pools
{
    public class Pooleable : MonoBehaviour
    {
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Activates the Pooleable
        /// </summary>
        public virtual void Activate()
        {
            if(IsActive) return;
        
            gameObject.SetActive(true);
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the Pooleable
        /// </summary>
        public virtual void Deactivate()
        {
            if (!IsActive) return;
        
            gameObject.SetActive(false);
            IsActive = false;
        }
    }
}
