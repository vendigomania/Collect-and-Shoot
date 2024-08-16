using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillAdvisor : MonoBehaviour
{
    private static string _isKillingTutorialCompletedPPKey = "IsKillingTutorialCompleted";

    public static void CompleteTheTutorial() {
        if(PlayerPrefs.GetInt(_isKillingTutorialCompletedPPKey, 0) == 0) {
            PlayerPrefs.SetInt(_isKillingTutorialCompletedPPKey, 1);
        }
    }
}
