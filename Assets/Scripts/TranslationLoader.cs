using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslationLoader : MonoBehaviour
{
    private bool _translationIsLoaded = false;

    public static TranslationLoader Instance;

    [SerializeField] private string _russianTranslation, _englishTranslation;

    private string _enCode = "en";

    [SerializeField] private TMP_Text _textComponent;

    private void Awake() {
        Instance = this;
        if(_textComponent == null) {
            _textComponent = GetComponent<TMP_Text>();
        }
    }

    private string _ruCode = "ru";

    private void Update() {
        if (true) {
            if (!_translationIsLoaded) {
                LoadTranslation();
            }
        }
    }

    private void LoadTranslation() {
        _textComponent.text = (IsCurrentLanguageRussian() ? _russianTranslation : _englishTranslation);
        //_translationIsLoaded = true;
    }

    public static bool IsCurrentLanguageRussian() => (Application.systemLanguage == SystemLanguage.Russian);
}
