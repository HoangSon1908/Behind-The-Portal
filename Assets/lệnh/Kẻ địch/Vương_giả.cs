using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vương_giả : Kẻ_địch
{
    [Header("Vương giả")]
    public GameObject tinh_anh;
    public GameObject đấu_khí;

    protected override void Start()
    {
        rẽ = điểm_rẽ.Điểm[0];
        InvokeRepeating(nameof(nhuệ_khí), 0f, 10f);
        máu = máu_khởi_đầu;
    }

    protected override void hẹo()
    {
        isDead = true;
        sinh_quái.số_lượng_địch--;
        chỉ_số_người_chơi.tinh_thể += tiền;
        Destroy(gameObject);
    }

    // POLYMORPHISM
    protected override void tiếp_tục()
    {
        if (điểm >= điểm_rẽ.Điểm.Length - 1)
        {
            chỉ_số_người_chơi.tinh_thể += tiền;
            chỉ_số_người_chơi.mạng -= 10;
            sinh_quái.số_lượng_địch--;
            Destroy(gameObject);
            return;
        }
        điểm++;
        rẽ = điểm_rẽ.Điểm[điểm];
    }

    public void nhuệ_khí()
    {
        sinh_quái.số_lượng_địch++;
        StartCoroutine(thức_tỉnh());
        Instantiate(tinh_anh, transform.position, Quaternion.identity);
        if (máu <= máu_khởi_đầu - 250)
        {
            máu += 250;
        }
        thanh_máu.fillAmount = máu / máu_khởi_đầu;
    }

    IEnumerator thức_tỉnh()
    {
        đấu_khí.SetActive(true);
        yield return new WaitForSeconds(3f);
        đấu_khí.SetActive(false);
    }
}
