using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvicePanelToCameraRotator : MonoBehaviour
{
    private void LateUpdate() {
        RotateToCamera();
    }

    private void RotateToCamera() {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
    }
}
