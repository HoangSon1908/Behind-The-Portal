using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quản_lý : MonoBehaviour
{

    public static bool kết_thúc;

    public GameObject gameOver;

    public GameObject gameWinner;

    private void Start()
    {
        kết_thúc = false;
    }

    private void Update()
    {
        if (kết_thúc)
            return;
        
        if (chỉ_số_người_chơi.mạng <= 0)
        {
            StartCoroutine(thua());
        }

        if (sinh_quái.số_lượng_địch==0 && sinh_quái.Boss_cuối)
        {
            StartCoroutine(thắng());
        }
    }

    
    public IEnumerator thua()
    {
        yield return new WaitForSeconds(1);
        nhạc_nền.instance.phát_lại();
        nhạc_nền.instance.Đã_thua();
        gameOver.SetActive(true);
        kết_thúc = true;
    }

    public IEnumerator thắng()
    {
        yield return new WaitForSeconds(1);
        if (kết_thúc == true)
        {
            yield break;
        }
        nhạc_nền.instance.phát_lại();
        nhạc_nền.instance.đã_thắng();
        gameWinner.SetActive(true);
        kết_thúc = true;
    }
}
