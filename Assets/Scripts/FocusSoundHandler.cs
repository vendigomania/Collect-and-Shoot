using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSoundHandler : MonoBehaviour
{
    private AudioSource[] audioSources;

    public void SetSound(bool state) {
        audioSources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audioSources.Length; i++) {
            audioSources[i].volume = state ? 1 : 0;
        }
    }
}
