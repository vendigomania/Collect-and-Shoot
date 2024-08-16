using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoldKeyThrowHandler : MonoBehaviour
{
    [SerializeField] private BulletsCollecter _bulletsCollector;

    [SerializeField] private GameObject _goldKeyToThrowPrefab;

    public void ThrowGoldKeyToLock(GameObject targetLock) {
        GameObject lastBullet = _bulletsCollector.PickedBullets.Last().gameObject;
        GameObject newKey = Instantiate(_goldKeyToThrowPrefab, lastBullet.transform.position, lastBullet.transform.rotation);
        ThrownGoldKeyMovementHandler instance = newKey.GetComponent<ThrownGoldKeyMovementHandler>();
        instance.TargetLock = targetLock.transform;
        _bulletsCollector.UseLastBullet(true);
    }
}
