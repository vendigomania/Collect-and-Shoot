using UnityEngine.SceneManagement;
using UnityEngine;
using YG;

public class AutoLastSceneLoader : MonoBehaviour
{
    private string _lastSavedScenePPKey = "LastSavedScene";

    private bool _isTurned = false;

    private void Start() {
        TryToLoadGame();
    }

    private void TryToLoadGame() {
        if (!_isTurned) {
            if (YandexGame.SDKEnabled) {
                LoadLastSavedScene();
                _isTurned = true;
            }
        }
    }

    private void Update() {
        TryToLoadGame();
    }

    public void LoadLastSavedScene() {
        SceneManager.LoadScene(YandexGame.savesData.isCheckPointSaved ? YandexGame.savesData.lastRegisteredCheckPointIndex : YandexGame.savesData.lastSavedPPKey);
    }
}
