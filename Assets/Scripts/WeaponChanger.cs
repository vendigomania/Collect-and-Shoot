using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponChanger : MonoBehaviour
{
    public Weapon[] AllWeapons;

    private Animator _targetAnimator;

    public static WeaponChanger Instance;

    public int CurrentWeaponIndex { get; private set; } = 0;

    private string _gunIndexAnimatorFieldName = "GunIndex";

    private void Awake() {
        Instance = this;
        _targetAnimator = GetComponent<Animator>();
    }

    public void ChangeWeaponTo(int targetWeaponIndex) {
        CurrentWeaponIndex = targetWeaponIndex;
        UpdateWeaponsVisibility(targetWeaponIndex);
        ChangeAnimatorWeaponState(targetWeaponIndex);
    }

    private void UpdateWeaponsVisibility(int targetIndex) {
        for (int i = 0; i < AllWeapons.Length; i++) {
            if(i == targetIndex) {
                AllWeapons[i].gameObject.SetActive(true);
                continue;
            }
            AllWeapons[i].gameObject.SetActive(false);
        }
    }

    private void ChangeAnimatorWeaponState(int targetIndex) {
        _targetAnimator.SetInteger(_gunIndexAnimatorFieldName, targetIndex);
    }
}
