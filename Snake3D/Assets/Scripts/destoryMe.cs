using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destoryMe : MonoBehaviour
{
    [Header("消失时间")]
    public float desTime;

    private void Start()
    {
        Destroy(gameObject, desTime);
    }
}
