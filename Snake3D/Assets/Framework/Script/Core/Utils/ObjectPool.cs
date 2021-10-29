using System. Collections. Generic;
using UnityEngine;

public class ObjectPool : SingletonGetMono<ObjectPool>
{

    public Dictionary<string, List<GameObject>> poolsDict = new Dictionary<string, List<GameObject>>();
    //取出物体
    public T Spawn<T> (string name, Transform parent)
    {
        //如果没有这个类型的池子就创建一个
        if (!poolsDict. ContainsKey(name))
        {
            poolsDict. Add(name, new List<GameObject>());
        }

        //得到池子
        List<GameObject> ObjList;
        poolsDict. TryGetValue(name, out ObjList);

        //在池子里寻找被隐藏的游戏物体
        GameObject go = null;
        foreach (var obj in ObjList)
        {
            if (!obj. activeSelf)
            {
                go = obj;
            }
        }

        if (go == null)//不存在隐藏的游戏物体
        {
            go = Instantiate(Resources. Load<GameObject>("Prefabs/" + name));
            ObjList. Add(go);
        }
        else//存在隐藏的游戏物体
        {
            go. SetActive(true);
        }
        //go. transform. localEulerAngles(Vector3. zero);
        go. transform. SetParent(parent);
        go. name = go. name. Replace("(Clone)", "");
        return go.GetComponent <T>();
    }

    //回收物体
    public void Unspawn (GameObject go)
    {
        foreach (List<GameObject> list in poolsDict. Values)
        {
            if (list. Contains(go) && go. activeSelf)
            {
                go. SetActive(false);
            }
        }
    }

    //销毁池子
    public void ClearPool (string name)
    {
        if (poolsDict. ContainsKey(name))
        {
            foreach (GameObject go in poolsDict [ name ])
            {
                 Destroy(go);
            }
            poolsDict. Remove(name);
        }
    }

}