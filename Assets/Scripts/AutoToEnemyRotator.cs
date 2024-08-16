using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoToEnemyRotator : MonoBehaviour
{
    private string _enemyTag = "Enemy";

    private string _lockTag = "Lock";

    public bool IsTargetingAtEnemy{ get; private set; }

    private Transform _targetToLookAt;

    [SerializeField] private Transform _player;

    private float _rotationToEnemySpeed = 15f;

    private float _maximalDistanceToLeave = 4f;

    private float _betweenEnemiesChangeRotationDelay = 0.001f;

    private bool _canEnterEnemy = true;

    private TargetToRotate _targetToRotate = TargetToRotate.Enemy;

    private EnemyHealthHandler _targetHealthHandler;

    public Quaternion TargetRotation{ get; private set; }

    private Collider _targetEnemyCollider = null;

    private bool _isTrackingEnemy = false;

    public static AutoToEnemyRotator Instance;

    private void Awake() {
        Instance = this;
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == _enemyTag || other.tag == $"{_enemyTag}Boss") TriggerEnemyTargeting(other);
        else if (other.tag == _lockTag && (BulletsSpawner.Instance.IsGoldenBulletGiven && !BulletsCollecter.Instance.UsedGoldenBullet)) TriggerLockTargeting(other);
    }

    private void TriggerEnemyTargeting(Collider other) {
        if (!IsTargetingAtEnemy) {
            _targetToRotate = TargetToRotate.Enemy;
            _targetToLookAt = other.transform;
            IsTargetingAtEnemy = true;
            _targetHealthHandler = _targetToLookAt.GetComponent<EnemyHealthHandler>() ?? _targetToLookAt.GetComponentInParent<EnemyHealthHandler>();
            //StartEnemyEnteringLimit();
        }
    }

    private void TriggerLockTargeting(Collider other) {
        if (!IsTargetingAtEnemy) {
            _targetToRotate = TargetToRotate.Lock;
            _targetToLookAt = other.transform;
            IsTargetingAtEnemy = true;
            //StartEnemyEnteringLimit();
        }
    }

    private void StartEnemyEnteringLimit() {
        if (_canEnterEnemy) {
            _canEnterEnemy = false;
            StartCoroutine(BetweenEnemiesEnterDelay());
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
        if(_targetToRotate == TargetToRotate.Enemy) {
            if (_targetHealthHandler != null) {
                if (_targetHealthHandler.IsDead) {
                    IsTargetingAtEnemy = false;
                }
            }
            else IsTargetingAtEnemy = false;
        }
    }
    
    private void TryToLeaveTarget() {
        if (_targetToLookAt != null) {
            float distance = Vector3.Distance(_player.position, _targetToLookAt.position);
            if (distance > _maximalDistanceToLeave) {
                IsTargetingAtEnemy = false;
            }
        }
        else IsTargetingAtEnemy = false;
    }

    private void LookAtTargetWhenInTrigger() {
        if (IsTargetingAtEnemy) {
            TargetRotation = Quaternion.LookRotation(_targetToLookAt.position - _player.position);
        }
    }
}
public enum TargetToRotate{ Enemy, Lock }
