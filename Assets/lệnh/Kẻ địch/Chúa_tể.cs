using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chúa_tể : Kẻ_địch
{
    [Header("Chúa tể")]
    public GameObject kẻ_địch;

    protected override void hẹo()
    {
        isDead = true;
        sinh_quái.số_lượng_địch--;
        chỉ_số_người_chơi.tinh_thể += tiền;
        tốc_độ_khởi_đầu = 0;
        transform.tag = "Untagged";
        StartCoroutine(gọi_tùy_tùng());
    }

    protected override void tiếp_tục()
    {
        if (điểm >= điểm_rẽ.Điểm.Length - 1)
        {
            chỉ_số_người_chơi.tinh_thể += tiền;
            chỉ_số_người_chơi.mạng -= 5;
            sinh_quái.số_lượng_địch--;
            Destroy(gameObject);
            return;
        }
        điểm++;
        rẽ = điểm_rẽ.Điểm[điểm];
    }

    IEnumerator gọi_tùy_tùng()
    {
        for (int i = 0; i < 3; i++)
        {
            sinh_quái.số_lượng_địch++;
            yield return new WaitForSeconds(.25f);
            GameObject newEnemy = Instantiate(kẻ_địch, transform.position, Quaternion.identity);
            Kẻ_địch enemyScript = newEnemy.GetComponent<Tùy_tùng>();
            enemyScript.rẽ = rẽ;
            enemyScript.điểm = điểm;
        }
        Destroy(gameObject);
    }
}
