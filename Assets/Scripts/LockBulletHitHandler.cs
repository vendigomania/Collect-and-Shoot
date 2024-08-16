using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class LockBulletHitHandler : MonoBehaviour
{
    private string _bulletTag = "BulletParticle";

    [field: SerializeField] public bool IsLockDestroyed { get; private set; } = false;

    [SerializeField] private float _delayAfterUnlocking = 1.5f;

    public static bool GetHit = false;

    [SerializeField] private MeshRenderer[] _meshRenderers;

    [SerializeField] private ParticleSystem _destroyingFX;

    public UnityEvent OnPadlockUnlocked, AfterUnlockingDelay;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == _bulletTag) {
            TryToUnlockLock(other);
        }
    }

    private void TryToUnlockLock(Collider other) {
        if (!IsLockDestroyed && !GetHit) {
            if (!EnemiesExistanceHandler.IsAEnemyOnScene()) {
                Bullet instance = other.GetComponent<Bullet>();
                if (instance.SentBy == ParticleSendingOrigin.Player) {
                    AutoShootingHandler.Instance.OnGoldenBulletUsed?.Invoke();
                    GetHit = true;
                    _destroyingFX.Play(true);
                    IsLockDestroyed = true;
                    instance.SelfDestroyOnHit();
                    BulletsCollecter.Instance.UsedGoldenBullet = true;
                    OnPadlockUnlocked?.Invoke();
                    StartCoroutine(AfterUnlockingDelayRoutine());
                }
            }
        }
    }

    public void ForceUnlocking() {
        if(!IsLockDestroyed && !GetHit) {
            if (!EnemiesExistanceHandler.IsAEnemyOnScene()) {
                AutoShootingHandler.Instance.OnGoldenBulletUsed?.Invoke();
                GetHit = true;
                _destroyingFX.Play(true);
                IsLockDestroyed = true;
                BulletsCollecter.Instance.UsedGoldenBullet = true;
                OnPadlockUnlocked?.Invoke();
                StartCoroutine(AfterUnlockingDelayRoutine());
            }
        }
    }

    private IEnumerator AfterUnlockingDelayRoutine() {
        yield return new WaitForSeconds(_delayAfterUnlocking);
        {
            AfterUnlockingDelay?.Invoke();
        }
    }

    public void DisableAllVisual() {
        for (int i = 0; i < _meshRenderers.Length; i++) {
            _meshRenderers[i].enabled = false;
        }
    }
}
