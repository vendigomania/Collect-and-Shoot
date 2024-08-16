using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class BulletsCollectAdvisor : MessagesShowHandler
{
    private bool _showedTheMessage = false;

    private string _message = "Collect!";

    private static int _bulletsLeftToCollect = 3;

    private string _hasTutorialBeenShownPlayerPrefsKey = "HasBulletsTutorialBeenShown";

    private bool _canBeTriggered = true;

    private float _betweenShowingDelay = 0.5f;

    private void FixedUpdate() {
        TryToHideTheMessage();
    }

    private void TryToHideTheMessage() {
        if(PlayerPrefs.GetInt(_hasTutorialBeenShownPlayerPrefsKey, 0) == 1) {
            HideMessage();
        }
    }

    public void ShowTheMessage() {
        if(PlayerPrefs.GetInt(_hasTutorialBeenShownPlayerPrefsKey, 0) == 0) {
            if (!_showedTheMessage) {
                if (_canBeTriggered) {
                    ShowMessage(_message);
                    StartCoroutine(BetweenMessageShowingDelay());
                    _canBeTriggered = false;
                }
            }
        }
    }

    public void HideTheMessage() {
        if (_canBeTriggered) {
            HideMessage();
            _canBeTriggered = false;
            StartCoroutine(BetweenMessageShowingDelay());
        }
    }

    private IEnumerator BetweenMessageShowingDelay() {
        yield return new WaitForSeconds(_betweenShowingDelay);
        {
            _canBeTriggered = true;
        }
    }

    public void DisableShowingAbility() {
        if (!_showedTheMessage) {
            HideMessage();
            _showedTheMessage = true;
            DecreaseBulletsLeftAmount();
        }
    }

    private void DecreaseBulletsLeftAmount() {
        if (_bulletsLeftToCollect > 1) {
            _bulletsLeftToCollect--;
        }
        else PlayerPrefs.SetInt(_hasTutorialBeenShownPlayerPrefsKey, 1);
    }
}
