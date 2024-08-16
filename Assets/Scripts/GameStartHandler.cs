using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using YG;

public class GameStartHandler : MonoBehaviour
{
    public static GameStartHandler Instance;

    public bool IsGameStarted { get; private set; } = false;

    public static bool AskedForAReview = false;

    private float _reviewDelay = 300;

    public long CurrentLevel;

    public UnityEvent OnGameStarted;

    private void Awake() {
        Instance = this;
        //StartCoroutine(WaitForReviewDelay());
    }

    private IEnumerator WaitForReviewDelay() {
        yield return new WaitForSeconds(_reviewDelay);
        {
            //AskForReview();
        }
    }

    public void AskForReview() {
        if (!AskedForAReview) {
            YandexGame.ReviewShow(true);
            AskedForAReview = true;
        }
    }

    public void StartGame() {
        if (!IsGameStarted) {
            IsGameStarted = true;
            OnGameStarted?.Invoke();
            /*SupersonicWisdom.Api.NotifyLevelStarted(CurrentLevel, null);
            Debug.Log($"Start Level {GameStartHandler.Instance.CurrentLevel}");*/
        }
    }

    public void NotifyLevelCompleted() {
        /*SupersonicWisdom.Api.NotifyLevelCompleted(EnemyWaveSpawner.Instance.TargetLevelToNotify, null);
        Debug.Log($"Complete Level {EnemyWaveSpawner.Instance.TargetLevelToNotify}");
        EnemyWaveSpawner.Instance.TargetLevelToNotify++;*/
    }
}
