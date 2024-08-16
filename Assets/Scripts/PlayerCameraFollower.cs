using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 _offset, _rotatingOffset;

    [SerializeField] private float _followingSpeed, _cameraRotatingSpeed;

    [SerializeField] private Transform _target;

    private void Update() {
        FollowPlayer();
    }

    private void FollowPlayer() {
        if (GameStartHandler.Instance.IsGameStarted) {
            Vector3 targetPosition = _target.position + _offset;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _followingSpeed * Time.smoothDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation((_target.position + _rotatingOffset) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _cameraRotatingSpeed * Time.smoothDeltaTime);
        }
    }
}
