using UnityEngine;

public class camera : MonoBehaviour
{
    public float tốc_độ = 20f;

    public float tốc_độ_lăn = 5f;

    public float y_nhỏ_nhất = 10f;
    public float y_lớn_nhất = 80f;

    private float giới_hạn_di_chuyển = 10f;
    void Update()
    {
        if (Quản_lý.kết_thúc == true)
        {
            this.enabled = false;
            return;
        }
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward*tốc_độ*Time.deltaTime,Space.World); 
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * tốc_độ * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * tốc_độ * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * tốc_độ * Time.deltaTime, Space.World);
        }

        // Giới hạn vị trí cam
        Vector3 vị_trí_cam = transform.position;
        vị_trí_cam.x = Mathf.Clamp(vị_trí_cam.x, 23f - giới_hạn_di_chuyển, 23f + giới_hạn_di_chuyển);
        vị_trí_cam.z = Mathf.Clamp(vị_trí_cam.z, -33f - giới_hạn_di_chuyển, -33f + giới_hạn_di_chuyển);
        transform.position = vị_trí_cam;

        float lăn_chuột = Input.GetAxis("Mouse ScrollWheel");//phóng to thu nhỏ màn hình

        Vector3 lăn=transform.position;
        lăn.y -= lăn_chuột * 1000 * tốc_độ_lăn * Time.deltaTime;
        lăn.y = Mathf.Clamp(lăn.y, y_nhỏ_nhất, y_lớn_nhất);//giới hạn khả năng thu phóng màn hình :)
        transform.position = lăn;
    }
}
