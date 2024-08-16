using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentLevelTextsHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _previousWave, _nextWave;

    [SerializeField] private EnemyWaveSpawner _waveSpawner;

    public static CurrentLevelTextsHandler Instance;

    private void Awake() {
        Instance = this;
    }

    public void UpdateTexts() {
        _previousWave.text = $"{_waveSpawner.WavesCompleted}";
        _nextWave.text = $"{_waveSpawner.WavesCompleted + 1}";
    }
}
