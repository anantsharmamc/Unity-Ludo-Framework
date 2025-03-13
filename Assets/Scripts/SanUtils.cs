using UnityEngine;
using System.Collections.Generic;

namespace com.bhambhoo.fairludo
{
    public class SanUtils : MonoBehaviour
    {
        private static Dictionary<Transform, AudioSource[]> transformAudioSourceMap;
        private static int ignoreRaycast = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        public static void PlaySound(AudioClip clip)
        {
            PlaySound(clip, MatchManager.Instance.audioSource);
        }

        // Stops the audio source and plays the given clip on it
        public static void PlaySound(AudioClip clip, AudioSource audioSource)
        {
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