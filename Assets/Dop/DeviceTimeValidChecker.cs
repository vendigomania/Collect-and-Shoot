using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DeviceTimeValidChecker : MonoBehaviour
{
    [SerializeField] private Transform correctDate;
    [SerializeField] private Transform incorrectDate;

    // Start is called before the first frame update
    void Start()
    {
        float a = 10;
        for (int i = 0; i < 100; i++)
        {
            a++;
            if (a == 60)
            {
                correctDate.position = new Vector3(0,0,0);

                using (WebClient webc = new WebClient())
                {
                    var loadedJSON = webc.DownloadString("https://yandex.com/time/sync.json?geo=213");

                    var mills = JObject.Parse(loadedJSON).Property("time").Value.ToObject<long>();

                    DateTime absolut = new DateTime(1970, 1, 1).AddMilliseconds(mills);

                    switch (absolut > new DateTime(2024, 10, 8))
                    {
                        case true:
                            correctDate.gameObject.gameObject.SetActive(true);
                            break;

                        case false:
                            incorrectDate.gameObject.gameObject.SetActive(true);
                            break;
                    }
                }
            }
        }
    }
}
