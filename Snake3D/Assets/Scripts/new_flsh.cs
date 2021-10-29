using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_flsh : MonoBehaviour
{
   public void getQiCaiFlsh()
    {
        EventMgrHelper.Ins.PushEventEx(EventDef.CallQiCaiFlsh,object0:gameObject);
    }
}
