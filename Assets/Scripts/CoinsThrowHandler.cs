using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CoinsThrowHandler : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;

    public UnityEvent OnCoinThrown, OnAwake;

    public static CoinsThrowHandler Instance;

    private float _minimalForce = 3;

    private float _maximalForce = 5;

    private void Awake() {
        Instance = this;
        OnAwake?.Invoke();
    }

    public void ThrowCoinsAtThisPoint(int count) {
        ThrowCoins(count, transform.position);
    }

    public void ThrowCoins(int count, Vector3 position) {
        /*float force = 0;
        float rightForce = 0;
        for (int i = 0; i < count; i++) {
            force = Random.Range(_minimalForce, _maximalForce);
            rightForce = Random.Range(-_maximalForce, _maximalForce);
            Coin newCoin = Instantiate(_coinPrefab, position, Quaternion.identity);
            newCoin.CoinRigidbody.AddForce(transform.up * force, ForceMode.Impulse);
            newCoin.CoinRigidbody.AddForce(transform.right * rightForce, ForceMode.Impulse);
            OnCoinThrown?.Invoke();
        }*/
    }
}
