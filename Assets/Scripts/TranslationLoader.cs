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
        if (_textComponent == null) {
            _textComponent = GetComponent<TMP_Text>();
        }
    }

    private string _ruCode = "ru";

    private void Start()
    {
        for (int i = 0; i < _localeList.Count; i++) {
            if( _localeList[i].rows[0].value == _englishTranslation) 
            { 
                for(int j = 0; j < _localeList[i].rows.Length; j++)
                {
                    if(_localeList[i].rows[j].key == Application.systemLanguage)
                    {
                        _textComponent.text = _localeList[i].rows[j].value;
                    }
                }
            }
        }
    }

    private void LoadTranslation() {
        _textComponent.text = (IsCurrentLanguageRussian() ? _russianTranslation : _englishTranslation);
        //_translationIsLoaded = true;
    }

    public static bool IsCurrentLanguageRussian() => (Application.systemLanguage == SystemLanguage.Russian);

    struct Locale
    {
        public (SystemLanguage key, string value)[] rows;
    }

    private List<Locale> _localeList = new List<Locale>()
    {
        new Locale //tap to start
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Tap to play!"),
                (SystemLanguage.Russian, "Начать!"),
                (SystemLanguage.German, "Starten!"),
                (SystemLanguage.Greek, "Ξεκινήσετε!"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Capacity"),
                (SystemLanguage.Russian, "Вместимость"),
                (SystemLanguage.German, "Kapazität"),
                (SystemLanguage.Greek, "Ικανότητα"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Speed"),
                (SystemLanguage.Russian, "Скорость"),
                (SystemLanguage.German, "Geschwindigkeit"),
                (SystemLanguage.Greek, "Ταχύτητα"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Health"),
                (SystemLanguage.Russian, "Здоровье"),
                (SystemLanguage.German, "Gesundheit"),
                (SystemLanguage.Greek, "Υγεία"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Shooting"),
                (SystemLanguage.Russian, "Выстрелы"),
                (SystemLanguage.German, "Shooting"),
                (SystemLanguage.Greek, "Λήψη"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Magnet"),
                (SystemLanguage.Russian, "Магнит"),
                (SystemLanguage.German, "Magnet"),
                (SystemLanguage.Greek, "Μαγνήτης"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Regeneration"),
                (SystemLanguage.Russian, "Регенерация"),
                (SystemLanguage.German, "Regeneration"),
                (SystemLanguage.Greek, "Αναγέννηση"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Critical hit"),
                (SystemLanguage.Russian, "Крит. удар"),
                (SystemLanguage.German, "Kritischer Treffer"),
                (SystemLanguage.Greek, "Κρίσιμο χτύπημα"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Freezing"),
                (SystemLanguage.Russian, "Заморозка"),
                (SystemLanguage.German, "Einfrieren"),
                (SystemLanguage.Greek, "Κατάψυξη"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Burning"),
                (SystemLanguage.Russian, "Огонь"),
                (SystemLanguage.German, "Das Feuer"),
                (SystemLanguage.Greek, "Φωτιά"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Choose an upgrade!"),
                (SystemLanguage.Russian, "Выбери улучшение!"),
                (SystemLanguage.German, "Wählen Sie ein Upgrade!"),
                (SystemLanguage.Greek, "Επιλέξτε μια αναβάθμιση!"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Options"),
                (SystemLanguage.Russian, "Опции"),
                (SystemLanguage.German, "Einstellung"),
                (SystemLanguage.Greek, "Ρυθμίσεων"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Music"),
                (SystemLanguage.Russian, "Музыка"),
                (SystemLanguage.German, "Musik"),
                (SystemLanguage.Greek, "Μουσική"),
            }
        },
        new Locale
        {
            rows = new (SystemLanguage, string)[]
            {
                (SystemLanguage.English, "Sound"),
                (SystemLanguage.Russian, "Звук"),
                (SystemLanguage.German, "Klang"),
                (SystemLanguage.Greek, "Ήχος"),
            }
        },
    };
}
