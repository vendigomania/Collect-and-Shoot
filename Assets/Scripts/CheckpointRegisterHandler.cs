using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

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
            YandexGame.ResetSaveProgress();
        }
    }

    public void RegisterCheckpoint(int checkPointIndex) {
        CheckpointRegistered = true;
        YandexGame.savesData.isCheckPointSaved = true;
        YandexGame.savesData.lastRegisteredCheckPointIndex = checkPointIndex;
        YandexGame.SaveProgress();
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
