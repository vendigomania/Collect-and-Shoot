using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPicker : MonoBehaviour
{
    private string _weaponToPickTag = "WeaponToPick";

    [SerializeField] private WeaponChanger _weaponChanger;

    public UnityEvent OnPickedWeapon;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == _weaponToPickTag) {
            WeaponToPick weaponToPick = other.GetComponent<WeaponToPick>();
            _weaponChanger.ChangeWeaponTo(weaponToPick.TargetWeaponIndex);
            OnPickedWeapon?.Invoke();
            weaponToPick.OnWeaponPicked?.Invoke();
        }
    }
}
