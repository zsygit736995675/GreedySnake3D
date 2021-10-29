using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SnakeBody : MonoBehaviour
{
    public  float offset;

    bool isDestroy = false;

    public Transform target;


    public void Move(Vector3 targetPos,float speed)
    {
        if (!Snake_Game.Ins.snake.isDeath)
        {
            if (Vector3.Distance(transform.position, targetPos) >= offset)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed * 3f);
            }
        }
    }



    public void Death() 
    {
        float dis = UnityEngine.Random.Range(-0.3f, 0.3f);
        Vector3 offset = new Vector3(dis, 0, dis);
        transform.DOMove(transform.position+ offset, 0.5f);
    }

    GameObject add;
    /// <summary>
    /// 放大
    /// </summary>
    public void Enlarge(int index,Action action=null) 
    {
        if (!isDestroy) 
        {
           transform.DOScale( new Vector3(2f,5, 2f), index * 0.05f).onComplete = () => {

               if (Snake_Game.Ins.isPause||Snake_Game.Ins.isvictory)
                   return;

                transform.DOScale(new Vector3(2f,10,2f), 0.1f).onComplete = () => {

                    if (Snake_Game.Ins.snake.isDeath || Snake_Game.Ins.isvictory) 
                    {
                        transform.localScale = new Vector3(2f, 5, 2f);
                        return;
                    }

                    transform.DOScale(new Vector3(2f, 5, 2f), 0.1f).onComplete = () => {
                        action?.Invoke();
                    };
                };
            };  
        }
    }

    public void PlayAdd() 
    {
        if (add == null)
        {
            GameObject go = Resources.Load<GameObject>("Game/qiu_add");
            add = Instantiate(go, transform);
            add.transform.localPosition = new Vector3(0,0.2f,0f);
           // add.gameObject.SetActive(false);
        }
        transform.localScale = Vector3.one;
        transform.DOScale(new Vector3(2, 5, 2), 0.5f).onComplete=()=> {
            add.gameObject.SetActive(true);
        };
    }

    ParticleSystem sys;
    bool isBurst = false;
    /// <summary>
    /// 爆炸
    /// </summary>
    public void Burst(int index)
    {
        isBurst = true;
        Invoke(nameof(Burst),index*0.3f);
    }

    void Burst() 
    {
        gameObject.SetActive(false);
        AudioController.Ins.PlayEffect("bomb_explode");
        GameObject go = Resources.Load<GameObject>("Game/qiu_hei");
        GameObject qiu = Instantiate(go);
        sys = qiu.GetComponent<ParticleSystem>();
        sys.gameObject.SetActive(true);
        sys.transform.position = new Vector3(transform.position.x,0.2f,transform.position.z);
        sys.transform.localScale = Vector3.one ;
        isDestroy = true;

        Invoke(nameof(Des),0.5f);
    }

    private void Des()
    {
        if (sys != null && !sys.isPlaying)
        {
            Destroy(sys.gameObject);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (target != null && !Snake_Game.Ins.snake.isDeath && isBurst)
        {
            if (Vector3.Distance(transform.position, target.position) >= offset)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * Snake_Game.Ins.snake.speed * 3f);
            }
        }
    }


}
