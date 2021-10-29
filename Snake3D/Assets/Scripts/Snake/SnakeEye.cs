using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEye : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        if (collision.gameObject.tag.Equals("obstacle") || collision.gameObject.tag.Equals("wall"))
        {
           

        }
    }


}
