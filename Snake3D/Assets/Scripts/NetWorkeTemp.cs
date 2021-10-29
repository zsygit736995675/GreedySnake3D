using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkeTemp : SingletonObject<NetWorkeTemp>
{

    bool isOpen = false;

    public void Start()
    {
        InvokeRepeating("networkIsOk",0,0.5f);
    }

    public void networkIsOk()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!isOpen)
            {
                isOpen = true;
                PanelMgr.GetInstance.ShowPanel(PanelName.Panel_NetWork);
            }
        }
        else if (Application.internetReachability
            == NetworkReachability.ReachableViaCarrierDataNetwork ||
            Application.internetReachability
            == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (isOpen)
            {
                isOpen = false;
                PanelMgr.GetInstance.HidePanel(PanelName.Panel_NetWork);
            }
        }
    }

}
