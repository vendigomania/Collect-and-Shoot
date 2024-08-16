using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeHandler : MonoBehaviour
{
    public UnityEvent OnUpgraded, OnLateUpgrade;

    [SerializeField] private int _valueImproveAfterUse = 1;

    public int UpgradeValue = 1;

    [SerializeField] private TMP_Text _levelText, _upgradeValueText;

    [SerializeField] private Sprite _exceptionalBackground;

    private Image _currentImage;

    private Sprite _currentBackground;

    [SerializeField] private string _customInfoAfterUpgradeText;

    [SerializeField] private UpgradeValueType _upgradeValueType = UpgradeValueType.Number;

    [SerializeField] private bool _startValueIsZero = false;

    [SerializeField] private int _firstUpgradeValue = 50;

    private bool _firstUpgradeApplied = false;

    private int _itemLevel = 1;

    [SerializeField] private bool _canBeExceptional = true;

    public bool DontOfferAfterUpgrade = false;

    private string _iterationsPlayerPrefsKey = "Iterations";

    private string _finalPPKey;

    public bool IsAExceptional { get; private set; } = false;

    [SerializeField] private bool _updateTextsAtAwake = true;

    [SerializeField] private bool _applyIterationsAtAwake = false;

    [SerializeField] private TMP_FontAsset _defaultFont;

    private bool _addedOneLevel = false;

    private bool _initialized = false;

    private void Awake()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            if (_defaultFont != null)
            {
                texts[i].font = _defaultFont;
            }
        }
        _finalPPKey = $"{_iterationsPlayerPrefsKey}{gameObject.name}";
        _currentImage = GetComponent<Image>();
        _currentBackground = _currentImage.sprite;
        if (_updateTextsAtAwake)
        {
            UpdateTexts();
        }
        MakeAllChildTextAsThin();
        OnUpgraded.AddListener(ShowAd);
    }

    private void ShowAd()
    {
    }

    private void Start()
    {

    }

    private void ApplyIterations()
    {
        int iterations = 0;
        for (int i = 0; i < iterations; i++)
        {
            UpgradeWithoutSavingIterations();
        }
    }

    private void MakeAllChildTextAsThin()
    {
        TMP_Text[] _texts = GetComponentsInChildren<TMP_Text>();
        for (int i = 0; i < _texts.Length; i++)
        {
            _texts[i].fontStyle = FontStyles.UpperCase;
        }
    }

    public void Upgrade()
    {
        if (!IsAExceptional)
        {
            _itemLevel++;
            ForceUpgrade(true);
        }
        else
        {
            _itemLevel++;
            ForceUpgrade(true);
            ForceUpgrade(true);
        }
    }

    private void Update()
    {
        if (!_initialized)
        {
            if (true)
            {
                if (_applyIterationsAtAwake)
                {
                    ApplyIterations();
                }
                else
                {
                    
                }

                gameObject.SetActive(false);

                _levelText.text = TranslationLoader.IsCurrentLanguageRussian() ? $"УР.{_itemLevel}" : $"LVL.{_itemLevel}";
                _initialized = true;
            }
        }

        _levelText.text = TranslationLoader.IsCurrentLanguageRussian() ? $"УР.{_itemLevel}" : $"LVL.{_itemLevel}";
    }

    public void UpgradeWithoutSavingIterations()
    {
        if (!IsAExceptional)
        {
            _itemLevel++;
            ForceUpgrade(false);
        }
        else
        {
            _itemLevel++;
            ForceUpgrade(false);
            ForceUpgrade(false);
        }
    }

    private void ForceUpgrade(bool saveIterations)
    {
        OnUpgraded?.Invoke();
        if (_startValueIsZero && !_firstUpgradeApplied)
        {
            UpgradeValue = _firstUpgradeValue;
            _firstUpgradeApplied = true;
        }
        else UpgradeValue += _valueImproveAfterUse;
        OnLateUpgrade?.Invoke();
        UpdateTexts();

        if (DontOfferAfterUpgrade)
        {
            UpgradesRandomizer.Instance.ClearFromRow(gameObject);
            StartCoroutine(TurnOffAfterDelay());
        }
    }

    private IEnumerator TurnOffAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        {
            gameObject.SetActive(false);
        }
    }

    public void SetExceptionalState(bool targetState)
    {
        if (_canBeExceptional)
        {
            UpdateTexts();
            IsAExceptional = targetState;
            _currentImage.sprite = (IsAExceptional ? _exceptionalBackground : _currentBackground);
            if (targetState)
            {
                _itemLevel += (_addedOneLevel ? 2 : 1);
                _addedOneLevel = true;
                _levelText.text = TranslationLoader.IsCurrentLanguageRussian() ? $"УР.{_itemLevel}" : $"LVL.{_itemLevel}";
                if (_upgradeValueType != UpgradeValueType.Custom)
                {
                    _upgradeValueText.text = (_upgradeValueType == UpgradeValueType.Number) ? $"+{UpgradeValue + (_valueImproveAfterUse * 2)}" : $"{UpgradeValue + (_valueImproveAfterUse * 2)}%";

                    if (_startValueIsZero && !_firstUpgradeApplied)
                    {
                        _upgradeValueText.text = (_upgradeValueType == UpgradeValueType.Number) ? $"+{UpgradeValue + (_valueImproveAfterUse * 2)}" : $"{_firstUpgradeValue}%";
                    }
                }
                else
                {
                    _upgradeValueText.text = $"+{UpgradeValue + (_valueImproveAfterUse * 2)}{_customInfoAfterUpgradeText}";
                }
            }
        }
    }

    public void UpdateTexts()
    {
        if (_startValueIsZero)
        {
            UpdateTextsWithStartZeroValue();
            return;
        }

        if (UpgradeValue > 0f)
        {
            _levelText.text = TranslationLoader.IsCurrentLanguageRussian() ? $"УР.{_itemLevel}" : $"LVL.{_itemLevel}";
            if (_upgradeValueType != UpgradeValueType.Custom)
            {
                _upgradeValueText.text = (_upgradeValueType == UpgradeValueType.Number) ? $"+{UpgradeValue}" : $"{UpgradeValue}%";
            }
            else
            {
                _upgradeValueText.text = $"+{UpgradeValue}{_customInfoAfterUpgradeText}";
            }
        }
    }

    public void UpdateTextsWithStartZeroValue()
    {
        if (UpgradeValue > 0f)
        {
            _levelText.text = TranslationLoader.IsCurrentLanguageRussian() ? $"УР.{_itemLevel}" : $"LVL.{_itemLevel}";
            if (_upgradeValueType != UpgradeValueType.Custom)
            {
                _upgradeValueText.text = (_upgradeValueType == UpgradeValueType.Number) ? $"+{UpgradeValue + _valueImproveAfterUse}" : $"{UpgradeValue + _valueImproveAfterUse}%";
            }
            else
            {
                _upgradeValueText.text = $"+{UpgradeValue + _valueImproveAfterUse}{_customInfoAfterUpgradeText}";
            }
        }
    }
}
public enum UpgradeValueType{ Number, Percentage, Custom }
