using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoShootingHandler : MonoBehaviour {
    private bool _isShooting = false;

    public UnityEvent OnShotMade, OnGoldenBulletUsed;

    [SerializeField] private string _enemyTag = "Enemy";

    private string _lockTag = "Lock";

    [SerializeField] private float _rayLength = 100;

    [SerializeField] private Vector3 _raycastOffset;

    [SerializeField] private float _betweenShootingIterationDelay = 3;

    [SerializeField] private GameObject _bulletParticlePrefab, _goldenBulletParticlePrefab;

    [SerializeField] private BulletsCollecter _bulletsCollector;

    public bool CanMakeMultipleShots = false;

    [SerializeField] private GoldKeyThrowHandler _goldKeyThrowHandler;

    private bool _enemyInShootingRange = false;

    private RaycastHit _hit;

    public static AutoShootingHandler Instance;

    private float _exceptionalHitDefaultChance = 0.05f;

    [HideInInspector] public float CriticalHitChance, FreezingHitChance, BurningHitChance;

    private int _targetShotsAmount = 0;

    [SerializeField] private ParticleSendingOrigin _particleSendingOrigin;

    private GameObject _targetLock = null;

    private EnemyHealthHandler _targetEnemyHealthHandler = null;

    [SerializeField] private UpgradeHandler _shootingSpeedUpgrader, _criticalHitUpgrader, _freezingHitUpgrader, _burningHitUpgrader;

    private void Awake() {
        Instance = this;
        CriticalHitChance = 0f;
        FreezingHitChance = 0f;
        BurningHitChance = 0f;
    }

    private void FixedUpdate() {
        UpdateEnemyVisibility();
        TryToDetectEnemy();
        TryToOpenTheLock();
    }

    public void UpgradeCriticalHitChance() {
        CriticalHitChance = (float)(_criticalHitUpgrader.UpgradeValue / 100f);
    }

    public void UpgradeFreezingHitChance() {
        FreezingHitChance = (float)(_freezingHitUpgrader.UpgradeValue / 100f);
    }

    public void UpgradeBurningHitChance() {
        BurningHitChance = (float)(_burningHitUpgrader.UpgradeValue / 100f);
    }

    public void UpgradeShootingSpeed() {
        float decreaseAmount = (_betweenShootingIterationDelay * (float)(_shootingSpeedUpgrader.UpgradeValue / 100f));
        float minimalAmount = 0.1f;
        if (_betweenShootingIterationDelay - decreaseAmount >= minimalAmount) {
            _betweenShootingIterationDelay -= decreaseAmount;
        }
    }

    private void UpdateEnemyVisibility() {
        _enemyInShootingRange = Physics.Raycast(transform.position + _raycastOffset, transform.forward, out _hit, _rayLength);
    }

    public void ChangeMultipleShotsUsage(bool targetState) => CanMakeMultipleShots = targetState;

    private void TryToDetectEnemy() {
        if (GameStartHandler.Instance.IsGameStarted) {
            if (!_isShooting) {
                WeaponChanger weaponChanger = WeaponChanger.Instance;
                Weapon currentWeapon = weaponChanger.AllWeapons[weaponChanger.CurrentWeaponIndex];
                //Debug.DrawRay(transform.position + _raycastOffset, transform.forward * _rayLength, Color.green);
                if (_enemyInShootingRange) {
                    if (!StickmanHealthHandler.Instance.IsDead) {
                        if (_hit.collider.tag == _enemyTag || _hit.collider.tag == $"{_enemyTag}Boss") {
                            if (_bulletsCollector.PickedBullets.Count > 0 && !_bulletsCollector.GoldenBulletIsTheOne()) {
                                StartShooting(currentWeapon, false);
                                _targetEnemyHealthHandler = _hit.collider.GetComponent<EnemyHealthHandler>() ?? _hit.collider.GetComponentInParent<EnemyHealthHandler>();
                            }
                        }
                    }
                }
            }
        }
    }

    private void TryToOpenTheLock() {
        if (_bulletsCollector.HasGoldenBullet()) {
            float maximalDistance = 2f;
            LockBulletHitHandler[] locks = FindObjectsOfType<LockBulletHitHandler>();
            for (int i = 0; i < locks.Length; i++) {
                if (!locks[i].IsLockDestroyed) {
                    if (Vector3.Distance(transform.position, locks[i].transform.position) <= maximalDistance) {
                        _targetLock = locks[i].gameObject;
                        _goldKeyThrowHandler.ThrowGoldKeyToLock(_targetLock);
                        return;
                    }
                }
            }
        }
    }

    private void StartShooting(Weapon currentWeapon, bool useGoldenBullet) {
        _targetShotsAmount = currentWeapon.ShotsForOneIteration;
        if (!useGoldenBullet) if (CanMakeMultipleShots) _bulletsCollector.UseLastBullet(useGoldenBullet);
        StartCoroutine(BetweenShootingIterationDelay());
        StartCoroutine(TryToContinueShooting(currentWeapon, useGoldenBullet));
        _isShooting = true;
    }

    private IEnumerator BetweenShootingIterationDelay() {
        yield return new WaitForSeconds(_betweenShootingIterationDelay);
        {
            _isShooting = false;
        }
    }

    private IEnumerator TryToContinueShooting(Weapon currentWeapon, bool useGoldenBuullet) {
        yield return new WaitForSeconds(currentWeapon.DelayBetweenEveryShot);
        {
            if (_targetShotsAmount > 0 && !StickmanHealthHandler.Instance.IsDead) {
                if (_enemyInShootingRange) {
                    MakeAShot(currentWeapon, useGoldenBuullet);
                    StartCoroutine(TryToContinueShooting(currentWeapon, useGoldenBuullet));
                }
            }
            else StopCoroutine(TryToContinueShooting(currentWeapon, useGoldenBuullet));
        }
    }

    public void MakeAShot(Weapon currentWeapon, bool useGoldenBullet) {
        if (!useGoldenBullet) {
            if (!_targetEnemyHealthHandler.IsDead) {
                _targetShotsAmount--;
                OnShotMade?.Invoke();
                SendBulletParicle(currentWeapon, useGoldenBullet);
                if (!useGoldenBullet) {
                    if (!CanMakeMultipleShots) _bulletsCollector.UseLastBullet(useGoldenBullet);
                }
                else _bulletsCollector.UseLastBullet(useGoldenBullet);
            }
        }
    }

    public void SendBulletParicle(Weapon currentWeapon, bool useGoldenBullet) {
        Bullet instance = Instantiate(useGoldenBullet ? _goldenBulletParticlePrefab : _bulletParticlePrefab, currentWeapon.ShootingOrigin.position, Quaternion.identity).GetComponent<Bullet>();
        instance.MovementHandler.TargetDirection = currentWeapon.ShootingOrigin.forward;
        instance.TargetDamage = currentWeapon.Damage;
        instance.SentBy = _particleSendingOrigin;
        instance.transform.rotation = Quaternion.LookRotation(instance.MovementHandler.TargetDirection);
    }
}
