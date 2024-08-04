using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Tùy_tùng : Kẻ_địch
{
    protected override void Start()
    {
        máu = máu_khởi_đầu;
    }

    protected override void hẹo()
    {
        isDead = true;
        sinh_quái.số_lượng_địch--;
        chỉ_số_người_chơi.tinh_thể += tiền;
        Destroy(gameObject);
    }
}
