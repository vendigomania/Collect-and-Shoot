using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    public float CurrentSpeed{ get => _movementSpeed; }

    private float _defaultSpeed = 3f;

    private Rigidbody _rigidbody;

    public bool IsWalking { get; private set; } = false;

    [SerializeField] private UpgradeHandler _speedUpgrader;

    public static PlayerMovementHandler Instance;

    private string _speedPlayerPrefsKey = "Speed";

    [SerializeField] private Joystick _movementJoystick;

    private void Awake() {
        Instance = this;
        _movementSpeed = _defaultSpeed;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void ChangeSpeedValueBy() {
        _movementSpeed += (_movementSpeed * (float)(_speedUpgrader.UpgradeValue / 100f)) * 1.1f;
    }

    private void FixedUpdate() {
        MovePlayer();
        UpdateIsWalkingState();
    }

    private void UpdateIsWalkingState() => IsWalking = (_movementJoystick.Vertical != 0 || _movementJoystick.Horizontal != 0);

    private void MovePlayer() {
        if (GameStartHandler.Instance.IsGameStarted && !StickmanHealthHandler.Instance.IsDead) {
            Vector3 direction = new Vector3(_movementJoystick.Horizontal, 0, _movementJoystick.Vertical);
            _rigidbody.velocity = -direction * _movementSpeed;
        }
    }
}
