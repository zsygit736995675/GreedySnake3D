using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 飞翔红包
/// </summary>
public class flyRed : MonoBehaviour
{

    Button redBtn;

    private void Start()
    {
        redBtn = GetComponent<Button>();
        redBtn.onClick.AddListener(OnMouseClick);
    }

    //点击飞翔红包
    private void OnMouseClick()
    {
#if !UNITY_EDITOR
        PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold,RedType.OpenCommon,false);
        Destroy(transform.parent.gameObject);
#else
        Debug.Log("飞翔红包点击");
        PanelMgr.GetInstance.ShowPanel(PanelName.Panel_Gold,RedType.OpenCommon);
        Destroy(transform.parent.gameObject);
#endif
    }
}
