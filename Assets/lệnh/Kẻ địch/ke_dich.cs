using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Kẻ_địch : MonoBehaviour
{ 
    [HideInInspector]
    public float tốc_độ;

    public float tốc_độ_khởi_đầu = 10f;

    [HideInInspector]
    public Transform rẽ;
    [HideInInspector]
    public int điểm = 0;

    public float máu_khởi_đầu = 100f;

    [HideInInspector]
    public float máu;

    public int tiền;

    public GameObject hiệu_ứng_hẹo;

    public Image thanh_máu;

    protected bool isDead = false;

    protected virtual void Start()
    {       
        rẽ = điểm_rẽ.Điểm[0];
        máu = máu_khởi_đầu;
    }

    public void nhận_sát_thương(float sát_thương)
    {
        máu -= sát_thương;

        thanh_máu.fillAmount = máu/máu_khởi_đầu ;

        if (máu <= 0 && !isDead)
        {
            hẹo();
        }
    }

    public void Làm_chậm(float làm_chậm)
    {
        if (tốc_độ> tốc_độ_khởi_đầu * (1f - làm_chậm))
        {
            tốc_độ = tốc_độ_khởi_đầu * (1f - làm_chậm);
        }
    }

    protected virtual void hẹo()
    {
        isDead = true;
        GameObject h = (GameObject)Instantiate(hiệu_ứng_hẹo, transform.position, Quaternion.identity);
        Destroy(h, 0.5f);
        chỉ_số_người_chơi.tinh_thể += tiền;
        sinh_quái.số_lượng_địch--;
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Quản_lý.kết_thúc == true)
        {
            this.enabled = false;
            return;
        }

        Vector3 đi = rẽ.position - transform.position;
        transform.Translate(Time.deltaTime * tốc_độ * đi.normalized,Space.World);//di chuyển kẻ địch theo hướng từ vị trí hiện tại đến một vị trí đích với tốc độ đã cho và trong không gian thế giới.

        if (Vector3.Distance(transform.position, rẽ.position) <= 0.1f)
        {
            tiếp_tục();
        }       
        tốc_độ = tốc_độ_khởi_đầu;
    }

    protected virtual void tiếp_tục()
    {
        if (điểm >= điểm_rẽ.Điểm.Length - 1)
        {
            chỉ_số_người_chơi.tinh_thể += tiền;
            chỉ_số_người_chơi.mạng --;
            sinh_quái.số_lượng_địch--;
            Destroy(gameObject);
            return;
        }
        điểm++;
        rẽ = điểm_rẽ.Điểm[điểm];
    }
}
