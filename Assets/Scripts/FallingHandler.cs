using UnityEngine.Events;
using UnityEngine;

public class FallingHandler : MonoBehaviour
{
    public UnityEvent OnGetToWaterZone;

    [SerializeField] private Vector3 _offset;

    [SerializeField] private float _rayLength;

    private bool _didGetToWaterZone = false;

    private void FixedUpdate() {
        CheckGroundUnderFeet();
    }

    private void CheckGroundUnderFeet() {
        if(!Physics.Raycast(transform.position + _offset, -transform.up, _rayLength)) {
            if (!_didGetToWaterZone) {
                OnGetToWaterZone?.Invoke();
                _didGetToWaterZone = true;
            }
        }
    }
}
