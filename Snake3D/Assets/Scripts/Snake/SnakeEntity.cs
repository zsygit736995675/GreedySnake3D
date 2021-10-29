using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEntity : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("prop") || collision.gameObject.tag.Equals("obstacle") || collision.gameObject.tag.Equals("ball"))
        {
            Prop prop = collision.gameObject.GetComponent<Prop>();
            if (prop != null)
            {
                prop.Trigger();
            }
            else
            {
                prop = collision.gameObject.GetComponentInParent<Prop>();
                if (prop != null)
                {
                    prop.Trigger();
                }
            }
        }
        //else if (collision.gameObject.tag.Equals("body"))
        //{
        //    SnakeBody body = collision.gameObject.GetComponent<SnakeBody>();
        //    transform.GetComponentInParent<Snake>().HitBody(body);
        //}
        else if (collision.gameObject.tag.Equals("wall"))
        {
            if (!Snake_Game.Ins.isvictory && Snake_Game.Ins.isStart)
                Snake_Game.Ins.Failure();
        }
        else if (collision.gameObject.tag.Equals("qi"))
        {
            Snake_Game.Ins.PassLevel();
        }
    }

}
