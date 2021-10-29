
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteor : MonoBehaviour
{
    public Transform StrPath;
    public Transform P1Path;
    public Transform P1Path_2;
    public Transform EndPath;
    public Transform obj;
    // Start is called before the first frame update
    void Start()
    {
        //liu1 = transform.Find("liuxing");
        //liu2 = transform.Find("liuxing2");
        //liu3 = transform.Find("liuxing3");
        P1Path = UnityEngine.Random.Range(0, 100) > 50 ? P1Path_2 : P1Path;
        obj.GetComponent<Image>().color = obj. GetComponent<TrailRenderer>().startColor;
        EndPath = GameObject.Find("EndLIu").transform;
        StrPath = this.transform;
        StartCoroutine(MoveBeizi(obj, StrPath.position, P1Path.position, EndPath.position));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private List<Vector3> Beizi_2(Vector3 staTrs, Vector3 p1, Vector3 endTrs)
    {
        List<Vector3> path = new List<Vector3>();
        for (float i = 0; i < 1; i += 0.05f)
        {
            path.Add((1 - i) * (1 - i) * staTrs + 2 * i * (1 - i) * p1 + i * i * endTrs);

        }
        return path;
    }
    public int _inderx = 0;
    IEnumerator MoveBeizi(Transform obj, Vector3 staTrs, Vector3 p1, Vector3 endTrs)
    {
        _inderx++;
       
        if (_inderx<20)
        {
        Vector3 v = Beizi_2(staTrs, p1, endTrs)[_inderx] - obj.transform.position;
            v.z = 0;
            float angle = Vector3.SignedAngle(Vector3.up, v, Vector3.forward);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            obj.rotation = rotation;
            obj.position = Beizi_2(staTrs, p1, endTrs)[_inderx];
        }
      
       

        yield return new WaitForSeconds(0.01f);
        if (_inderx < 20)
        {
            StartCoroutine(MoveBeizi(obj, StrPath.position, P1Path.position, EndPath.position));
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }
    //角度
    private float GetAngle(Vector3 a, Vector3 b)
    {
        float angle = Vector3.Angle(a, b);
        return angle;
    }

}
