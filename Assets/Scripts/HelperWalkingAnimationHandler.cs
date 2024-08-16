using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HelperWalkingAnimationHandler : MonoBehaviour
{
    private const string _walkingStateName = "IsWalking";

    private Animator _targetAnimator;

    private void Awake() {
        _targetAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate() => TryToTurnWalkingAnimation();

    private void TryToTurnWalkingAnimation() {
        _targetAnimator.SetBool(_walkingStateName, HelperMovementHandler.Instance.IsWalking);
    }
}
