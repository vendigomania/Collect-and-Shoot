using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BridgeChangingLocationZoneHandler : MonoBehaviour
{
    public UnityEvent OnPlayerEnteredZone;

    private bool _hasPlayerEnteredTheZone = false;

    private string _playerTag = "Player";

    private void OnTriggerEnter(Collider other) {
        if(other.tag == _playerTag) {
            if (!_hasPlayerEnteredTheZone) {
                OnPlayerEnteredZone?.Invoke();
                _hasPlayerEnteredTheZone = true;
            }
        }
    }
}
