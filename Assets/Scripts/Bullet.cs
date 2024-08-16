using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int TargetDamage = 0;

    [SerializeField] private float _automaticSelfDestroyAfter = 5;

    public BulletParticleMovementHandler MovementHandler;

    public ParticleSendingOrigin SentBy = ParticleSendingOrigin.Player;

    private void Awake() => AutomaticSelfDestroy();

    private void AutomaticSelfDestroy() => Destroy(gameObject, _automaticSelfDestroyAfter);

    public void SelfDestroyOnHit() {
        Destroy(gameObject);
    }
}
public enum ParticleSendingOrigin{ Player, Enemy }