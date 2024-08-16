using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesExistanceHandler : MonoBehaviour
{
    [SerializeField] private EnemyWaveSpawner _waveSpawner;

    private float _checkEverySeconds = 2;

    private string _enemyTag = "Enemy";

    public void InitializeHandler(){
        StartCoroutine(NoEnemiesAtSceneCheckingRoutine());
    }

    private IEnumerator NoEnemiesAtSceneCheckingRoutine() {
        yield return new WaitForSeconds(_checkEverySeconds);
        {
            TryToStartSpawningNewWaveOnNoEnemies();
            StartCoroutine(NoEnemiesAtSceneCheckingRoutine());
        }
    }

    private void TryToStartSpawningNewWaveOnNoEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
        if (BulletsCollecter.Instance.CollectedEnoughBulletsToStart) {
            if (enemies.Length <= 0) {
                _waveSpawner.SpawnNewWave();
            }
        }
    }

    public static bool IsAEnemyOnScene() {
        string tag = "Enemy";
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        return (enemies.Length > 0);
    }

    public static bool IsAliveEnemyOnScene() {
        EnemyHealthHandler[] enemies = FindObjectsOfType<EnemyHealthHandler>();
        for (int i = 0; i < enemies.Length; i++) {
            if (!enemies[i].IsDead) {
                return true;
            }
        }
        return (EnemyWaveSpawner.Instance.IsSpawningWave);
    }
}
