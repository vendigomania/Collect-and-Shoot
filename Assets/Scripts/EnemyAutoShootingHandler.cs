using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAutoShootingHandler : MonoBehaviour
{
    private bool _isShooting = false;

    public UnityEvent OnShotMade;

    [SerializeField] private string _enemyTag = "Enemy";

    [SerializeField] private float _rayLength = 100;

    [SerializeField] private float _betweenShootingIterationDelay = 3;

    [SerializeField] private GameObject _bulletParticlePrefab;

    [SerializeField] private Weapon _currentWeapon;

    [SerializeField] private Vector3 _raycastOffset;

    private int _targetShotsAmount = 0;

    [SerializeField] private ParticleSendingOrigin _particleSendingOrigin;

    [SerializeField] private EnemyHealthHandler _healthHandler;

    private void FixedUpdate() {
        TryToDetectEnemy();
    }

    private void TryToDetectEnemy() {
        if (GameStartHandler.Instance.IsGameStarted && !_healthHandler.IsDead) {
            if (!_isShooting) {
                RaycastHit hit;
                _targetShotsAmount = _currentWeapon.ShotsForOneIteration;
                if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out hit, _rayLength)) {
                    if (hit.collider.tag == _enemyTag) {
                        StartCoroutine(BetweenShootingIterationDelay());
                        MakeAShot();
                        StartCoroutine(TryToContinueShooting(_currentWeapon));
                        _isShooting = true;
                    }
                }
            }
        }
    }

    private IEnumerator BetweenShootingIterationDelay() {
        yield return new WaitForSeconds(_betweenShootingIterationDelay);
        {
            _isShooting = false;
        }
    }

    private IEnumerator TryToContinueShooting(Weapon currentWeapon) {
        yield return new WaitForSeconds(currentWeapon.DelayBetweenEveryShot);
        {
            if (_targetShotsAmount > 0 && !_healthHandler.IsDead) {
                MakeAShot();
                StartCoroutine(TryToContinueShooting(currentWeapon));
            }
            else StopCoroutine(TryToContinueShooting(currentWeapon));
        }
    }

    public void MakeAShot() {
        _targetShotsAmount--;
        OnShotMade?.Invoke();
        SendBulletParicle(_currentWeapon);
    }

    public void SendBulletParicle(Weapon currentWeapon) {
        Bullet instance = Instantiate(_bulletParticlePrefab, currentWeapon.ShootingOrigin.position, Quaternion.identity).GetComponent<Bullet>();
        instance.SentBy = _particleSendingOrigin;
        instance.MovementHandler.TargetDirection = currentWeapon.ShootingOrigin.forward;
        instance.TargetDamage = currentWeapon.Damage;
    }
}
