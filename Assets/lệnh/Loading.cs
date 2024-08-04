using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

//Lệnh xử lý cho hiệu ứng chuyển cảnh (Bên trong đối tượng loading)
public class Loading : MonoBehaviour
{
    public Image load;
    public AnimationCurve curve;//tạo hiệu ứng nhanh dần
    public static bool đã_tải;

    private void Start()
    {
        đã_tải = false;
        StartCoroutine(đăng_vào());//tạo hiệu ứng load vào ở mỗi cảnh
    }

    public void tải(string scene)
    {
        if (đã_tải)
        {
            return;
        }
        StartCoroutine(đăng_tải(scene));
    }
    
    public IEnumerator đăng_vào()
    {
        float t = 1f;

        while (t > 0f)
        {
            t-=Time.deltaTime;
            float a =curve.Evaluate(t);
            load.color = new Color(0f, 191f, 255f,a);//tạo hiệu ứng nhạt dần
            yield return 0;
        }
    }

    public IEnumerator đăng_tải(string scene)
    {
        float t = 0f;

        while (t <1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            load.color = new Color(0f, 191f, 255f, a);//tạo hiệu ứng đậm dần
            yield return 0;
        }
        SceneManager.LoadScene(scene);//xong hiệu ứng thì chuyển cảnh
        đã_tải = false;
    }
}
