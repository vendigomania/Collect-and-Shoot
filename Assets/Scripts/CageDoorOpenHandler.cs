using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CageDoorOpenHandler : MonoBehaviour
{
    [SerializeField] private LockBulletHitHandler[] _padlocks;

    [SerializeField] private Animator _doorAnimator;

    [SerializeField] private ParticleSystem _destroyingEffect;

    private float _destroyCageAfter = 3;

    public UnityEvent OnDoorOpened;

    [field: SerializeField] public bool IsDoorOpened { get; private set; } = false;

    public void TryToOpenDoor() {
        int padlocksUnlocked = 0;
        for (int i = 0; i < _padlocks.Length; i++) {
            if (_padlocks[i].IsLockDestroyed) padlocksUnlocked++;
        }

        if(padlocksUnlocked >= _padlocks.Length) {
            if (!IsDoorOpened) {
                OpenDoor();
                IsDoorOpened = true;
                OnDoorOpened?.Invoke();
               /* EnemyWaveSpawner.Instance.TargetLevelToNotify++;
                SupersonicWisdom.Api.NotifyLevelCompleted(EnemyWaveSpawner.Instance.TargetLevelToNotify, null);
                Debug.Log($"Complete Level {EnemyWaveSpawner.Instance.TargetLevelToNotify}");*/
            }
        }
    }

    public void StartCageDestroying() {
        StartCoroutine(DestroyCageAfter());
    }

    private IEnumerator DestroyCageAfter() {
        yield return new WaitForSeconds(_destroyCageAfter);
        {
            DestroyCage();
        }
    }

    private void DestroyCage() {
        _destroyingEffect.Play(true);
        Destroy(transform.parent.gameObject);
    }
    
    public void OpenDoor() {
        string triggerName = "DoorOpen";
        _doorAnimator.SetTrigger(triggerName);
    }
}
