using UnityEngine;

//Lệnh cho việc xoay camera của Player (Trong Player)
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Độ nhạy của chuột
    public Transform LookAt; //đảm nhận việc xoay cho camera
    public Transform Player;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ chuột ở trung tâm màn hình
    }

    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 45f); // Giới hạn góc xoay theo trục dọc

        LookAt.localRotation = Quaternion.Euler(xRotation,0f, 0f); // Xoay camera của nhân vật

        Player.Rotate(Vector3.up * mouseX); // Xoay cơ thể của nhân vật theo trục ngang
    }
}


