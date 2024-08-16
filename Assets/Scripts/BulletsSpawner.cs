using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _simpleBulletPrefab, _goldenBulletPrefab, _helperBulletPrefab;

    [SerializeField] private bool _canSpawnHelperBullets = false;

    [SerializeField] private float _betweenBulletSpawningDelay = 2;

    [SerializeField] private float _maximalZOffset, _minimalZOffset;

    [SerializeField] private float _maximalXOffset, _minimalXOffset;

    private GameObject _player;

    public static BulletsSpawner Instance;

    public bool CanSpawnGoldenBullet = true;

    public bool IsGoldenBulletGiven = false;

    private string _playerTag = "Player";

    private void Awake() {
        Instance = this;
        _player = GameObject.FindGameObjectWithTag(_playerTag);
        StartSimpleBulletSpawning();
    }

    private void StartSimpleBulletSpawning() {
        StartCoroutine(BetweenSimpleBulletSpawnDelay());
    }

    private IEnumerator BetweenSimpleBulletSpawnDelay() {
        yield return new WaitForSeconds(_betweenBulletSpawningDelay);
        {
            if (!StickmanHealthHandler.Instance.IsDead && GameStartHandler.Instance.IsGameStarted) {
                SpawnBulletRandomlyAroundPlayer(_simpleBulletPrefab);
                if (!HelperHealthHandler.Instance.IsDead) {
                    if (_canSpawnHelperBullets) {
                        SpawnBulletRandomlyAroundPlayer(_helperBulletPrefab);
                    }
                }
            }
            StartCoroutine(BetweenSimpleBulletSpawnDelay());
        }
    }

    public void SpawnGoldenBulletInFrontOfPlayer() {
        if (!IsGoldenBulletGiven) {
            float zOffset = 3;
            Instantiate(_goldenBulletPrefab, _player.transform.position + (_player.transform.forward * zOffset) + new Vector3(0, _goldenBulletPrefab.transform.position.y, 0), Quaternion.identity);
            IsGoldenBulletGiven = true;
        }
    }

    public void SpawnBulletRandomlyAroundPlayer(GameObject bullet) {
        float xPosition = Random.Range(_minimalXOffset, _maximalXOffset);
        float zPosition = Random.Range(_minimalZOffset, _maximalZOffset);
        Instantiate(bullet, transform.position + new Vector3(xPosition, bullet.transform.position.y, zPosition), Quaternion.identity);
    }
}
