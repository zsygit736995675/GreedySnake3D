using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMe : MonoBehaviour
{
    [Header("旋转速度")]
    public float power;
    private void Update()
    {
        transform.Rotate(Vector3.back,Time.deltaTime*power);
    }
}
