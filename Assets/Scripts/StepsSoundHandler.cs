using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _targetSource;

    [SerializeField] private Joystick _targetJoystick;

    private void FixedUpdate() => TurnSourceOnWalking();

    private void TurnSourceOnWalking() => _targetSource.enabled = !StickmanHealthHandler.Instance.IsDead ? (_targetJoystick.Vertical != 0 || _targetJoystick.Horizontal != 0) : false;
}
