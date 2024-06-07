using UnityEngine;

public class cửa_hàng : MonoBehaviour
{
    Xây_dựng xây_dựng;
    
    public bản_thiết_kế trụ_súng;//tạo ra chỗ chỉnh sửa cho các trụ    
    public bản_thiết_kế trụ_pháo;
    public bản_thiết_kế trụ_laser;
    private void Start()
    {
        xây_dựng = Xây_dựng.instance;//tiện cho việc sử dụng Xây_dựng sau này
    }

    public void chọn_trụ_súng()
    {
        xây_dựng.chọn_ụ_súng(trụ_súng);
    }

    public void chọn_trụ_pháo()
    {
        xây_dựng.chọn_ụ_súng(trụ_pháo);
    }
    public void chọn_trụ_laser()
    {
        xây_dựng.chọn_ụ_súng(trụ_laser);
    }

    private void Update()
    {
        if (Quản_lý.kết_thúc == true)
        {
            this.enabled = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            chọn_trụ_súng();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            chọn_trụ_pháo();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            chọn_trụ_laser();
        }
    }
}
