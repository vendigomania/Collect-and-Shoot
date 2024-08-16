using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletParticleMovementHandler : MonoBehaviour
{
    [SerializeField] private float _flyingSpeed = 5;

    [SerializeField] private float _enemyFlyingSpeed = 7.5f;

    private float _finalFlyingSpeed = 0;

    private Rigidbody _rigidbody;

    private Bullet _instance;

    public Vector3 TargetDirection;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _instance = GetComponent<Bullet>();
    }

    private void UpdateBulletSpeed() => _finalFlyingSpeed = (_instance.SentBy == ParticleSendingOrigin.Player) ? _flyingSpeed : _enemyFlyingSpeed;

    private void FixedUpdate() {
        UpdateBulletSpeed();
        MoveBulletForward();
    }

    private void MoveBulletForward() => _rigidbody.MovePosition(transform.position + TargetDirection * Time.smoothDeltaTime * _finalFlyingSpeed);
}
