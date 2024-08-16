using System.Linq;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class BulletsCollecter : MonoBehaviour
{
    public List<BulletToPick> PickedBullets = new List<BulletToPick>();

    private string _bulletToPickTag = "Bullet";

    private string _helperBulletToPickTag = "HelperBullet";

    [SerializeField] private Transform _bulletsRoot;

    [SerializeField] private Vector3 _offsetBetweenBullets;

    private string _maxCapacityPlayerPrefsKey = "MaximalCapacity";

    public static BulletsCollecter Instance;

    [SerializeField] private HelperBulletsCollecter _helperCollector;

    public UnityEvent OnBulletPicked;

    private int _defaultMaximumCapacity = 5;

    [HideInInspector] public int BulletsCollected = 0;

    private int _currentMaximalCapacity = 5;

    public float XAndZTriggerScale = 0.4f;

    public bool CollectedEnoughBulletsToStart { get; set; } = false;

    public bool CollectedGoldBullet { get; private set; } = false;

    public CanBeUsedByTheGun CurrentGunType = CanBeUsedByTheGun.Pistol;

    [SerializeField] private GameObject _maximalBulletsText;

    [SerializeField] private UpgradeHandler _capacityUpgrader, _magnetSizeUpgrader;

    private float _triggerScaleDefaultSize = 2f;

    private bool _appliedFirstUpgateSize = false;

    public bool UsedGoldenBullet;

    public void ChangeCurrentGunType(int targetIndex) => CurrentGunType = (CanBeUsedByTheGun)targetIndex;

    private void FixedUpdate() {
        TurnMaximalBulletsText(_currentMaximalCapacity <= 0 ? true : false);
    }

    private void Awake() {
        _currentMaximalCapacity = _defaultMaximumCapacity;
        Instance = this;
    }

    public void ImproveCapacityValueBy() {
        _currentMaximalCapacity += _capacityUpgrader.UpgradeValue;
        TurnMaximalBulletsText(false);
    }

    public void UpgradeMagnetSize() {
        float maximalValue = 5;
        if (!_appliedFirstUpgateSize) {
            XAndZTriggerScale = _triggerScaleDefaultSize;
            _appliedFirstUpgateSize = true;
        }
        else XAndZTriggerScale += (_triggerScaleDefaultSize * (float)((_magnetSizeUpgrader.UpgradeValue) / 100f));
        //XAndZTriggerScale = Mathf.Clamp(XAndZTriggerScale, 0, maximalValue);
    }

    public void PickABulletForHelper(GameObject bullet) {
        _helperCollector.PickABullet(bullet);
    }

    public void PickABullet(GameObject bullet) {
        GameObject targetBullet = bullet.gameObject;
        BulletToPick instance = targetBullet.GetComponent<BulletToPick>();
        if (_currentMaximalCapacity > 0 || instance.IsAGoldenBullet) {
            if(instance.CanBeUsedBy == CurrentGunType || instance.CanBeUsedBy == CanBeUsedByTheGun.Any) {
                if (instance.CanBePickedUp) {
                    instance.TurnPickingUpEvent();
                    PickedBullets.Add(instance);
                    PlaceBulletAtBack(instance);
                    OnBulletPicked?.Invoke();
                    TryToReplaceGoldenBullet();
                    instance.CanBePickedUp = false;
                    _currentMaximalCapacity--;

                    if (instance.IsAGoldenBullet) CollectedGoldBullet = true;

                    BulletsCollected++;
                    if (BulletsCollected >= 3) {
                        CollectedEnoughBulletsToStart = true;
                    }
                }
            }
        }
        else TurnMaximalBulletsText(true);
    }

    public void TurnMaximalBulletsText(bool state) {
        _maximalBulletsText.SetActive(state);
        if(state) _maximalBulletsText.transform.position = PickedBullets.Last().transform.position + _offsetBetweenBullets;
    }

    public void TurnMaximalBulletsPosition(Vector3 targetPosition) => _maximalBulletsText.transform.position = targetPosition;

    public void TryToReplaceGoldenBullet() {
        int requiredAmount = 2;
        if(PickedBullets.Count >= requiredAmount) {
            for (int i = 0; i < PickedBullets.Count; i++) {
                if (PickedBullets[i].IsAGoldenBullet) {
                    if(PickedBullets.Last() != PickedBullets[i]) {
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
        catch{ }
        newBulletInstance.transform.SetPositionAndRotation((PickedBullets.Count <= oneBulletAddedAmount) ? rawPosition : lastOffsetedBulletPosition, _bulletsRoot.rotation);
    }

    public void UseLastBullet(bool useGoldenBullet) {
        if(PickedBullets.Count > 0) {
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
        GameObject targetObject = PickedBullets[targetIndex].gameObject;
        PickedBullets.RemoveAt(targetIndex);
        Destroy(targetObject);
    }

    public bool GoldenBulletIsTheOne() => (PickedBullets[PickedBullets.Count - 1].IsAGoldenBullet && PickedBullets.Count <= 1);

    public bool HasGoldenBullet() {
        for (int i = 0; i < PickedBullets.Count; i++) {
            if (PickedBullets[i].IsAGoldenBullet) {
                return true;
            }
        }
        return false;
    }

    public bool BulletCanBeCollected(BulletToPick instance) => (_currentMaximalCapacity > 0 || instance.IsAGoldenBullet);

    public void ResetGoldenBulletUsingState() {
        UsedGoldenBullet = false;
    }

    public void CleanAllCollectedBullets() {
        for (int i = 0; i < PickedBullets.Count; i++) {
            Destroy(PickedBullets[i].gameObject);
        }
        PickedBullets.Clear();
    }
}
