using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableToggle : MonoBehaviour
{
    [SerializeField] private GameObject on;
    [SerializeField] private GameObject off;

    public void SetOn(bool onV)
    {
        on.SetActive(onV);
        off.SetActive(!onV);
    }
}
