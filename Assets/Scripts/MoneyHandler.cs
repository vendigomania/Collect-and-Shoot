using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyHandler : MonoBehaviour
{
    private string _moneyPlayerPrefsKey = "MoneyAmount";

    [SerializeField] private bool _loadMoneyAmountToText;

    [SerializeField] private TMP_Text _targetText;

    public static MoneyHandler Instance;

    private void Awake() {
        Instance = this;
        if (_loadMoneyAmountToText) LoadMoneyAmountToText();
    }

    private void FixedUpdate() {
        LoadMoneyAmountToText();
    }

    private void LoadMoneyAmountToText() => _targetText.text = $"{PlayerPrefs.GetInt(_moneyPlayerPrefsKey, 0)}";

    public void ChangeMoneyAmount(int targetValueToAdd) {
        int currentValue = PlayerPrefs.GetInt(_moneyPlayerPrefsKey, 0);
        int newValue = currentValue + targetValueToAdd;
        PlayerPrefs.SetInt(_moneyPlayerPrefsKey, newValue);
        if (_loadMoneyAmountToText) LoadMoneyAmountToText();
    }

    public void SetMoneyAmount(int targetValue) {
        PlayerPrefs.SetInt(_moneyPlayerPrefsKey, targetValue);
        if (_loadMoneyAmountToText) LoadMoneyAmountToText();
    }
}
