using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StickmanAnimatorHandler : MonoBehaviour
{
    private int _targerLayerIndex;

    private Animator _targetAnimator;

    private void Awake() {
        _targetAnimator = GetComponent<Animator>();
    }


    public void ChangeLayerWeight(float targetWeight) => _targetAnimator.SetLayerWeight(_targerLayerIndex, targetWeight);

    public void InitializeLayerWeightChanging(int targetLayerIndex) {
        _targerLayerIndex = targetLayerIndex;
    }
}
