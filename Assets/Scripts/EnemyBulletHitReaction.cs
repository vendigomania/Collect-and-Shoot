using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBulletHitReaction : MonoBehaviour
{
    [SerializeField] private EnemyHealthHandler _healthHandler;

    private string _bulletTag = "BulletParticle";

    [SerializeField] private ParticleSystem _bloodEffect, _freezingStart, _burning, _exceptionalHitEnd, _criticalHitEffect;

    private bool _exceptionalHitHasBeenApplied = false;

    private Animator _animator;

    private NavMeshAgent _agent;

    private int _burningDamagePerSecond = 30;

    private float _burningDuration = 5f;

    private float _defaultSpeed, _defaultAnimatorSpeed;

    private float _disableFreezingAfter = 5f;

    [SerializeField] private float _freezedSpeed, _freezedAnimatorSpeed;

    private bool _isBurningDamageApplying = false;

    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _defaultSpeed = _agent.speed;
        _defaultAnimatorSpeed = _animator.speed;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == _bulletTag) {
            RegisterHit(other.gameObject);
        }
    }

    private void RegisterHit(GameObject bullet) {
        Bullet instance = bullet.GetComponent<Bullet>();
        if(instance.SentBy == ParticleSendingOrigin.Player) {
            _healthHandler.ChangeHealthValue(-instance.TargetDamage);
            TurnCriticalHit(instance);
            TurnFreezingHit();
            TurnBurningHit();
            instance.SelfDestroyOnHit();
            CreateEffect(bullet);
        }
    }

    private void CreateEffect(GameObject bullet) {
        /*_bloodEffect.transform.LookAt(bullet.transform);
        _bloodEffect.Play(true);*/
    }

    public void TurnCriticalHit(Bullet instance) {
        if (!_exceptionalHitHasBeenApplied) {
            float requiredChance = 1f - AutoShootingHandler.Instance.CriticalHitChance;
            float randomValue = Random.value;
            if (randomValue >= requiredChance) {
                _criticalHitEffect.Play(true);
                _healthHandler.ChangeHealthValue(-instance.TargetDamage);
                _exceptionalHitHasBeenApplied = true;
            }
        }
    }

    public void TurnFreezingHit() {
        if (!_exceptionalHitHasBeenApplied) {
            float requiredChance = 1f - AutoShootingHandler.Instance.FreezingHitChance;
            float randomValue = Random.value;
            if (randomValue >= requiredChance) {
                _freezingStart.Play(true);
                _exceptionalHitHasBeenApplied = true;
                _agent.speed = _freezedSpeed;
                _animator.speed = _freezedAnimatorSpeed;
                StartCoroutine(FreezingDisable());
            }
        }
    }

    private IEnumerator FreezingDisable() {
        yield return new WaitForSeconds(_disableFreezingAfter);
        {
            _exceptionalHitEnd.Play(true);
            _agent.speed = _defaultSpeed;
            _animator.speed = _defaultAnimatorSpeed;
        }
    }

    public void TurnBurningHit() {
        if (!_exceptionalHitHasBeenApplied) {
            float requiredChance = 1f - AutoShootingHandler.Instance.BurningHitChance;
            float randomValue = Random.value;
            if (randomValue >= requiredChance) {
                _burning.Play(true);
                StartCoroutine(BurningDuration());
                _exceptionalHitHasBeenApplied = true;
                _isBurningDamageApplying = true;
                StartCoroutine(DamageApplying());
            }
        }
    }

    private IEnumerator DamageApplying() {
        yield return new WaitForSeconds(1);
        {
            if (_isBurningDamageApplying) {
                _healthHandler.ChangeHealthValue(-_burningDamagePerSecond);
                StartCoroutine(DamageApplying());
            }
        }
    }

    private IEnumerator BurningDuration() {
        yield return new WaitForSeconds(_burningDuration);
        {
            _isBurningDamageApplying = false;
            _exceptionalHitEnd.Play(true);
            _burning.Stop(true);
        }
    }
}
