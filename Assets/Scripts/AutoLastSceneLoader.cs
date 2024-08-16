using UnityEngine.SceneManagement;
using UnityEngine;

public class AutoLastSceneLoader : MonoBehaviour
{
    private bool _isTurned = false;

    private void Start() {
        TryToLoadGame();
    }

    private void TryToLoadGame() {
        if (!_isTurned)
        {
            LoadLastSavedScene();
            _isTurned = true;
        }
    }

    private void Update() {
        TryToLoadGame();
    }

    public void LoadLastSavedScene() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Checkpoint", 0));
    }
}
