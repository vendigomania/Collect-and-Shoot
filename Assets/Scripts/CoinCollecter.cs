using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollecter : MonoBehaviour
{
    private string _coinTag = "Coin";

    [SerializeField] private CageDoorOpenerOnAllPrizeMoneyCollected _cageDoorOpenerOnAllPrizeMoneyCollected;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == _coinTag) {
            CollectCoin(other);
            if(_cageDoorOpenerOnAllPrizeMoneyCollected != null) _cageDoorOpenerOnAllPrizeMoneyCollected.TryToInvokeEvent();
        }
    }

    private void CollectCoin(Collider other) {
        Coin coin = other.GetComponent<Coin>();
        coin.StartCoinCollecting(transform);
    }
}
