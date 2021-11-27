using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Audio[] audios;

    private static AudioManager _instance;


    private void Awake()
    {
        if(_instance!=null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Audio x in audios)
        {
            x.source = gameObject.AddComponent<AudioSource>();
            x.source.clip = x.clip;
            x.source.volume = x.volume;
            x.source.mute = x.mute;
            x.source.loop = x.loop;
            x.source.playOnAwake = false;
        }
    }

    public void PlayAudio(string name)
    {
        Audio x = Array.Find(audios, audio => audio.name == name);
        if (x == null)
        {
            Debug.LogWarning("Audio not found");
        }
        else
        {
            x.source.Play();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayAudio("Fondo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
