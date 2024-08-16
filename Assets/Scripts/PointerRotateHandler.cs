using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerRotateHandler : MonoBehaviour
{
    public Transform Target;

    [SerializeField] private GameObject _visualArrow;

    private bool _overrideState = false;

    public static PointerRotateHandler Instance;

    [SerializeField] private bool _canBeUsedWithBulletsTutorial = true;

    [SerializeField] private bool _canBeUsedWithEnemyKillTutorial = true;

    private bool _tutorialHasBeenEnded = false;

    private string _hasTutorialBeenShownPlayerPrefsKey = "HasBulletsTutorialBeenShown";

    private string _isKillingTutorialCompletedPPKey = "IsKillingTutorialCompleted";

    private void Update() {
        if (_canBeUsedWithBulletsTutorial) {
            TryToTurnToTheNearestBullet();
        }

        if (_canBeUsedWithEnemyKillTutorial) {
            TryToTurnToTheNearestEnemy();
        }
    }

    public void ChangeStateOverriding(bool targetState) {
        _overrideState = targetState;
    }

    private void TryToTurnToTheNearestEnemy() {
        if(PlayerPrefs.GetInt(_isKillingTutorialCompletedPPKey, 0) == 0) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0) {
                ChangeStateOverriding(true);
                ForceVisualArrowTurningOn(true);
                SetTarget(GetClosestEnemy(enemies));
            }
        }
    }

    public void FinishEnemyKillTutorial() {
        PlayerPrefs.SetInt(_isKillingTutorialCompletedPPKey, 1);
        ChangeStateOverriding(false);
        ForceVisualArrowTurningOn(false);
        SetTarget(null);
    }

    private void TryToTurnToTheNearestBullet() {
        if(PlayerPrefs.GetInt(_hasTutorialBeenShownPlayerPrefsKey, 0) == 0) {
            BulletToPick[] bullets = FindObjectsOfType<BulletToPick>();
            if(bullets.Length > 0) {
                ChangeStateOverriding(true);
                ForceVisualArrowTurningOn(true);
                SetTarget(GetClosestBullet(bullets));
            }
        }
        else if (!_tutorialHasBeenEnded) {
            ChangeStateOverriding(false);
            ForceVisualArrowTurningOn(false);
            SetTarget(null);
            _tutorialHasBeenEnded = true;
        }
    }

    public Transform GetClosestBullet(BulletToPick[] bullets) {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (BulletToPick bullet in bullets) {
            if (!bullet.HasBeenPickedUp) {
                float dist = Vector3.Distance(bullet.transform.position, currentPos);
                if (dist < minDist) {
                    tMin = bullet.transform;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }

    public Transform GetClosestEnemy(GameObject[] enemies) {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject enemy in enemies) {
            float dist = Vector3.Distance(enemy.transform.position, currentPos);
            if (dist < minDist) {
                tMin = enemy.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    public void ForceVisualArrowTurningOn(bool targetState) {
        _visualArrow.SetActive(targetState);
    }

    public void SetTarget(Transform target) {
        Target = target;
        if(target == null) {
            Target = _defaultObject;
        }
    }

    private Transform _defaultObject = null;

    private void Awake() {
        _defaultObject = GameObject.Find("Cage").transform;
        Instance = this;
    }

    private void LateUpdate() {
        if(Target != null) {
            RotateToTarget();
            if (!_overrideState) {
                ControlVisibleArrowEnableState();
            }
        }
    }

    private void RotateToTarget() {
        Quaternion targetRotation = Quaternion.LookRotation(Target.position - transform.position);
        transform.rotation = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
    }

    private void ControlVisibleArrowEnableState() => _visualArrow.SetActive((!BulletsCollecter.Instance.UsedGoldenBullet && BulletsSpawner.Instance.IsGoldenBulletGiven && BulletsCollecter.Instance.CollectedGoldBullet));
}
