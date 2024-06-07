using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class sinh_quái : MonoBehaviour
{
    public static int số_lượng_địch = 0;
    
    public Transform điểm_tạo_quái;

    private Wave wave;

    public Wave[] waves;

    private int đợt = 0;

    public Text Waves;

    public GameObject final_boss;

    public static bool Boss_cuối;

    public float chờ = 10f;
    private float đếm_ngược = 2f;

    public Quản_lý quản_lý;

    private void Start()
    {
        số_lượng_địch = 0;
        Boss_cuối = false;
    }
    private void Update()
    {
        if (số_lượng_địch > 0)
        {
            return;
        }
        if (Quản_lý.kết_thúc == true)
        {
            this.enabled = false;
            return;
        }

        if (đếm_ngược<=0f)
        {
            StartCoroutine(bắt_đầu());//gọi hàm đếm ngược
            
            đếm_ngược = chờ;
            return;
        }
        if (đợt == waves.Length && số_lượng_địch == 0)
        {
            StartCoroutine(boss_cuối());
            this.enabled = false;
        }

        đếm_ngược -=Time.deltaTime;
    }

    IEnumerator boss_cuối()
    {
        Waves.text = "FINAL BOSS"; 
        Waves.color = Color.red;
        yield return new WaitForSeconds(2);
        nhạc_nền.instance.nhạc_boss();
        yield return new WaitForSeconds(8);
        tạo_quái(final_boss);
        số_lượng_địch++;
        Boss_cuối = true;// Đánh dấu rằng đã thực hiện một lần
    }

    IEnumerator bắt_đầu()
    {
        chỉ_số_người_chơi.lượt++;

        wave = waves[đợt];

        số_lượng_địch = wave.số_lượng1+wave.số_lượng2;
        if (wave.kẻ_địch1 != null)
        {
            StartCoroutine(đợt1());
        }
        if (wave.kẻ_địch2 != null)
        {
            StartCoroutine(đợt2());
        }
        đợt++;
        Waves.text = "Lượt : " + đợt.ToString() + "/10";
        yield return 0;
    }
    IEnumerator đợt1()
    {
        for (int i = 0; i < wave.số_lượng1; i++)
        {
            tạo_quái(wave.kẻ_địch1);
            yield return new WaitForSeconds(wave.rate1);//thời gian chờ
        }
    }
    IEnumerator đợt2()
    {
        yield return new WaitForSeconds(wave.thời_gian_trước_đợt2);
        for (int i = 0; i < wave.số_lượng2; i++)
        {
            tạo_quái(wave.kẻ_địch2);
            yield return new WaitForSeconds(wave.rate2);
        }
    }

    //ABSTRACTION
    public void tạo_quái(GameObject kẻ_địch)
    {
        Instantiate(kẻ_địch, điểm_tạo_quái.position, điểm_tạo_quái.rotation);
    }
}
