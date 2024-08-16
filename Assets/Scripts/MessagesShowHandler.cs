using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MessagesShowHandler : MonoBehaviour
{
    private bool _canShowMessage = true;

    [SerializeField] private TMP_Text _targetText;

    public UnityEvent OnPlayerEntered, OnPlayerExit;

    [SerializeField] private GameObject _targetPanel;

    [SerializeField] private string _targetTag = "Player";

    public void ChangeCanShowMessageState(bool targetState) => _canShowMessage = targetState;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == _targetTag) OnPlayerEntered?.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == _targetTag) OnPlayerExit?.Invoke();
    }

    public void ShowMessage(string message) {
        if (_canShowMessage) {
            _targetText.text = message;
            _targetPanel.SetActive(true);
        }
    }

    public void HideMessage() {
        _targetPanel.SetActive(false);
    }
}
