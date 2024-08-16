using UnityEngine.AI;
using UnityEngine;

public class HelperMovementHandler : MonoBehaviour
{
    [SerializeField] private Transform _targetToFollow;

    [SerializeField] private NavMeshAgent _agent;

    public bool CanWalk = false;

    public bool IsWalking { get; private set; } = false;

    public static HelperMovementHandler Instance;

    private void Awake() {
        Instance = this;
    }

    private void FixedUpdate() {
        UpdateHelperSpeed();
        MovePlayer();
        UpdateIsWalkingState();
    }

    public void UpdateHelperSpeed() => _agent.speed = PlayerMovementHandler.Instance.CurrentSpeed;

    private void UpdateIsWalkingState() {
        Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);
        float minVelocityToRegister = 0.075f;
        IsWalking = (localVelocity.z >= minVelocityToRegister || localVelocity.z <= -minVelocityToRegister);
    }

    private void MovePlayer() {
        if (CanWalk) {
            if (GameStartHandler.Instance.IsGameStarted && !HelperHealthHandler.Instance.IsDead) {
                _agent.SetDestination(_targetToFollow.position);
            }
        }
    }

    public void ChangeCanWalkState(bool state) => CanWalk = state;
}
