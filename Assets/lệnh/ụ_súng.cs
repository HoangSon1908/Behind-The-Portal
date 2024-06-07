using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ụ_súng : MonoBehaviour
{   
    private Transform mục_tiêu;
    private float đếm_ngược = 0f;
    private Kẻ_địch k_mục_tiêu;

    public string kẻ_thù = "kẻ_thù";

    [Header("thông tin súng")]
    public float khoảng_cách = 15f;
    public float tốc_độ_bắn = 1f;

    [Header("thông tin đạn")]
    public GameObject đạn;
    public Transform vị_trí_đạn;

    [Header("laser")]
    public bool Laser=false;
    public LineRenderer lineRenderer;
    public ParticleSystem hiệu_ứng_laser;
    public Light ánh_sáng;
    public int sát_thương = 5;
    public float làm_chậm = 0.5f;
    
    void Start()//gọi hàm chọn mục tiêu mỗi 0.5s
    {
        InvokeRepeating("chọn_mục_tiêu", 0f, 0.25f);
    }

    void chọn_mục_tiêu()//chọn và bỏ mục tiêu
    {
        GameObject[] kẻ_địch = GameObject.FindGameObjectsWithTag(kẻ_thù);
        float khoảng_cách_ngắn_nhất = Mathf.Infinity;
        GameObject kẻ_thù_gần_nhất = null;

        foreach(GameObject kẻ_thù in kẻ_địch)
        {
            float khoảng_cách_kẻ_thù=Vector3.Distance(transform.position,kẻ_thù.transform.position);
            if (khoảng_cách_kẻ_thù < khoảng_cách_ngắn_nhất)
            {
                khoảng_cách_ngắn_nhất = khoảng_cách_kẻ_thù;
                kẻ_thù_gần_nhất = kẻ_thù;
            }
        }
        if (kẻ_thù_gần_nhất != null && khoảng_cách_ngắn_nhất <= khoảng_cách)
        {
            mục_tiêu = kẻ_thù_gần_nhất.transform;
            k_mục_tiêu = kẻ_thù_gần_nhất.GetComponent<Kẻ_địch>();
        }
        else
        {
            mục_tiêu = null;
        }
    }
    
    void Update()//liên tục xoay về phía kẻ thù chỉ trên trục y
    {

        if (Quản_lý.kết_thúc == true)
        {
            this.enabled = false;
            return;
        }
        if (mục_tiêu == null)
        {
            if (Laser)
            {
                if(lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    hiệu_ứng_laser.Stop();
                    ánh_sáng.enabled = false;
                }
            }
            return;
        }

        khóa_mục_tiêu();

        if (Laser)
        {
            bắn_laser();
        }
        else
        {
            if (đếm_ngược <= 0f)
            {
                bắn();
                đếm_ngược = 1f / tốc_độ_bắn;
            }
            đếm_ngược -= Time.deltaTime;
        }
    }

    void khóa_mục_tiêu()
    {
        // Tính toán hướng từ vị trí của nhân vật đến vị trí của mục tiêu
        Vector3 hướng = mục_tiêu.position - transform.position;

        // Tạo một quaternion (nhìn) để quay transform của nhân vật hướng về mục tiêu
        Quaternion nhìn = Quaternion.LookRotation(hướng);

        // Lấy góc xoay của quaternion dưới dạng vector Euler (x, y, z)
        Vector3 xoay = nhìn.eulerAngles;

        // Đặt transform của nhân vật sao cho nó xoay 90 độ thêm trên trục Y so với hướng đến mục tiêu
        transform.rotation = Quaternion.Euler(0f, xoay.y + 90f, 0f);
    }

    void bắn_laser()
    {
        if (!k_mục_tiêu)
        {
            return;
        }
        k_mục_tiêu.nhận_sát_thương(sát_thương*Time.deltaTime);
        k_mục_tiêu.Làm_chậm(làm_chậm);
        
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            hiệu_ứng_laser.Play();
            ánh_sáng.enabled = true;
        }

        lineRenderer.SetPosition(0, vị_trí_đạn.position);
        lineRenderer.SetPosition(1, mục_tiêu.position);

        Vector3 hướng = vị_trí_đạn.position - mục_tiêu.position;

        hiệu_ứng_laser.transform.rotation = Quaternion.LookRotation(hướng);

        hiệu_ứng_laser.transform.position = mục_tiêu.position + hướng.normalized * 0.5f;//di chuyển theo hướng  có độ dài bằng nửa khoảng cách ban đầu.
    }

    void bắn()
    {
        GameObject _Đạn= (GameObject)Instantiate(đạn, vị_trí_đạn.position, vị_trí_đạn.rotation);
        Đạn đạn_bắn=_Đạn.GetComponent<Đạn>();
        if (đạn_bắn != null)
        {
            đạn_bắn.khóa_mục_tiêu(mục_tiêu);
        }
    }

    private void OnDrawGizmosSelected()//khi ụ súng bị chọn sẽ thực hiện lệnh
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, khoảng_cách);//vẽ  hình tròn màu xanh thể hiện phạm vi súng
    }
}
