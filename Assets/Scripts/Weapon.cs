using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] public int ShotsForOneIteration { get; private set; } = 1;

    [field: SerializeField] public float DelayBetweenEveryShot { get; private set; } = 0.1f;

    [field: SerializeField] public Transform ShootingOrigin { get; private set; }

    [field: SerializeField] public int Damage { get; private set; } = 25;
}
