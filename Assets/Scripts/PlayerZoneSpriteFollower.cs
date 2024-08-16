using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneSpriteFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void LateUpdate() {
        FollowPlayer();
    }

    private void FollowPlayer() => transform.position = _player.position;
}
