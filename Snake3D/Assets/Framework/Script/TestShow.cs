//using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShow : MonoBehaviour
{
   
    public AudioClip clip;
    void Start()
    {
        transform.GetAudio().SetClip(clip).OnComplete(()=> { Debug.Log("完成"); }).Play();
    }


    void Update()
    {
    }

}
