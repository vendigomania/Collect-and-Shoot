using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthHandler : HealthHandler
{
    public UnityEvent OnDeath;

    [SerializeField] private ParticleSystem _onDestroyEffectPrefab;

    private int _coinsToThrowOnDeath = 2;

    private int _coinsToThrowForce = 3;

    private StickmanHealthHandler _stickmanHealth;

    private HelperHealthHandler _helperHealth;

    [SerializeField] private bool _turnProgressBarOnDeath = true;

    public bool IsDead { get; private set; } = false;

    private void Awake() {
        _stickmanHealth = FindObjectOfType<StickmanHealthHandler>();
        _helperHealth = FindObjectOfType<HelperHealthHandler>();
    }

    public void ChangeHealthValue(int targetValueToAdd) {
        if (!IsDead) {
            if (Health + targetValueToAdd > 0) {
                Health += targetValueToAdd;
            }
            else {
                if(_turnProgressBarOnDeath) ProgressBarFillingHandler.Instance.OnEnemyKilled();
                Health = 0;
                IsDead = true;
                OnDeath?.Invoke();
                //ThrowCoinsOnDeath(_coinsToThrowOnDeath, _coinsToThrowForce);
                TryToTurnWaveStateText();
                PointerRotateHandler.Instance.FinishEnemyKillTutorial();
                _stickmanHealth.UseRegeneration();
                _helperHealth.UseRegeneration();
            }
        }
    }

    public void ThrowCoinsOnDeath(int count, int force) => CoinsThrowHandler.Instance.ThrowCoins(count, transform.position);

    public void SelfDestroyAfter(float seconds) {
        //StartCoroutine(TurnDestroyEffectDelay(seconds));
        Destroy(gameObject, seconds);
    }

    private IEnumerator TurnDestroyEffectDelay(float seconds) {
        yield return new WaitForSeconds(seconds);
        {
            TurnOnDestroyEffect();
        }
    }

    private void TurnOnDestroyEffect() {
        ParticleSystem instance = Instantiate(_onDestroyEffectPrefab, transform.position, Quaternion.identity);
        float destroyVFXObjectAfter = 5;
        Destroy(instance.gameObject, destroyVFXObjectAfter);
        instance.Play(true);
    }

    public void TryToTurnWaveStateText() {
        if (!EnemiesExistanceHandler.IsAliveEnemyOnScene()) {
            EnemyWaveSpawner.Instance.OnWaveCompleted?.Invoke();
            GameObject waveStateText = GameObject.Find("WaveStateText");
            WaveStateTextHandler instance = waveStateText.GetComponent<WaveStateTextHandler>();
            instance.UpdateTextTo(TranslationLoader.IsCurrentLanguageRussian() ? "Конец волны" : "Wave end");
            instance.GetComponent<Animator>().SetTrigger("Appear");
            StartCoroutine(AdShowDelay());


        }
    }

    private IEnumerator AdShowDelay() {
        yield return new WaitForSeconds(2);
        {
            /*YG.YandexGame.FullscreenShow();
            Debug.Log("Ad shown");*/
        }
    }
}
