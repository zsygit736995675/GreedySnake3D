using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spine.AnimationState;

public class SpineBase : MonoBehaviour
{

    private SkeletonGraphic sgp;

    Action callback;

    private void Awake()
    {
        sgp = GetComponent<SkeletonGraphic>();
    }


    public void PlayAnim(string animName, bool isloop, System.Action callback = null)
    {
        sgp.AnimationState.SetAnimation(0, animName, isloop);

        if (callback != null)
        {
            this.callback = callback;
            sgp.AnimationState.Complete += PalyComplete;
        }
    }


    void PalyComplete(TrackEntry trackEntry)
    {
        sgp.AnimationState.Complete -= PalyComplete;
        callback?.Invoke();
    }
}
