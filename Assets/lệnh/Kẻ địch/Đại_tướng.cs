using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Đại_tướng : Kẻ_địch
{
    [Header("Đại tướng")]
    public Transform Xoay;
    public bool hồi_sinh;
    public GameObject áo_giáp;
    public GameObject nổ_lớn;

    protected override void hẹo()
    {
        if (!hồi_sinh)
        {
            isDead = true;
            GameObject h = (GameObject)Instantiate(hiệu_ứng_hẹo, transform.position, Quaternion.identity);
            Destroy(h, 0.5f);
            chỉ_số_người_chơi.tinh_thể += tiền;
            sinh_quái.số_lượng_địch--;
            Destroy(gameObject);
            return;
        }
        phase2();
    }
    public void phase2()
    {
        Debug.Log("Ta còn sống");
        áo_giáp.SetActive(false);
        GameObject hiệu_ứng = (GameObject)Instantiate(nổ_lớn, transform.position, Quaternion.identity);
        Destroy(hiệu_ứng, 1f);
        tốc_độ_khởi_đầu = 8;
        máu_khởi_đầu *= 1.25f;
        máu = máu_khởi_đầu;
        thanh_máu.color = new Color(1.0f, 0.65f, 0.0f, 1.0f);
        thanh_máu.fillAmount = 1.0f;
        hồi_sinh = false;
    }

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
        // Tính toán hướng từ vị trí hiện tại của nhân vật đến mục tiêu
        Vector3 hướng = rẽ.position - transform.position;

        // Sử dụng Quaternion để xoay nhân vật theo hướng của mục tiêu
        Quaternion xoay_mục_tiêu = Quaternion.LookRotation(hướng);

        // Lấy góc xoay của quaternion dưới dạng vector Euler (x, y, z)
        Vector3 xoay = xoay_mục_tiêu.eulerAngles;

        // Đặt transform của nhân vật sao cho nó xoay -90 độ thêm trên trục Y so với hướng đến mục tiêu
        Xoay.rotation = Quaternion.Euler(0f, xoay.y - 90f, 0f);
    }
}
