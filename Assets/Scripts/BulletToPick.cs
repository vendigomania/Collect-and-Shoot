using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletToPick : MonoBehaviour
{
    [SerializeField] private bool _isAGoldenBullet = false;

    public bool IsAGoldenBullet{ get => _isAGoldenBullet; }

    [SerializeField] private GameObject[] _bulletsVariations;

    [HideInInspector] public bool CanBePickedUp = true;

    [SerializeField] private Material _goldenBulletMaterial, _simpleBulletMaterial;

    [SerializeField] private MeshRenderer _meshRenderer;

    private BoxCollider _triggerCollider;

    public UnityEvent OnBulletPickedUp;

    public CanBeUsedByTheGun CanBeUsedBy = CanBeUsedByTheGun.Pistol;

    private string _propTag = "Prop";

    private bool _checkedWrongPosition = false;

    private float _destroyAfterAutomatically = 25f;

    private bool _visibiltyState = true;

    private float _betweenEffectDelay = 0.25f;

    private float _destroyEffectTurnAfter = 20f;

    private bool _isFlyingToPlayer = false;

    private GameObject _player;

    [field: SerializeField] public bool HasBeenPickedUp { get; private set; } = false;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == _propTag) {
            if (!IsAGoldenBullet) {
                if (!_checkedWrongPosition) {
                    if (!_isFlyingToPlayer) {
                        Destroy(gameObject);
                        _checkedWrongPosition = true;
                    }
                }
            }
        }
        else if(other.tag == "Player") {
            if (!_isFlyingToPlayer) {
                if(gameObject.tag != "HelperBullet") {
                    if (BulletsCollecter.Instance.BulletCanBeCollected(this)) {
                        _isFlyingToPlayer = true;
                    }
                }
                else {
                    if (HelperBulletsCollecter.Instance.BulletCanBeCollected(this)) {
                        _isFlyingToPlayer = true;
                    }
                }
            }
        }
    }

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _triggerCollider = GetComponent<BoxCollider>();
        if (!_isAGoldenBullet) {
            StartCoroutine(DestroyWhenNotUsedForLong());
            StartCoroutine(TurnDestroyingEffect());
        }
        else {
            PointerRotateHandler.Instance.ChangeStateOverriding(true);
            PointerRotateHandler.Instance.ForceVisualArrowTurningOn(true);
            PointerRotateHandler.Instance.SetTarget(transform);
        }
    }

    private IEnumerator DestroyWhenNotUsedForLong() {
        yield return new WaitForSeconds(_destroyAfterAutomatically);
        {
            if (!HasBeenPickedUp && !_isFlyingToPlayer) Destroy(gameObject);
        }
    }

    private IEnumerator TurnDestroyingEffect() {
        yield return new WaitForSeconds(_destroyEffectTurnAfter);
        {
            if (!HasBeenPickedUp) {
                StartCoroutine(BetweenEffectDelay());
            }
        }
    }

    private IEnumerator BetweenEffectDelay() {
        yield return new WaitForSeconds(_betweenEffectDelay);
        {
            if (!HasBeenPickedUp) {
                _visibiltyState = !_visibiltyState;
                _meshRenderer.enabled = _visibiltyState;
                StartCoroutine(BetweenEffectDelay());
            }
            else StopCoroutine(BetweenEffectDelay());
        }
    }

    public void TurnPickingUpEvent() {
        if (!HasBeenPickedUp) {
            OnBulletPickedUp?.Invoke();
            HasBeenPickedUp = true;
            _meshRenderer.enabled = true;
            if (_isAGoldenBullet) {
                PointerRotateHandler.Instance.ChangeStateOverriding(false);
                PointerRotateHandler.Instance.ForceVisualArrowTurningOn(false);
                PointerRotateHandler.Instance.SetTarget(null);
            }
        }
    }

    public void ChangeGoldenBulletState(bool targetState) {
        _isAGoldenBullet = targetState;
        _meshRenderer.material = targetState ? _goldenBulletMaterial : _simpleBulletMaterial;
        for (int i = 0; i < _bulletsVariations.Length; i++) {
            _bulletsVariations[i].SetActive(false);
        }
        _bulletsVariations[(targetState) ? 1 : 0].SetActive(true);
    }

    private void FixedUpdate() {
        _triggerCollider.size = new Vector3(BulletsCollecter.Instance.XAndZTriggerScale, _triggerCollider.size.y, BulletsCollecter.Instance.XAndZTriggerScale);
    }

    private void Update() {
        if (_isFlyingToPlayer) {
            MoveToPlayer();
            TryToCollectBullet();
        }
    }

    private void MoveToPlayer() {
        float maxDistanceDelta = 10f;
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, maxDistanceDelta * Time.smoothDeltaTime);
    }

    private void TryToCollectBullet() {
        float maximalDistance = 0.5f;
        if(Vector3.Distance(transform.position, _player.transform.position) <= maximalDistance) {
            if(gameObject.tag == "HelperBullet") {
                BulletsCollecter.Instance.PickABulletForHelper(gameObject);
            }
            else BulletsCollecter.Instance.PickABullet(gameObject);
            _isFlyingToPlayer = false;
        }
    }
}
public enum CanBeUsedByTheGun{ Pistol, Rifle, Any }
