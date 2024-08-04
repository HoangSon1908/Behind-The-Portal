using UnityEngine;

//Lệnh để xử lý đổi vũ khí của người chơi (Trong WeaponHolder trong Graphics của Player)
public class WeaponSwap : MonoBehaviour
{
    private int currentWeapon = 0;//hàm lưu vũ khí hiện tại
    void Start()
    {
        SelectWeapon();//trang bị vũ khí đầu tiên trong WeaponHolder
    }

    void Update()
    {
        int previousWeapon = currentWeapon;//lưu vũ khí trước khi đổi
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentWeapon >= transform.childCount - 1)//nếu lăn chuột đến quá số súng đăng có thì quay lại súng đầu tiền
            {
                currentWeapon = 0;
            }
            else
            {
                currentWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)//ngược lại với trên
        {
            if (currentWeapon <=0)
            {
                currentWeapon = transform.childCount-1;
            }
            else
            {
                currentWeapon--;
            }
        }
        //Tập lệnh để đổi vũ khí bằng số và kiểm tra có súng ở vị trí không
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount>=2)
        {
            currentWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            currentWeapon = 2;
        }
        if (previousWeapon != currentWeapon)//nếu có thay đổi súng thì gọi lệnh trang bị
        {
            SelectWeapon();
        }
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)//kiểm tra từng súng xem có phải là súng cần thì hiện còn lại ẩn
        {
            if (i == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
