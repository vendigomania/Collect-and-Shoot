using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CageDoorOpenerOnAllPrizeMoneyCollected : MonoBehaviour
{
    public UnityEvent OnAllMoneyCollected;

    [SerializeField] private int _targetCount = 20;

    private int _currentCount = 0;

    public void TryToInvokeEvent() {
        _currentCount++;
        if(_currentCount >= (_targetCount - 1)) {
            OnAllMoneyCollected?.Invoke();
            Debug.Log("invoke");
        }
        Debug.Log(_currentCount);
    }
}
