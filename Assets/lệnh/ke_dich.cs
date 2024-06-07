using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Kẻ_địch : MonoBehaviour
{ 
    [HideInInspector]
    public float tốc_độ;

    public float tốc_độ_khởi_đầu = 10f;

    private Transform rẽ;
    private int điểm = 0;

    public float máu_khởi_đầu = 100f;

    [HideInInspector]
    public float máu;

    public int tiền;

    public GameObject hiệu_ứng_hẹo;

    public Image thanh_máu;

    private bool isDead = false;

    [Header("Chúa tể")]
    public bool chết_tạo_quái;
    public GameObject kẻ_địch;
    public bool tùy_tùng;

    [Header("Đại tướng")]
    public Transform Xoay;
    public bool đại_tướng;
    public bool hồi_sinh;
    public GameObject áo_giáp;
    public GameObject nổ_lớn;

    [Header("Vương giả")]
    public bool Vương_giả;
    public GameObject finality;
    public GameObject tinh_anh;
    public GameObject đấu_khí;


    private void Start()
    {       
        if (!tùy_tùng)
        {
            rẽ = điểm_rẽ.Điểm[0];
        }
        if (Vương_giả)
        {
            InvokeRepeating("nhuệ_khí", 0f, 10f);
        }
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

    void hẹo()
    {
        if (chết_tạo_quái)
        {
            isDead = true;
            sinh_quái.số_lượng_địch--;
            chỉ_số_người_chơi.tinh_thể += tiền;
            tốc_độ_khởi_đầu =0;
            transform.tag = "Untagged";
            StartCoroutine(gọi_tùy_tùng());
        }

        if (!chết_tạo_quái && !hồi_sinh)
        {
            isDead = true;
            GameObject h = (GameObject)Instantiate(hiệu_ứng_hẹo, transform.position, Quaternion.identity);
            Destroy(h, 0.5f);
            chỉ_số_người_chơi.tinh_thể += tiền;
            sinh_quái.số_lượng_địch--;
            Destroy(gameObject);
        }
        if (đại_tướng)
        {
            phase2();
        }
    }

    public void phase2()
    {
        áo_giáp.SetActive(false);
        GameObject hiệu_ứng = (GameObject)Instantiate(nổ_lớn, transform.position, Quaternion.identity) ;
        Destroy(hiệu_ứng, 1f);
        tốc_độ_khởi_đầu = 8;
        máu_khởi_đầu *= 1.25f;
        máu = máu_khởi_đầu;
        thanh_máu.color = new Color(1.0f, 0.65f, 0.0f, 1.0f);
        thanh_máu.fillAmount=1.0f;
        hồi_sinh = false;
    }

    IEnumerator gọi_tùy_tùng()
    {
        for(int i = 0; i < 3; i++)
        {
            sinh_quái.số_lượng_địch++;
            yield return new WaitForSeconds(.25f);
            GameObject newEnemy = Instantiate(kẻ_địch, transform.position, Quaternion.identity);
            Kẻ_địch enemyScript = newEnemy.GetComponent<Kẻ_địch>();
            enemyScript.rẽ = this.rẽ;
            enemyScript.điểm = this.điểm;
        }
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
        transform.Translate(đi.normalized*tốc_độ*Time.deltaTime,Space.World);//di chuyển kẻ địch theo hướng từ vị trí hiện tại đến một vị trí đích với tốc độ đã cho và trong không gian thế giới.

        if (Vector3.Distance(transform.position, rẽ.position) <= 0.1f)
        {
            tiếp_tục();
        }       
        tốc_độ = tốc_độ_khởi_đầu;
    }

    public void nhuệ_khí()
    {
        sinh_quái.số_lượng_địch++;
        StartCoroutine(thức_tỉnh());
        GameObject newEnemy = Instantiate(tinh_anh, transform.position, Quaternion.identity);        
        if(máu<=máu_khởi_đầu-250)
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

    void tiếp_tục()
    {
        if (điểm >= điểm_rẽ.Điểm.Length - 1)
        {
            chỉ_số_người_chơi.tinh_thể += tiền;
            if (chết_tạo_quái)
            {
                chỉ_số_người_chơi.mạng -= 4;
            }
            if (đại_tướng)
            {
                chỉ_số_người_chơi.mạng -= 9;
            }
            if (Vương_giả)
            {
                chỉ_số_người_chơi.mạng -= 9;
            }
            chỉ_số_người_chơi.mạng --;
            sinh_quái.số_lượng_địch--;
            Destroy(gameObject);
            return;
        }
        điểm++;
        rẽ = điểm_rẽ.Điểm[điểm];
        if (đại_tướng)
        {
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
}
