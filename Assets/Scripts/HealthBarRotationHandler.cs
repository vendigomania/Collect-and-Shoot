using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarRotationHandler : MonoBehaviour
{
    private Transform _camera;

    private void Awake() {
        _camera = Camera.main.transform;
    }

    private void LateUpdate() {
        AlwaysLookAtCamera();
    }

    private void AlwaysLookAtCamera() {
        Quaternion targetRotation = Quaternion.LookRotation(_camera.position - transform.position);
        transform.rotation = targetRotation;
    }
}
