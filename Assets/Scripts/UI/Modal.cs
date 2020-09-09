﻿using System;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class Modal : MonoBehaviour
    {
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Hide = Animator.StringToHash("hide");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ShowModal()
        {
            _animator.SetTrigger(Show);
        }

        public void HideModal()
        {
            _animator.SetTrigger(Hide);
        }
    }
}