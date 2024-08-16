using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NearEnemyHitHandler : MonoBehaviour
{
    private string _playerTag = "Player";

    private bool _isPlayerInZone = false;

    public UnityEvent OnEnemyTriggered, OnPlayerHit;

    private bool _enemyHasBeenTriggered = false;

    private bool _isHitting = false;

    [SerializeField] private int _damage;

    [SerializeField] private EnemyHealthHandler _healthHandler;

    [SerializeField] private float _tryToApplyHitAfter = 1;

    private void OnTriggerStay(Collider other) {
        if(other.tag == _playerTag) {
            if (!_enemyHasBeenTriggered) {
                if (!_isHitting) {
                    TriggerEnemyToHit(other);
                    _enemyHasBeenTriggered = true;
                    _isHitting = true;
                }
            }
            _isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == _playerTag) {
            _isPlayerInZone = false;
        }
    }

    private void TriggerEnemyToHit(Collider player) {
        if (!_healthHandler.IsDead) {
            OnEnemyTriggered?.Invoke();
        }
        TryToApplyHit(player);
    }

    private void TryToApplyHit(Collider player) {
        StartCoroutine(HitApplyingDelay(player));
    }

    private IEnumerator HitApplyingDelay(Collider player) {
        yield return new WaitForSeconds(_tryToApplyHitAfter);
        {
            if (_isPlayerInZone) {
                if (!_healthHandler.IsDead) {
                    StickmanHealthHandler playerHealth = player.GetComponent<StickmanHealthHandler>();
                    if (!playerHealth.IsDead) {
                        playerHealth.ChangeHealthValue(-_damage);
                        playerHealth.CreateBloodEffect(transform);
                        OnPlayerHit?.Invoke();
                    }
                }
            }
            _enemyHasBeenTriggered = false;
            _isHitting = false;
        }
    }
}
