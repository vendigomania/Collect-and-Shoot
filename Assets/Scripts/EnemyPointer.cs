using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    [SerializeField] private GameObject _visualPointer;

    public void SetVisibility(bool targetState) {
        if(_visualPointer.activeSelf != targetState) _visualPointer.SetActive(targetState);
    }
}
