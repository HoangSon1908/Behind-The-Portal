using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class điểm_rẽ : MonoBehaviour
{
    public static Transform[] Điểm;

    private void Awake()//gọi trước cả Start() :)
    {
        Điểm = new Transform[transform.childCount];
        for (int i = 0; i < Điểm.Length; i++)
        {
            Điểm[i]= transform.GetChild(i);
        }
    }
}
