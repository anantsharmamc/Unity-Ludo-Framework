using UnityEngine;
using System.Collections.Generic;

namespace com.bhambhoo.fairludo
{
    public static class AudioUtil
    {
        private static Dictionary<Transform, AudioSource[]> transformAudioSourceMap;

        public static void PlaySound(AudioClip clip, AudioSource audioSource = null)
        {
            if (audioSource == null)
            {
                audioSource = GameManager.Instance.AudioSource;
            }

            if (clip == null)
            {
                Debug.LogError("Trying to play audio but no clip to play for audioSource on " +
                               audioSource.gameObject.name);
                return;
            }

            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.time = 0;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}