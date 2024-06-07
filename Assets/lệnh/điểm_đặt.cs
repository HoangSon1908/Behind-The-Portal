using UnityEngine.EventSystems;
using UnityEngine;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class điểm_đặt : MonoBehaviour
{
    private Vector3 phần_bù;
    
    private Color màu=Color.gray;

    private Renderer rend;

    private Color màu_ban_đầu;

    public GameObject trụ_súng;
    [HideInInspector]
    public bản_thiết_kế thiết_kế;
    [HideInInspector]
    public bool đã_nâng=false;

    Xây_dựng xây_dựng;

    public int giá_bán;

    private void Start()
    {
        phần_bù = new Vector3(0f, 0.1f, 0f);
        rend = GetComponent<Renderer>();
        màu_ban_đầu = rend.material.color;

        xây_dựng = Xây_dựng.instance;
    }

    public Vector3 nhận_vị_trí_xây()
    {
        return transform.position + phần_bù;
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())//đảm bảo chuột đăng ở trên điểm đặt
            return;
        
        if (!xây_dựng.có_thể_xây)
        {
            return;
        }
        if (!xây_dựng.có_thể_mua || chỉ_số_người_chơi.số_trụ_súng==10)
        {
            rend.material.color=Color.red;
        }else
        {
            rend.material.color = màu;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = màu_ban_đầu;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (trụ_súng != null)
        {
            xây_dựng.chọn_điểm_đặt(this);
            return;
        }

        if (!xây_dựng.có_thể_xây)//nếu không thể xây dựng thì return :)
        {
            return;
        }
        if(chỉ_số_người_chơi.số_trụ_súng == 10)
        {
            return;
        }

        Xây(xây_dựng._ụ_súng());
    }
    void Xây(bản_thiết_kế b)
    {
        if (chỉ_số_người_chơi.tinh_thể < b.giá_mua)
        {
            return;
        }
        chỉ_số_người_chơi.số_trụ_súng++;

        chỉ_số_người_chơi.tinh_thể -= b.giá_mua;

        thiết_kế = b;

        GameObject hiệu_ứng = (GameObject)Instantiate(xây_dựng.hiệu_ứng_xây_dựng,nhận_vị_trí_xây(), Quaternion.identity);
        Destroy(hiệu_ứng, 2f);

        GameObject ụ_súng = (GameObject)Instantiate(b.ụ_súng,nhận_vị_trí_xây(), Quaternion.identity);
        trụ_súng = ụ_súng;

        giá_bán = thiết_kế.giá_mua / 2;
    }

    public void nâng_cấp_ụ_súng()
    {
        if (chỉ_số_người_chơi.tinh_thể < thiết_kế.giá_nâng_cấp)
        {
            return;
        }

        chỉ_số_người_chơi.tinh_thể -= thiết_kế.giá_nâng_cấp;

        giá_bán = (thiết_kế.giá_nâng_cấp + thiết_kế.giá_mua) / 2;

        Destroy(trụ_súng);//phá trụ cũ

        //xây trụ nâng cấp
        GameObject hiệu_ứng = (GameObject)Instantiate(xây_dựng.hiệu_ứng_xây_dựng, nhận_vị_trí_xây(), Quaternion.identity);
        Destroy(hiệu_ứng, 2f);

        GameObject ụ_súng = (GameObject)Instantiate(thiết_kế.ụ_súng_nâng_cấp, nhận_vị_trí_xây(), Quaternion.identity);
        trụ_súng = ụ_súng;

        đã_nâng = true;
    }

    public void bán_ụ_súng()
    {
        chỉ_số_người_chơi.tinh_thể += giá_bán;
        GameObject hiệu_ứng = (GameObject)Instantiate(xây_dựng.hiệu_ứng_bán, nhận_vị_trí_xây(), Quaternion.identity);
        Destroy(hiệu_ứng, 2f);

        Destroy(trụ_súng);//phá trụ cũ
        thiết_kế = null;
        đã_nâng = false;
        chỉ_số_người_chơi.số_trụ_súng--;
    }

    public void va_chạm()
    {
        if (trụ_súng != null)
        {
            Destroy(trụ_súng);
        }
    }
}
