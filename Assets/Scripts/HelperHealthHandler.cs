using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HelperHealthHandler : HealthHandler
{
    public UnityEvent OnDeath;

    [SerializeField] private ParticleSystem _onDestroyEffectPrefab;

    public bool IsDead { get; private set; } = false;

    public static HelperHealthHandler Instance;

    private string _healthPlayerPrefsKey = "Health";

    [SerializeField] private ParticleSystem _bloodEffect;

    private int _defaultHealthAmount = 100;

    private int _hpsRegenerationPerSecond = 0;

    [SerializeField] private UpgradeHandler _healthUpgrader, _regenerationUpgrader;

    private int _lastUpdatedHealth = 100;

    private void Awake() {
        Instance = this;
        Health = _defaultHealthAmount;
        _lastUpdatedHealth = Health;
        //StartCoroutine(RegenerationRoutine());
    }

    private IEnumerator RegenerationRoutine() {
        yield return new WaitForSeconds(1);
        {
            if (!IsDead) {
                if ((Health + _hpsRegenerationPerSecond) <= _lastUpdatedHealth) {
                    Health += _hpsRegenerationPerSecond;
                }
                StartCoroutine(RegenerationRoutine());
            }
        }
    }

    public void UpgradeRegeneration() {
        
    }

    public void ChangeHealthAmountBy() {
        Health += (Health * (_healthUpgrader.UpgradeValue / 100));
        _lastUpdatedHealth = Health;
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
        _bloodEffect.transform.LookAt(point);
        _bloodEffect.Play(true);
    }
}
