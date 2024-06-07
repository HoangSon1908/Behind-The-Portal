using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Xây_dựng : MonoBehaviour
{
    public static Xây_dựng instance;

    void Awake()//đảm bảo rằng chỉ có một thể hiện duy nhất của lớp này tồn tại trong cảnh.(vì có quá nhiều khối vuông ý mà)
    {
        instance = this;
    }
    
    private bản_thiết_kế xây_dựng;
    private điểm_đặt lựa_chọn;

    public GameObject hiệu_ứng_xây_dựng;

    public GameObject hiệu_ứng_bán;

    public Giao_diện giao_diện;

    public bool có_thể_xây{ get { return xây_dựng != null; } }//kiểm tra xem có thể xây dựng tháp không dựa trên việc có bản thiết kế nào được chọn 
    public bool có_thể_mua { get { return chỉ_số_người_chơi.tinh_thể >= xây_dựng.giá_mua; } }

    public void chọn_điểm_đặt(điểm_đặt Điểm_đặt)
    {
        if (lựa_chọn == Điểm_đặt)
        {
            hủy_chọn();
            return;
        }
        
        lựa_chọn = Điểm_đặt;

        xây_dựng = null;

        giao_diện.Chọn(Điểm_đặt);
    }
    
    public void hủy_chọn()
    {
        lựa_chọn = null;
        giao_diện.Ẩn();
    }
    public void chọn_ụ_súng(bản_thiết_kế ụ_súng)
    {
        xây_dựng = ụ_súng;
        nhạc_nền.instance.đổi();
        hủy_chọn();
    }

    public bản_thiết_kế _ụ_súng()
    {
        return xây_dựng;
    }
}
