using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HelperShootingHandler : MonoBehaviour
{
    private bool _isShooting = false;

    public UnityEvent OnShotMade;

    [SerializeField] private string _enemyTag = "Enemy";

    private string _lockTag = "Lock";

    [SerializeField] private float _rayLength = 100;

    [SerializeField] private Vector3 _raycastOffset;

    [SerializeField] private float _betweenShootingIterationDelay = 3;

    [SerializeField] private GameObject _bulletParticlePrefab;

    [SerializeField] private HelperBulletsCollecter _bulletsCollector;

    [SerializeField] private Weapon _currentWeapon;

    private int _targetShotsAmount = 0;

    [SerializeField] private ParticleSendingOrigin _particleSendingOrigin;

    [SerializeField] private UpgradeHandler _shootingSpeedUpgrader;

    private EnemyHealthHandler _targetEnemyHealthHandler = null;

    private void FixedUpdate() {
        TryToDetectEnemy();
    }

    public void UpgradeShootingSpeed() {
        float decreaseAmount = (_betweenShootingIterationDelay * (float)(_shootingSpeedUpgrader.UpgradeValue / 100f));
        float minimalAmount = 1f;
        if (_betweenShootingIterationDelay - decreaseAmount >= minimalAmount) {
            _betweenShootingIterationDelay -= decreaseAmount;
        }
    }

    private void TryToDetectEnemy() {
        if (GameStartHandler.Instance.IsGameStarted) {
            if (!_isShooting) {
                RaycastHit hit;
                Weapon currentWeapon = _currentWeapon;
                _targetShotsAmount = currentWeapon.ShotsForOneIteration;
                //Debug.DrawRay(transform.position + _raycastOffset, transform.forward * _rayLength, Color.green);
                if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out hit, _rayLength)) {
                    if (!HelperHealthHandler.Instance.IsDead) {
                        if (hit.collider.tag == _enemyTag || hit.collider.tag == $"{_enemyTag}Boss") {
                            if (_bulletsCollector.PickedBullets.Count > 0 && !_bulletsCollector.GoldenBulletIsTheOne()) {
                                StartShooting(currentWeapon, false);
                                _targetEnemyHealthHandler = hit.collider.GetComponent<EnemyHealthHandler>() ?? hit.collider.GetComponentInParent<EnemyHealthHandler>();
                            }
                        }
                    }
                }
            }
        }
    }

    private void StartShooting(Weapon currentWeapon, bool useGoldenBullet) {
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
                MakeAShot(currentWeapon, useGoldenBuullet);
                StartCoroutine(TryToContinueShooting(currentWeapon, useGoldenBuullet));
            }
            else StopCoroutine(TryToContinueShooting(currentWeapon, useGoldenBuullet));
        }
    }

    public void MakeAShot(Weapon currentWeapon, bool useGoldenBullet) {
        if (!_targetEnemyHealthHandler.IsDead) {
            _targetShotsAmount--;
            OnShotMade?.Invoke();
            SendBulletParicle(currentWeapon);
            _bulletsCollector.UseLastBullet(useGoldenBullet);
        }
    }

    public void SendBulletParicle(Weapon currentWeapon) {
        Bullet instance = Instantiate(_bulletParticlePrefab, currentWeapon.ShootingOrigin.position, Quaternion.identity).GetComponent<Bullet>();
        instance.MovementHandler.TargetDirection = currentWeapon.ShootingOrigin.forward;
        instance.TargetDamage = currentWeapon.Damage;
        instance.SentBy = _particleSendingOrigin;
    }
}
