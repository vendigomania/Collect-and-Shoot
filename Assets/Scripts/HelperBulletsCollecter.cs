using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;

public class HelperBulletsCollecter : MonoBehaviour
{
    public List<BulletToPick> PickedBullets = new List<BulletToPick>();

    private string _bulletToPickTag = "Bullet";

    [SerializeField] private Transform _bulletsRoot;

    [SerializeField] private Vector3 _offsetBetweenBullets;

    private string _maxCapacityPlayerPrefsKey = "MaximalCapacity";

    public static HelperBulletsCollecter Instance;

    public UnityEvent OnBulletPicked;

    private int _defaultMaximumCapacity = 5;

    private int _bulletsCollected = 0;

    private int _currentMaximalCapacity = 0;

    public bool CollectedEnoughBulletsToStart { get; private set; } = false;

    public bool CollectedGoldBullet { get; private set; } = false;

    [SerializeField] private GameObject _maximalBulletsText;

    [SerializeField] private UpgradeHandler _capacityUpgrader;

    public bool UsedGoldenBullet;

    private void Awake() {
        Instance = this;
        _currentMaximalCapacity = _defaultMaximumCapacity;
    }

    public void ImproveCapacityValueBy() {
        _currentMaximalCapacity += _capacityUpgrader.UpgradeValue;
        TurnMaximalBulletsText(false);
    }

    public void PickABullet(GameObject bullet) {
        GameObject targetBullet = bullet.gameObject;
        BulletToPick instance = targetBullet.GetComponent<BulletToPick>();
        if (_currentMaximalCapacity > 0 || instance.IsAGoldenBullet) {
            if (instance.CanBePickedUp) {
                instance.TurnPickingUpEvent();
                PickedBullets.Add(instance);
                PlaceBulletAtBack(instance);
                OnBulletPicked?.Invoke();
                TryToReplaceGoldenBullet();
                instance.CanBePickedUp = false;
                _currentMaximalCapacity--;
                TurnMaximalBulletsText(_currentMaximalCapacity <= 0 ? true : false);

                if (instance.IsAGoldenBullet) CollectedGoldBullet = true;

                BulletsCollecter.Instance.BulletsCollected++;
                if (BulletsCollecter.Instance.BulletsCollected >= 3) {
                    BulletsCollecter.Instance.CollectedEnoughBulletsToStart = true;
                }
            }
        }
        else TurnMaximalBulletsText(true);
    }

    public void TurnMaximalBulletsText(bool state) {
        _maximalBulletsText.SetActive(state);
        if (state) _maximalBulletsText.transform.position = PickedBullets.Last().transform.position + _offsetBetweenBullets;
    }

    public void TurnMaximalBulletsPosition(Vector3 targetPosition) => _maximalBulletsText.transform.position = targetPosition;

    public void TryToReplaceGoldenBullet() {
        int requiredAmount = 2;
        if (PickedBullets.Count >= requiredAmount) {
            for (int i = 0; i < PickedBullets.Count; i++) {
                if (PickedBullets[i].IsAGoldenBullet) {
                    if (PickedBullets.Last() != PickedBullets[i]) {
                        PickedBullets.Last().ChangeGoldenBulletState(true);
                        PickedBullets[i].ChangeGoldenBulletState(false);
                    }
                    break;
                }
            }
        }
    }

    public void PlaceBulletAtBack(BulletToPick newBulletInstance) {
        int oneBulletAddedAmount = 1;
        Vector3 lastOffsetedBulletPosition = Vector3.zero;
        newBulletInstance.transform.SetParent(_bulletsRoot);
        Vector3 rawPosition = _bulletsRoot.position;
        try {
            lastOffsetedBulletPosition = PickedBullets[PickedBullets.Count - 2].transform.position + _offsetBetweenBullets;
        }
        catch { }
        newBulletInstance.transform.SetPositionAndRotation((PickedBullets.Count <= oneBulletAddedAmount) ? rawPosition : lastOffsetedBulletPosition, _bulletsRoot.rotation);
    }

    public void UseLastBullet(bool useGoldenBullet) {
        if (PickedBullets.Count > 0) {
            int lastIndex = PickedBullets.Count - 1;
            int previousIndex = PickedBullets.Count - 2;
            if (PickedBullets.Count > 1) {
                if (PickedBullets[lastIndex].IsAGoldenBullet) {
                    if (!useGoldenBullet) {
                        PickedBullets[previousIndex].ChangeGoldenBulletState(true);
                    }
                    else CollectedGoldBullet = false;
                }
            }
            DestroyBullet(lastIndex);
            _currentMaximalCapacity++;
            TurnMaximalBulletsText(false);
        }
    }

    public void DestroyBullet(int targetIndex) {
        Destroy(PickedBullets[targetIndex].gameObject);
        PickedBullets.RemoveAt(targetIndex);
    }

    public bool GoldenBulletIsTheOne() => (PickedBullets[PickedBullets.Count - 1].IsAGoldenBullet && PickedBullets.Count <= 1);

    public bool BulletCanBeCollected(BulletToPick instance) => (_currentMaximalCapacity > 0 || instance.IsAGoldenBullet);

    public bool HasGoldenBullet() {
        for (int i = 0; i < PickedBullets.Count; i++) {
            if (PickedBullets[i].IsAGoldenBullet) {
                return true;
            }
        }
        return false;
    }

    public void ResetGoldenBulletUsingState() {
        UsedGoldenBullet = false;
    }
}
