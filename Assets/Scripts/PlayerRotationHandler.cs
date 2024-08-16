using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour
{
    [SerializeField] private float _followingSpeed = 22.5f;

    private float _targetAngle = 0;

    [SerializeField] private bool _rotateFromJoystick = true;

    [SerializeField] private Joystick _rotationJoystick;

    private void Update() => RotatePlayer();
    
    private void RotatePlayer() {
        if (GameStartHandler.Instance.IsGameStarted && !StickmanHealthHandler.Instance.IsDead) {
            if (_rotateFromJoystick) {
                if (_rotationJoystick.Vertical != 0 || _rotationJoystick.Horizontal != 0) {
                    _targetAngle = Mathf.Atan2(-_rotationJoystick.Horizontal, -_rotationJoystick.Vertical) * Mathf.Rad2Deg;
                }

            }
            Quaternion toEnemyRotationRaw = AutoToEnemyRotator.Instance.TargetRotation;
            Quaternion joystickRotation = Quaternion.Euler(0, _targetAngle, 0);
            Quaternion toEnemyRotation = new Quaternion(0, toEnemyRotationRaw.y, 0, toEnemyRotationRaw.w);
            transform.rotation = Quaternion.Slerp(transform.rotation, (AutoToEnemyRotator.Instance.IsTargetingAtEnemy) ? toEnemyRotation : joystickRotation, _followingSpeed * Time.smoothDeltaTime);
        }
    }
}
