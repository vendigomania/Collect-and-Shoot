using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneLoader : MonoBehaviour
{
    private string _lastSavedScenePPKey = "LastSavedScene";

    private int _sceneToLoad = 0;

    private void Awake() {
        Application.targetFrameRate = 60;

        /*PlayerSettings.WebGL.emscriptenArgs = "-s WASM_MEM_MAX=2048MB";
        PlayerSettings.WebGL.memorySize = 2048;*/
    }

    public void LoadScene(int targetIndex) => SceneManager.LoadScene(targetIndex);

    public void InitializeDelayedLoading(int targetIndex) => _sceneToLoad = targetIndex;

    public void LoadSceneWithDelay(float delay) {
        StartCoroutine(DelayedLoading(delay));
    }

    public void SetTargetSceneAsNextLevel(int targetIndex) {
        PlayerPrefs.SetInt("Checkpoint", targetIndex);
        PlayerPrefs.Save(); 
    }

    private IEnumerator DelayedLoading(float delay) {
        yield return new WaitForSeconds(delay);
        {
            LoadScene(_sceneToLoad);
        }
    }
}
