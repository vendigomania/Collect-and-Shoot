using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNearEnemyWeaponChooser : MonoBehaviour
{
    [SerializeField] private int index;

    [SerializeField] private GameObject[] _availableWeapon;

    [SerializeField] private GameObject _helmet, _coat;

    [SerializeField] private bool[] _turnHelmetOnWeaponIndex, _turnCoatOnWeaponIndex;

    private void Awake() => SelectWeaponRandomly();

    private void SelectWeaponRandomly() {
        for (int i = 0; i < _availableWeapon.Length; i++) {
            _availableWeapon[i].SetActive(false);
        }
        //int index = Random.Range(0, _availableWeapon.Length);
        _availableWeapon[index].SetActive(true);
        TurnClothRootDependingFromWeapon(index);
    }

    private void TurnClothRootDependingFromWeapon(int index) {
        if (_turnCoatOnWeaponIndex[index]) _coat.SetActive(true);

        if (_turnHelmetOnWeaponIndex[index]) _helmet.SetActive(true);
    }
}