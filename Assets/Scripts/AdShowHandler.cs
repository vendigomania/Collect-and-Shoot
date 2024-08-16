using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class AdShowHandler : MonoBehaviour
{
    private FocusSoundHandler _focusSoundHandler;

    public static AdShowHandler Instance;

    public bool IsAdOpen { get; set; } = false;

    public bool CanShowAd = false;

    private GameObject _timerText;

    private int _secondsLeft = 3;

    public void LoadAdDelay() {
        CanShowAd = false;
        StartCoroutine(AdDelay());
    }

    private IEnumerator AdDelay() {
        yield return new WaitForSecondsRealtime(60);
        {
            CanShowAd = true;
        }
    }

    private void Awake() {
        /*_timerText = GameObject.Find("TimerText");
        _timerText.SetActive(false);

        YandexGame.OpenFullAdEvent += LoadAdDelay;
        LoadAdDelay();*/

        Instance = this;
        _focusSoundHandler = GetComponent<FocusSoundHandler>();
        YandexGame.OpenFullAdEvent += AdOpen;
        YandexGame.CloseFullAdEvent += AdClose;
    }

    private void UpdateText() {
        if (_secondsLeft > 1) {
            _secondsLeft--;
            _timerText.GetComponent<TMPro.TMP_Text>().text = TranslationLoader.IsCurrentLanguageRussian() ? $"Реклама появится через {_secondsLeft}сек." : $"Ad will be shown after {_secondsLeft}sec.";
            StartCoroutine(Timer());
        }
        else {
            _timerText.SetActive(false);
        }
    }

    private IEnumerator Timer() {
        yield return new WaitForSeconds(1);
        {
            UpdateText();
        }
    }

    public IEnumerator AdShowDelay() {
        yield return new WaitForSeconds(3);
        {
            YandexGame.FullscreenShow();
        }
    }

    public void AdOpen() {
        IsAdOpen = true;
        _focusSoundHandler.SetSound(false);
    }

    public void AdClose() {
        IsAdOpen = false;
        _focusSoundHandler.SetSound(true);
    }
}
