using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        if (GetComponent<AudioSource>().volume != UIButtons.musicVolume)
        {
            GetComponent<AudioSource>().volume = UIButtons.musicVolume;
        }
    }
}
