using System;
using UnityEngine;

public class AudioMgr : Singleton<AudioMgr>
{
    public Action<bool> SetSound;
    //public static event Action<Transform, bool> SetSound;
    public static event Action<bool> SetMusic;

    public bool isMute;

    public void GetAudioByName (Transform go, string path, bool isloop = false)
    {
        AudioSource audio = go. GetOrAddComponent<AudioSource>();
        if (isMute)
        {
            audio. Stop();
        }
        else
        {
            path = "Audio/" + path;
            AudioClip clip = Resources. Load<AudioClip>(path);
            audio. clip = clip;
            audio. Play();
            audio. loop = isloop;
        }
    }

    public void SetAudioSound (bool isPlay)
    {
        SetSound(isPlay);
        //SetSound(go, isPlay);
    }

    public void SetAudioMusic (bool isPlay)
    {
        SetMusic(isPlay);
    }

    public void SetStop (Transform go)
    {
        AudioSource audio = go. GetComponent<AudioSource>();
        if (audio != null)
        {
            audio. Stop();
        }
    }

    public void SetStop (GameObject go)
    {
        SetStop(go. transform);
    }

    public void GetAudioByName (GameObject go, string path)
    {
        GetAudioByName(go. transform, path);
    }
}
