using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class FocusSoundHandler : MonoBehaviour
{
    private AdShowHandler _adShowHandler;

    private AudioSource[] audioSources;

    private void Awake() {
        _adShowHandler = GetComponent<AdShowHandler>();
    }

    private void Start() {
        OnFocusGet();
    }

    public void OnFocusGet() {
        if (!_adShowHandler.IsAdOpen) {
            SetSound(true);
        }
    }

    public void OnFocusLose() {
        if (!_adShowHandler.IsAdOpen) {
            SetSound(false);
        }
    }

    public void SetSound(bool state) {
        audioSources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audioSources.Length; i++) {
            audioSources[i].volume = state ? 1 : 0;
        }
    }
}
