using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _pointerPrefab;

    [HideInInspector] public GameObject SpawnedPointer;

    private Transform _canvas;

    private string _canvasTag = "Canvas";

    private void Awake() {
        _canvas = GameObject.FindGameObjectWithTag(_canvasTag).transform;
        SpawnPointer();
    }

    public void SpawnPointer() {
        GameObject newPointer = Instantiate(_pointerPrefab, _canvas);
        PointerHandler instance = newPointer.GetComponent<PointerHandler>();
        instance.Enemy = transform;
        SpawnedPointer = newPointer;
    }

    public void DeletePointer() => Destroy(SpawnedPointer);
}
