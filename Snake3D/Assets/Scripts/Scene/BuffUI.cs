using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{

    public BuffType type;

    Transform root, citie, zanting, jiasu;

    RectTransform rect;

    float timer = 0;

    float timeLeft = 0;

    bool isEnd = false;


    public void Init() 
    {
        transform.localScale = Vector3.one;
        root = transform.SeachTrs<Transform>("root");
        citie = transform.SeachTrs<Transform>("citie");
        zanting = transform.SeachTrs<Transform>("zanting");
        jiasu  = transform.SeachTrs<Transform>("jiasu");
        rect = root.GetComponent<RectTransform>();
    }

    public void ShowBuff(BuffType type,float timer) 
    {
        this.type = type;
        this.timer = timer;
        this.timeLeft = timer;
        isEnd = false;

        citie.gameObject.SetActive(type==BuffType.CT);
        zanting.gameObject.SetActive(type == BuffType.ZT);
        jiasu.gameObject.SetActive(type == BuffType.JS);

        if (!gameObject.activeSelf || rect.anchoredPosition.x<0)
        {
            gameObject.SetActive(true);
            rect.anchoredPosition = new Vector2(-142, 0);
            DOTween.To(() => rect.anchoredPosition, x => rect.anchoredPosition = x, Vector2.zero, 0.5f);
        }
       
        root.GetComponent<Image>().fillAmount = 1;
    }

    public void Clear() 
    {
        isEnd = true;
        DOTween.To(() => rect.anchoredPosition, x => rect.anchoredPosition = x, new Vector2(-141, 0), 0.5f).onComplete = () => {

            gameObject.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnd) 
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                root.GetComponent<Image>().fillAmount = timeLeft / timer;
            }
            else
            {
                Clear();
                Snake_Game.Ins.RepelBuff (type);
            }
        }
    }



}
