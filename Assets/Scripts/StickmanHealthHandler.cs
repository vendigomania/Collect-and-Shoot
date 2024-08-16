using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickmanHealthHandler : HealthHandler
{
    public UnityEvent OnDeath;

    [SerializeField] private ParticleSystem _onDestroyEffectPrefab;

    public bool IsDead { get; private set; } = false;

    public static StickmanHealthHandler Instance;

    private string _healthPlayerPrefsKey = "Health";

    [SerializeField] private ParticleSystem _bloodEffect;

    [SerializeField] private UpgradeHandler _healthUpgrader, _regenerationUpgrader;

    private int _defaultHealthAmount = 100;

    private int _lastUpdatedHealth = 100;

    private int _hpsRegenerationPerSecond = 0;

    private void Awake() {
        Health = _defaultHealthAmount;
        _lastUpdatedHealth = Health;
        Instance = this;
        //StartCoroutine(RegenerationRoutine());
    }

    private IEnumerator RegenerationRoutine() {
        yield return new WaitForSeconds(1);
        {
            if (!IsDead) {
                if((Health + _hpsRegenerationPerSecond) <= _lastUpdatedHealth) {
                    Health += _hpsRegenerationPerSecond;
                }
                StartCoroutine(RegenerationRoutine());
            }
        }
    }

    public void ChangeHealthAmountBy() {
        Health += (int)(Health * (float)(_healthUpgrader.UpgradeValue / 100f));
        _lastUpdatedHealth = Health;
    }
    
    public void UpgradeRegeneration() {
        
    }

    public void UseRegeneration() {
        int addValue = (int)(_defaultHealthAmount * (float)(_regenerationUpgrader.UpgradeValue / 100f));
        if ((Health + addValue) <= _lastUpdatedHealth) {
            Health += addValue;
        }
    }

    public void ChangeHealthValue(int targetValueToAdd) {
        if (!IsDead) {
            if (Health + targetValueToAdd > 0) {
                Health += targetValueToAdd;
            }
            else {
                Death();
            }
        }
    }

    public void Death() {
        Health = 0;
        IsDead = true;
        OnDeath?.Invoke();
        //SupersonicWisdom.Api.NotifyLevelFailed(EnemyWaveSpawner.Instance.TargetLevelToNotify, null);
        Debug.Log($"Fail Level {EnemyWaveSpawner.Instance.TargetLevelToNotify}");
    }

    public void SelfDestroyAfter(float seconds) => Destroy(gameObject, seconds);

    private void OnDestroy() {
        TurnOnDestroyEffect();
    }

    private void TurnOnDestroyEffect() {
        ParticleSystem instance = Instantiate(_onDestroyEffectPrefab, transform.position, Quaternion.identity);
        float destroyVFXObjectAfter = 5;
        Destroy(instance.gameObject, destroyVFXObjectAfter);
        instance.Play(true);
    }

    public void CreateBloodEffect(Transform point) {
        /*_bloodEffect.transform.LookAt(point);
        _bloodEffect.Play(true);*/
    }
}
