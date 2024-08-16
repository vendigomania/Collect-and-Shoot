using UnityEngine.AI;
using UnityEngine;

public class HelperRotationHandler : MonoBehaviour
{
    private float _followingSpeed = 17.5f;

    [SerializeField] private NavMeshAgent _agent;

    private void Update() => RotatePlayer();

    private void RotatePlayer() {
        if (GameStartHandler.Instance.IsGameStarted) {
            Quaternion toEnemyRotationRaw = HelperToEnemyRotator.Instance.TargetRotation;
            Quaternion toEnemyRotation = new Quaternion(0, toEnemyRotationRaw.y, 0, toEnemyRotationRaw.w);
            _agent.updateRotation = !(HelperToEnemyRotator.Instance.IsTargetingAtEnemy);
            if (HelperToEnemyRotator.Instance.IsTargetingAtEnemy && !HelperHealthHandler.Instance.IsDead) {
                transform.rotation = Quaternion.Slerp(transform.rotation, toEnemyRotation, _followingSpeed * Time.smoothDeltaTime);
            }
        }
    }
}
