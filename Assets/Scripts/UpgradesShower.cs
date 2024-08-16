using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradesShower : MonoBehaviour
{
    [SerializeField] private Animator _targetPanelAnimator;

    public static UpgradesShower Instance;

    [HideInInspector] public bool AreUpgradesShown = false;

    [SerializeField] private GameObject _lockingPanel;

    private FloatingJoystick _joystick;

    public UnityEvent OnUpgradesShown, OnUpgradesHidden;

    private void Awake() {
        _joystick = FindObjectOfType<FloatingJoystick>();
        Instance = this;
    }

    public void ShowUpgrades() {
        if (!AreUpgradesShown) {
            string triggerName = "Appear";
            _targetPanelAnimator.SetTrigger(triggerName);
            AreUpgradesShown = true;
            _lockingPanel.SetActive(false);
            OnUpgradesShown?.Invoke();
            _joystick.DisableJoystick();
        }
    }

    public void HideUpgrades() {
        if (AreUpgradesShown) {
            string triggerName = "Disappear";
            _targetPanelAnimator.SetTrigger(triggerName);
            AreUpgradesShown = false;
            BulletsSpawner.Instance.CanSpawnGoldenBullet = true;
            EnemyWaveSpawner.Instance.ForceStartSpawning();
            _lockingPanel.SetActive(true);
            OnUpgradesHidden?.Invoke();
        }
    }
}
