using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Đạn : MonoBehaviour
{
    private Transform mục_tiêu;
    public float tốc_độ = 1f;

    public float phạm_vi=0f;

    public GameObject hiệu_ứng;

    public int Sát_thương;
    public void khóa_mục_tiêu(Transform Mục_tiêu)
    {
        mục_tiêu = Mục_tiêu;
    }
   
    void Update()
    {

        if (Quản_lý.kết_thúc == true)
        {
            this.enabled = false;
            return;
        }
        if (mục_tiêu == null)//loại bỏ đối tượng Đạn khi mục tiêu không còn tồn tại.
        {
            Destroy(gameObject);
            return;
        }

        Vector3 bắn = mục_tiêu.position - transform.position;
        float khoảng_cách = tốc_độ * Time.deltaTime;
        if (bắn.magnitude <=khoảng_cách)//kiểm tra đạn đã chạm hay chưa
        {
            chạm_mục_tiêu();
        }

        transform.Translate(bắn.normalized * khoảng_cách, Space.World);
        transform.LookAt(mục_tiêu);//nhìn vào mục tiêu
    }

    void chạm_mục_tiêu()
    {
        GameObject Hiệu_ứng = (GameObject)Instantiate(hiệu_ứng, transform.position, transform.rotation);
        Destroy(Hiệu_ứng, 0.5f);

        if (phạm_vi > 0)
        {
            Nổ();
        }
        else
        {
            sát_thương(mục_tiêu);
        }

        Destroy(gameObject);
    }

    void Nổ()
    {
        Collider[] va_chạm = Physics.OverlapSphere(transform.position, phạm_vi);
        foreach(Collider va in va_chạm)
        {
            if (va.tag == "kẻ_thù")
            {
                sát_thương(va.transform);
            }
        }
    }

    void sát_thương(Transform kẻ_địch)
    {
        Kẻ_địch k = kẻ_địch.GetComponent<Kẻ_địch>();
        if (k != null)
            k.nhận_sát_thương(Sát_thương);
    }

    private void OnDrawGizmosSelected()//khi đạn bị chọn sẽ thực hiện lệnh
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, phạm_vi);//vẽ  hình tròn màu xanh thể hiện phạm vi nổ
    }
}
