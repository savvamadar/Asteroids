using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    public AudioSource aSource;

    public void Awake()
    {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    public static void StartMusic()
    {
        if (instance)
        {
            instance.aSource.Play();
        }
    }
}
