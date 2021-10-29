using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeiJI : MonoBehaviour
{
    public bool isTrigger=false;

    private void Update()
    {
        if (isTrigger) 
        {
            Vector3 offset = transform.parent. forward * 5 * Time.deltaTime;
            Vector3 headPos = transform.parent.position;
            transform.parent.position = new Vector3(headPos.x + offset.x, 0, headPos.z + offset.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("obstacle")|| collision.gameObject.tag.Equals("wall")) 
        {
            Snake_Game.Ins.Missile(GetComponentInParent<Prop>());
        }
    }
}
