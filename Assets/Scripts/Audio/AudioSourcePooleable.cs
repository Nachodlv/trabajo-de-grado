using System;
using System.Collections;
using UnityEngine;
using Utils.Pools;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcePooleable: Pooleable
    {
        private AudioSource _audioSource;
        private Func<IEnumerator> _playingSoundCoroutine;
        public Transform Transform { get; private set; }

        public bool Spatialize
        {
            get => _audioSource.spatialize;
            set => _audioSource.spatialize = value;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            _playingSoundCoroutine = WaitClipToStop;
            Transform = transform;
        }

        public void SetClip(AudioType clip)
        {
            _audioSource.clip = AudioManager.Instance.GetAudioClip(clip);
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public void StartClip()
        {
            _audioSource.Play();
            StartCoroutine(_playingSoundCoroutine());
        }

        private IEnumerator WaitClipToStop()
        {
            var clipLength = _audioSource.clip.length;
            var now = Time.time;
            while (Time.time - now < clipLength)
                yield return null;
            Deactivate();
        }
    }
}
