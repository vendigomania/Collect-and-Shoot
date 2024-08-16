using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesRandomizer : MonoBehaviour
{
    public List<GameObject> _leftRow, _centerRow, _rightRow;

    [SerializeField] private List<GameObject> _shownUpgrades = new List<GameObject>();

    [SerializeField] private int[] _randomizeExceptionalUpgradeAtWavesLeft;

    public static UpgradesRandomizer Instance;

    private void Awake() {
        Instance = this;
    }

    public void ClearFromRow(GameObject target) {
        if (_leftRow.Contains(target)) {
            _leftRow.Remove(target);
            return;
        }

        if (_centerRow.Contains(target)) {
            _centerRow.Remove(target);
            return;
        }

        if (_rightRow.Contains(target)) {
            _rightRow.Remove(target);
            return;
        }
    }

    public void RandomizeUpgradesFromEachRow() {
        _shownUpgrades.Clear();
        ClearAllExceptional();

        int targetIndex = Random.Range(0, _leftRow.Count);
        for (int i = 0; i < _leftRow.Count; i++) {
            if(targetIndex == i) {
                _leftRow[i].SetActive(true);
                _shownUpgrades.Add(_leftRow[i]);
                continue;
            }
            _leftRow[i].SetActive(false);
        }

        int targetCenterIndex = Random.Range(0, _centerRow.Count);
        for (int i = 0; i < _centerRow.Count; i++) {
            if (targetCenterIndex == i) {
                _centerRow[i].SetActive(true);
                _shownUpgrades.Add(_centerRow[i]);
                continue;
            }
            _centerRow[i].SetActive(false);
        }

        int targetRightIndex = Random.Range(0, _rightRow.Count);
        for (int i = 0; i < _rightRow.Count; i++) {
            if (targetRightIndex == i) {
                _rightRow[i].SetActive(true);
                _shownUpgrades.Add(_rightRow[i]);
                continue;
            }
            _rightRow[i].SetActive(false);
        }

        TryToSetRandomShownUpgradeAsExceptional();
    }

    private void TryToSetRandomShownUpgradeAsExceptional() {
        for (int i = 0; i < _randomizeExceptionalUpgradeAtWavesLeft.Length; i++) {
            if(EnemyWaveSpawner.Instance.CurrentWavesAmount == _randomizeExceptionalUpgradeAtWavesLeft[i]) {
                SetRandomShownUpgradeAsExceptional();
            }
        }
    }

    private void ClearAllExceptional() {
        UpgradeHandler[] upgrades = FindObjectsOfType<UpgradeHandler>();
        for (int i = 0; i < upgrades.Length; i++) {
            upgrades[i].SetExceptionalState(false);
        }
    }

    private void SetRandomShownUpgradeAsExceptional() {
        int targetIndex = Random.Range(0, _shownUpgrades.Count);
        UpgradeHandler upgrade = _shownUpgrades[targetIndex].GetComponent<UpgradeHandler>();
        upgrade.SetExceptionalState(true);
    }
}
