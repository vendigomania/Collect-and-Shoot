using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAdjuster : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate = 60;

    [SerializeField] private int _vSyncCount = 0;

    private void Awake() => AdjustRenderSettings();

    public void AdjustRenderSettings() {
        Application.targetFrameRate = _targetFrameRate;
        QualitySettings.vSyncCount = _vSyncCount;
    }
}
