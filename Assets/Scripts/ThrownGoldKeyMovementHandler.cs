using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownGoldKeyMovementHandler : MonoBehaviour
{
    [HideInInspector] public Transform TargetLock;

    private float _rotationSpeed = 5f;

    private float _shrinkingSpeed = 5f;

    private float _movementSpeed = 7.5f;

    [SerializeField] private Vector3 _shrinkedSize;

    [SerializeField] private float _maximalDistance = 0.25f;

    private void Update() {
        if(TargetLock != null) {
            RotateToLockGradually();
            ShrinkToLockGradually();
            MoveToLockGradually();
            TryToInsertKey();
        }
    }

    private void RotateToLockGradually() {
        Quaternion targetRotation = Quaternion.LookRotation(TargetLock.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void ShrinkToLockGradually() {
        transform.localScale = Vector3.Slerp(transform.localScale, _shrinkedSize, _shrinkingSpeed * Time.deltaTime);
    }

    private void MoveToLockGradually() {
        transform.position = Vector3.Slerp(transform.position, TargetLock.position, _movementSpeed * Time.deltaTime);
    }

    private void TryToInsertKey() {
        if(Vector3.Distance(transform.position, TargetLock.position) <= _maximalDistance) {
            LockBulletHitHandler instance = TargetLock.GetComponent<LockBulletHitHandler>();
            instance.ForceUnlocking();
            Destroy(gameObject);
        }
    }
}
