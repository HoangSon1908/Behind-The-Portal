using UnityEngine;

[System.Serializable]//cho phép các dữ liệu của một lớp  được hiển thị và chỉnh sửa trong Inspector
public class bản_thiết_kế
{        
    public GameObject ụ_súng;
    public int giá_mua;

    public GameObject ụ_súng_nâng_cấp;
    public int giá_nâng_cấp;

}
[System.Serializable]
public class Wave
{
    [Header("đợt1")]
    public GameObject kẻ_địch1;
    public int số_lượng1=0;
    public float rate1=0.5f;

    [Header("đợt2")]
    public float thời_gian_trước_đợt2=1;
    public GameObject kẻ_địch2;
    public int số_lượng2 = 0;
    public float rate2 = 0.5f;
}
