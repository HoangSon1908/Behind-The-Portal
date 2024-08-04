using UnityEngine;
//Tập lệnh này để giúp ta tùy chỉnh mỗi wave quái sắp tới trong SummonEnemy
[System.Serializable]
public class Wave
{
    //khai báo loại kẻ địch và số lượng của nó mỗi wave
    [Header("Waves")]
    public GameObject kẻ_địch;
    public int số_lượng=0;
    public GameObject kẻ_địch_2;
    public int số_lượng_2 = 0;
    public GameObject kẻ_địch_bay;
    public int số_lượng_bay = 0;
}
