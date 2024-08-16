using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform _player, _basePointForEnemySpawning;

    [SerializeField] private float _minimalXPosition, _maximalXPosition;

    [SerializeField] private float _minimalZPosition, _maximalZPosition;

    [SerializeField] private int _wavesAmount;

    [SerializeField] private List<int> _typesForSubwave;
    [SerializeField] private List<int> _enemiesForSubwave;

    public int[] TurnUpgradesAtWavesLeft;

    public UnityEvent OnWaveStarted, OnReachedCheckpointWave;

    public UnityEvent OnWaveCompleted;

    public bool IsSpawningWave { get; private set; } = false;

    public UnityEvent OnAllWavesDestroyed;

    [SerializeField] private float _betweenEachSubwaveEnemySpawnDelay;

    [SerializeField] private GameObject[] _enemiesToSpawn;

    public int CurrentWavesAmount{ get; private set; }

    [field: SerializeField] public int WavesCompleted { get; private set; } = 1;

    public int CurrentEnemiesForSubwaveAmount { get; private set; }

    [SerializeField] private int _betweenWaveSpawningDelay = 5;

    [SerializeField] private int[] _spawnGoldBulletsAfterSceneIndexes;

    public UnityEvent OnReachedGoldSpawningWave;

    private BulletsCollecter _bulletsCollector;

    [SerializeField] private bool _callCheckpointEvent = true;

    private bool _turnedGoldenWave = false;

    public static EnemyWaveSpawner Instance;

    public int TargetLevelToNotify = 1;

    private void Awake() {
        Instance = this;
    }

    public void InitializeSpawner() {
        _bulletsCollector = FindObjectOfType<BulletsCollecter>();
        CurrentWavesAmount = _wavesAmount;
        CurrentEnemiesForSubwaveAmount = _enemiesForSubwave[WavesCompleted - 1];
    }
    
    public void SpawnNewWave() {
        int minimalAmount = 1;
        if (CurrentWavesAmount > minimalAmount) {
            if (!IsSpawningWave) {
                if (!UpgradesShower.Instance.AreUpgradesShown) {
                    TryToTurnGoldSpawningScene();
                    if (BulletsSpawner.Instance.IsGoldenBulletGiven) {
                        if (_bulletsCollector.UsedGoldenBullet) {
                            StartWaveSpawning();
                            _bulletsCollector.UsedGoldenBullet = false;
                            BulletsSpawner.Instance.IsGoldenBulletGiven = false;
                            _turnedGoldenWave = false;
                        }
                    }
                    else StartWaveSpawning();
                }
            }
        }
        else {
            ForceGoldSpawningWave();
            OnAllWavesDestroyed?.Invoke();
        }
    }

    public void StartWaveSpawning() {
        if (!IsNextWaveUpgradesTurning()) {
            ForceStartSpawning();
        }
        else {
            UpgradesShower.Instance.ShowUpgrades();
        }

        int checkpointWave = 6;
        if(CurrentWavesAmount == checkpointWave) {
            OnReachedCheckpointWave?.Invoke();
        }
    }

    public void ForceStartSpawning() {
       // SupersonicWisdom.Api.NotifyLevelStarted(TargetLevelToNotify, null);
        Debug.Log($"Start Level {TargetLevelToNotify}");
        OnWaveStarted?.Invoke();
        ProgressBarFillingHandler.Instance._perEnemyFillingValue = 1f / CurrentEnemiesForSubwaveAmount;
        LockBulletHitHandler.GetHit = false;
        StartCoroutine(BetweenWaveSpawningDelay());
        ProgressBarFillingHandler.Instance.ResetBarFillAmount();
        ProgressBarFillingHandler.Instance.ChangeBarIndex(1);
        IsSpawningWave = true;
    }
    
    private bool IsNextWaveUpgradesTurning() {
        for (int i = 0; i < TurnUpgradesAtWavesLeft.Length; i++) {
            if ((CurrentWavesAmount) == TurnUpgradesAtWavesLeft[i]) return true;
        }
        return false;
    }

    private IEnumerator BetweenWaveSpawningDelay() {
        yield return new WaitForSeconds(_betweenWaveSpawningDelay);
        {
            SpawnSubwaveEnemy();
            CurrentWavesAmount--;
        }
    }

    public void SpawnSubwaveEnemy() {
        int minimalAmount = 1;
        if (CurrentEnemiesForSubwaveAmount >= minimalAmount) {
            float minimalFromPlayerDistance = 3f;
            float xPosition = Random.Range(_minimalXPosition, _maximalXPosition);
            float zPosition = Random.Range(_minimalZPosition, _maximalZPosition);
            Vector3 targetPosition = _basePointForEnemySpawning.position + new Vector3(xPosition, _player.position.y, zPosition);
            Vector3 position = targetPosition;
            if(Vector3.Distance(_player.position, targetPosition) <= minimalFromPlayerDistance) {
                position = _player.position + new Vector3(minimalFromPlayerDistance, _player.position.y, minimalFromPlayerDistance);
            }
            //int enemyIndex = Random.Range(0, _enemiesToSpawn.Length);
            int enemyIndex = Random.Range(0, _typesForSubwave[CurrentEnemiesForSubwaveAmount - 1]);
            Instantiate(_enemiesToSpawn[enemyIndex], position, Quaternion.identity);
            CurrentEnemiesForSubwaveAmount--;
            StartCoroutine(TryToContinueSubwaveSpawning());
        }
        else {
            WavesCompleted++;
            CurrentEnemiesForSubwaveAmount = _enemiesForSubwave[WavesCompleted - 1];
            IsSpawningWave = false;
        }
    }

    private IEnumerator TryToContinueSubwaveSpawning() {
        yield return new WaitForSeconds(_betweenEachSubwaveEnemySpawnDelay);
        {
            SpawnSubwaveEnemy();
        }
    }

    private void TryToTurnGoldSpawningScene() {
        if (!_turnedGoldenWave) {
            bool goldenWave = false;
            for (int i = 0; i < _spawnGoldBulletsAfterSceneIndexes.Length; i++) {
                if (_spawnGoldBulletsAfterSceneIndexes[i] == CurrentWavesAmount) {
                    goldenWave = true;
                    _turnedGoldenWave = true;
                    break;
                }
            }

            if (goldenWave) {
                if(BulletsSpawner.Instance.CanSpawnGoldenBullet) OnReachedGoldSpawningWave?.Invoke();
            }
        }
    }
    
    private void ForceGoldSpawningWave() {
        if (!_turnedGoldenWave) {
            OnReachedGoldSpawningWave?.Invoke();
            _turnedGoldenWave = true;
        }
    }
}
