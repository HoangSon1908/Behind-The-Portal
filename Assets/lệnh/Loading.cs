using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Image load;
    public AnimationCurve curve;
    public static bool đã_tải;

    private void Start()
    {
        đã_tải = false;
        StartCoroutine(đăng_vào());
    }

    public void tải(string scene)
    {
        if (đã_tải)
        {
            return;
        }
        nhạc_nền.instance.Đăng_tải();
        StartCoroutine(đăng_tải(scene));
    }
    
    public IEnumerator đăng_vào()
    {
        float t = 1f;

        while (t > 0f)
        {
            t-=Time.deltaTime;
            float a =curve.Evaluate(t);
            load.color = new Color(0f, 191f, 255f,a);
            yield return 0;
        }
    }

    public IEnumerator đăng_tải(string scene)
    {
        nhạc_nền.instance.Đăng_tải();
        if (!nhạc_nền.instance.Nhạc_nền.isPlaying)
        {
            nhạc_nền.instance.phát_lại();
        }
        float t = 0f;

        while (t <1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            load.color = new Color(0f, 191f, 255f, a);
            yield return 0;
        }
        SceneManager.LoadScene(scene);
        đã_tải = false;
    }
}
