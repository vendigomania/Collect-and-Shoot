using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class WaveStateTextHandler : MonoBehaviour
{
    private TMP_Text _textComponent;

    private void Awake() {
        _textComponent = GetComponent<TMP_Text>();
    }

    public void UpdateTextTo(string targetText) => _textComponent.text = targetText;
}
