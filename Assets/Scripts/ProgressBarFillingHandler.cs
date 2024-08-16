using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFillingHandler : MonoBehaviour
{
    [SerializeField] private Image[] _fillingBars, _circles;

    [SerializeField] private Sprite _blueCircle;

    public static ProgressBarFillingHandler Instance;

    private float _fillingValue = 0;

    public int BarIndex = -1;

    public float _perEnemyFillingValue = 0.2f;

    public UnityEvent OnCurrentCircleChanged;

    private float _speedDivideValue = 0.25f;

    private float _xPosition = 0;

    [SerializeField] private RectTransform _bar;

    private bool _didntUpdatedCircleAtFirst = true;

    private void Awake() {
        _xPosition = _bar.anchoredPosition.x;
        Instance = this;
    }

    private void Update() {
        UpdateCurrentBarFilling();
        SyncXPositionAndBar();
    }

    private void UpdateCurrentBarFilling() {
        if (GameStartHandler.Instance.IsGameStarted) {
            if(BarIndex >= 0) {
                int targetIndex = BarIndex;
                float velocity = 0;
                float currentFillAmount = _fillingBars[targetIndex].fillAmount;
                currentFillAmount = Mathf.SmoothDamp(currentFillAmount, _fillingValue, ref velocity, Time.smoothDeltaTime / _speedDivideValue);
                _fillingBars[targetIndex].fillAmount = currentFillAmount;
            }
        }
    }

    private void SyncXPositionAndBar() {
        float velocity = 0;
        Vector2 current = _bar.anchoredPosition;
        current.x = Mathf.SmoothDamp(current.x, _xPosition, ref velocity, Time.smoothDeltaTime / _speedDivideValue);
        _bar.anchoredPosition = current;
    }

    public void ChangeBarIndex(int valueToAdd) {
        BarIndex += valueToAdd;
        if (_didntUpdatedCircleAtFirst) {
            _didntUpdatedCircleAtFirst = false;
        }
        else UpdateCompletedCircle();
    }

    public void UpdateCompletedCircle() {
        _circles[BarIndex].sprite = _blueCircle;
        OnCurrentCircleChanged?.Invoke();
    }

    public void OnEnemyKilled() {
        _fillingValue += _perEnemyFillingValue;
    }

    public void ResetBarFillAmount() {
        _fillingValue = 0;
    }
    
    public void MoveBarByX(float offset) {
        _xPosition += offset;
    }
}
