using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPlayerFollower : MonoBehaviour
{
    private Transform _target;

    [SerializeField] private Animator _animator;

    private string _playerTag = "Player";

    [SerializeField] private EnemyHealthHandler _healthHandler;

    private float _minimalSpeedToStayWalking = 0.15f;

    [SerializeField] private bool _canFollowPlayer = true;

    private NavMeshAgent _agent;

    private void Awake() {
        _target = GameObject.FindGameObjectWithTag(_playerTag).transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate() => FollowPlayer();

    private void FollowPlayer() {
        if (GameStartHandler.Instance.IsGameStarted && !_healthHandler.IsDead) {
            if (_canFollowPlayer) {
                Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);
                ControlWalkingAnimation(localVelocity.z);
                _agent.SetDestination(_target.position);
            }
        }
    }

    private void ControlWalkingAnimation(float localVelocityZ) => _animator.SetBool("IsWalking", localVelocityZ > _minimalSpeedToStayWalking);

    public void ChangeCanFollowPlayerState(bool targetState) => _canFollowPlayer = targetState;
}
