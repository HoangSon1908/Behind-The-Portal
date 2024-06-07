using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Giao_diện : MonoBehaviour
{
    private điểm_đặt đã_chọn;

    public TextMeshProUGUI chi_phí_nâng;
    public TextMeshProUGUI chi_phí_bán;

    public Button nút_nâng_cấp;

    public GameObject UI;

    public void Chọn(điểm_đặt chọn)
    {
        đã_chọn = chọn;

        transform.position = đã_chọn.nhận_vị_trí_xây();

        if (đã_chọn.đã_nâng)
        {
            chi_phí_nâng.text = "Đã xong";
            nút_nâng_cấp.interactable = false;
        }
        else
        {
            chi_phí_nâng.text = "$" + chọn.thiết_kế.giá_nâng_cấp;
            nút_nâng_cấp.interactable = true;
        }

        chi_phí_bán.text = "$" + chọn.giá_bán;

        UI.SetActive(true);
    }

    public void Ẩn()
    {
        UI.SetActive(false);
    }

    public void nâng_cấp()
    {
        đã_chọn.nâng_cấp_ụ_súng();
        Xây_dựng.instance.hủy_chọn();
    }

    public void bán()
    {
        đã_chọn.bán_ụ_súng();
        Xây_dựng.instance.hủy_chọn();
    }
}
