using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpriteEffectHandler : MonoBehaviour
{
    private string _moneyAmountPlayerPrefsKey = "MoneyAmount";

    private string _moneyBarTag = "MoneyBar";

    private Animator _animator;

    [field: SerializeField] public int MoneyValue { get; private set; } = 1;

    private float _maximalDistance = 25f;

    private Animator _moneyBarAnimator;

    private Transform _target;

    private float _moveSpeed = 1550f;

    private float _speedIncreaser = 5000f;

    private bool _isMovingToMoneyBar = false;

    private float _moveToMoneyBarDelay = 0.3f;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag(_moneyBarTag).transform;
        _moneyBarAnimator = _target.GetComponent<Animator>();
        _animator.SetTrigger("MoveToMoneyBar");
        StartCoroutine(MoveToMoneyBarDelay());
    }

    private IEnumerator MoveToMoneyBarDelay() {
        yield return new WaitForSeconds(_moveToMoneyBarDelay);
        {
            _isMovingToMoneyBar = true;
            _animator.enabled = false;
        }
    }

    private void Update() {
        TryToApplyMoneyCollection();
        MoveToMoneyBar();
    }
    
    private void MoveToMoneyBar() {
        if (_isMovingToMoneyBar) {
            _moveSpeed += _speedIncreaser * Time.smoothDeltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _moveSpeed * Time.smoothDeltaTime);
        }
    }

    private void TryToApplyMoneyCollection() {
        if (Vector3.Distance(transform.position, _target.position) <= _maximalDistance) {
            SaveMoneyAmount();
            _moneyBarAnimator.SetTrigger("OnMoneyGet");
            Destroy(gameObject);
        }
    }

    private void SaveMoneyAmount() {
        int currentValue = PlayerPrefs.GetInt(_moneyAmountPlayerPrefsKey, 0);
        int newValue = currentValue + MoneyValue;
        PlayerPrefs.SetInt(_moneyAmountPlayerPrefsKey, newValue);
    }
}
