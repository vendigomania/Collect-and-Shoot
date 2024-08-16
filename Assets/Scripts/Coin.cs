using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    [HideInInspector] public Rigidbody CoinRigidbody;

    [SerializeField] private GameObject _coinSpriteEffect;

    private string _canvasTag = "Canvas";

    [SerializeField] private AudioClip _moneyCollectClip;

    public UnityEvent OnCoinCollected;

    private Transform _canvas;

    private Transform _player;

    private bool _isFollowingPlayer = false;

    private float _followingSpeed = 5f;

    public bool IsAPrize = false;

    private float _maximalDistance = 0.5f;

    private void Awake() {
        _canvas = GameObject.FindGameObjectWithTag(_canvasTag).transform;
        CoinRigidbody = GetComponent<Rigidbody>();
    }

    public void StartCoinCollecting(Transform player) {
        _player = player;
        _isFollowingPlayer = true;
    }

    private void Update() {
        FollowPlayer();
        TryToBeCollected();
    }

    private void FollowPlayer() {
        if (_isFollowingPlayer) {
            transform.position = Vector3.MoveTowards(transform.position, _player.position, _followingSpeed * Time.deltaTime);
        }
    }

    private void TryToBeCollected() {
        if (_isFollowingPlayer) {
            if (Vector3.Distance(transform.position, _player.position) <= _maximalDistance) CollectCoin();
        }
    }

    public void CollectCoin() {
        GameObject coinSprite = Instantiate(_coinSpriteEffect, _canvas);
        OnCoinCollected?.Invoke();
        PlayClip();
        Destroy(gameObject);
    }

    private void PlayClip() {
        _player.GetComponent<AudioSource>().PlayOneShot(_moneyCollectClip);
    }
}
