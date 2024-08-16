using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBulletHitReaction : MonoBehaviour
{
    [SerializeField] private StickmanHealthHandler _healthHandler;

    [SerializeField] private HelperHealthHandler _helperHealthHandler;

    private string _bulletTag = "BulletParticle";

    [SerializeField] private UsedBy _usedBy = UsedBy.Player;

    [SerializeField] private ParticleSystem _bloodEffect;

    public UnityEvent OnGotBullet;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == _bulletTag) {
            RegisterHit(other.gameObject);
        }
    }

    private void RegisterHit(GameObject bullet) {
        Bullet instance = bullet.GetComponent<Bullet>();
        if (instance.SentBy == ParticleSendingOrigin.Enemy) {
            if (_usedBy == UsedBy.Player) _healthHandler.ChangeHealthValue(-instance.TargetDamage);
            else _helperHealthHandler.ChangeHealthValue(-instance.TargetDamage);
            instance.SelfDestroyOnHit();
            CreateEffect(bullet.transform);
            OnGotBullet?.Invoke();
        }
    }

    private void CreateEffect(Transform bullet) {
        /*_bloodEffect.transform.LookAt(bullet);
        _bloodEffect.Play(true);*/
    }
}
public enum UsedBy{ Player, Helper }
