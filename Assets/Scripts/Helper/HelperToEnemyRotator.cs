using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperToEnemyRotator : MonoBehaviour
{
    private string _enemyTag = "Enemy";

    private string _lockTag = "Lock";

    public bool IsTargetingAtEnemy { get; private set; }

    private Transform _targetToLookAt;

    private Transform _player;

    private float _rotationToEnemySpeed = 15f;

    private float _maximalDistanceToLeave = 5f;

    private float _betweenEnemiesChangeRotationDelay = 1.5f;

    private bool _canEnterEnemy = true;

    private TargetToRotate _targetToRotate = TargetToRotate.Enemy;

    private EnemyHealthHandler _targetHealthHandler;

    public Quaternion TargetRotation { get; private set; }

    public static HelperToEnemyRotator Instance;

    private void Awake() {
        Instance = this;
        _player = transform.parent;
    }

    private void OnTriggerStay(Collider other) {
        if(!HelperHealthHandler.Instance.IsDead) if (other.tag == _enemyTag || other.tag == $"{_enemyTag}Boss") TriggerEnemyTargeting(other);
    }

    private void TriggerEnemyTargeting(Collider other) {
        if (_canEnterEnemy) {
            if (!IsTargetingAtEnemy) {
                _targetToRotate = TargetToRotate.Enemy;
                _targetToLookAt = other.transform;
                IsTargetingAtEnemy = true;
                _canEnterEnemy = false;
                _targetHealthHandler = _targetToLookAt.GetComponent<EnemyHealthHandler>() ?? _targetToLookAt.GetComponentInParent<EnemyHealthHandler>();
                StartCoroutine(BetweenEnemiesEnterDelay());
            }
        }
    }

    private IEnumerator BetweenEnemiesEnterDelay() {
        yield return new WaitForSeconds(_betweenEnemiesChangeRotationDelay);
        {
            _canEnterEnemy = true;
        }
    }

    private void Update() {
        TryToLeaveTarget();
        AllowToSignNextEnemyOnCurrentKilled();
        LookAtTargetWhenInTrigger();
    }

    private void AllowToSignNextEnemyOnCurrentKilled() {
        if (_targetToRotate == TargetToRotate.Enemy) {
            if (IsTargetingAtEnemy) {
                if (_targetHealthHandler != null && _targetToLookAt != null) {
                    if (_targetHealthHandler.IsDead) {
                        IsTargetingAtEnemy = false;
                    }
                }
                else IsTargetingAtEnemy = false;
            }
        }
    }

    private void TryToLeaveTarget() {
        if (IsTargetingAtEnemy) {
            if (_targetToLookAt != null) {
                if (Vector3.Distance(_player.position, _targetToLookAt.position) > _maximalDistanceToLeave) {
                    IsTargetingAtEnemy = false;
                }
            }
        }
    }

    private void LookAtTargetWhenInTrigger() {
        if (IsTargetingAtEnemy) {
            TargetRotation = Quaternion.LookRotation(_targetToLookAt.position - _player.position);
        }
    }
}
