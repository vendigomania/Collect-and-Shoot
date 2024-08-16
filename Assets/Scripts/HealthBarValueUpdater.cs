using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HealthBarValueUpdater : MonoBehaviour
{
    [SerializeField] private HealthHandler _source;

    [SerializeField] private Image _bar;

    [SerializeField] private TMP_Text _healthText;

    private float _maximalValue, _fillValue;

    private string _playerTag = "Player";

    private void Awake() {
        UpdateMaximalValue();
    }

    public void UpdateMaximalValue() {
        _maximalValue = _source.Health;
    }

    private void FixedUpdate() {
        UpdateFillValue();
        UpdateBarFilling();
        //UpdateHealthText();
    }

    private void UpdateBarFilling() => _bar.fillAmount = _fillValue;

    private void UpdateHealthText() => _healthText.text = $"{_source.Health}";

    private void UpdateFillValue() => _fillValue = (_source.Health / _maximalValue);
}
