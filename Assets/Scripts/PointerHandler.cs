using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHandler : MonoBehaviour
{
    private Transform _player;

    [SerializeField] private Transform _screenPointer;

    [SerializeField] private GameObject _visibleSpriteOfScreenPointer;

    [SerializeField] private Quaternion[] _pointerRotations;

    [HideInInspector] public Transform Enemy;

    private string _playerTag = "Player";

    private void Awake() {
        InitializeFields();
    }

    private void InitializeFields() {
        _player = GameObject.FindGameObjectWithTag(_playerTag).transform;
    }

    private void FixedUpdate() {
        SynchronizeEnemiesWithPointers();
    }

    private void SynchronizeEnemiesWithPointers() {
        Vector3 distanceToEnemy = Enemy.position - _player.position;
        Ray ray = new Ray(_player.position, distanceToEnemy);
        Debug.DrawRay(_player.position, distanceToEnemy);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        int maximalPlanesAllowed = 4;
        int pointerRotationIndex = 0;

        float minimalDistance = Mathf.Infinity;
        for (int i = 0; i < maximalPlanesAllowed; i++) {
            if(planes[i].Raycast(ray, out float distance)) {
                if (distance < minimalDistance) {
                    minimalDistance = distance;
                    pointerRotationIndex = i;
                }
            }
        }
        ControlPointerVisibility(minimalDistance, distanceToEnemy);

        Vector3 point = ray.GetPoint(minimalDistance);
        _screenPointer.position = Camera.main.WorldToScreenPoint(point);
        _screenPointer.rotation = _pointerRotations[pointerRotationIndex];
    }

    private void ControlPointerVisibility(float minimalDistance, Vector3 distanceToEnemy) => _visibleSpriteOfScreenPointer.SetActive(minimalDistance <= distanceToEnemy.magnitude);
}
