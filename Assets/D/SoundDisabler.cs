using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDisabler : MonoBehaviour
{
    static bool sound = true;
    static bool music = true;

    private void Start()
    {
        SetMusicOn(music);
        SetSoundsOn(sound);
    }

    public void SetSoundsOn(bool on)
    {
        sound = on;
        var list = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (var obj in list)
        {
            if(obj.name != "BackgroundMusic")
            {
                obj.mute = !on;
            }
        }
    }

    public void SetMusicOn(bool on)
    {
        music = on;

        var list = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (var obj in list)
        {
            if (obj.name == "BackgroundMusic")
                obj.mute = !on;
        }
    }
}
