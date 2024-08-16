using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class YandexAdsAllower : MonoBehaviour
{
    private bool _canShowAds = false;

    private float _delay = 120;

    private void Awake() {
        StartCoroutine(AdShowAllowDelay());
    }

    private IEnumerator AdShowAllowDelay() {
        yield return new WaitForSeconds(_delay);
        {
            _canShowAds = true;
        }
    }

    public void ShowAd() {
        if (_canShowAds) {
            //YandexGame.Fu
        }
    }
}
