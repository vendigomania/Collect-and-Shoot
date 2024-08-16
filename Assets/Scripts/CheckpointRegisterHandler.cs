using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRegisterHandler : MonoBehaviour
{
    public bool CheckpointRegistered { get; private set; } = false;

    [SerializeField] private int _checkpointSceneIndex, _simpleSceneIndex;

    [SerializeField] private Animator _checkpointPanelAnimator;

    [SerializeField] private SceneLoader _sceneLoader;

    private float _loadingDelay = 2.85f;

    public bool ClearProgress = false;

    private void Update() {
        if (ClearProgress) {
            PlayerPrefs.DeleteKey("Checkpoint");
        }
    }

    public void RegisterCheckpoint(int checkPointIndex) {
        CheckpointRegistered = true;
        PlayerPrefs.SetInt("Checkpoint", checkPointIndex);
        PlayerPrefs.Save();
    }

    public void LoadSceneAfterDeath() {
        if (!CheckpointRegistered) {
            _sceneLoader.InitializeDelayedLoading(_simpleSceneIndex);
            _sceneLoader.LoadSceneWithDelay(_loadingDelay);
        }
        else {
            string triggerName = "Appear";
            _checkpointPanelAnimator.SetTrigger(triggerName);
        }
    }
}
