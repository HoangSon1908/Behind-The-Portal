using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class menu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject Hướng_dẫn;
    public Loading loading;

    public void bắt_đầu()
    {
        loading.tải("cảnh game");
        Loading.đã_tải = true;
    }

    public void thoát_game()
    {
        Application.Quit();
    }

    public void hướng_dẫn()
    {
        if (Loading.đã_tải)
        {
            return;
        }
        nhạc_nền.instance.Đăng_mở();
        StartCoroutine(đậm_dần());
    }

    public void thoát()
    {
        if (Loading.đã_tải)
        {
            return;
        }
        nhạc_nền.instance.Đăng_mở();
        StartCoroutine(mờ_dần());//không cần chờ đợi hoàn thành trước khi thực hiện các tác vụ khác
        Loading.đã_tải = true;
    }

    private IEnumerator đậm_dần()
    {
        Hướng_dẫn.SetActive(true);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime*2;
            canvasGroup.alpha = t;
            yield return null;
        }

        // Ẩn đối tượng hướng dẫn khi đã làm giảm dần Alpha xong
    }

    private IEnumerator mờ_dần()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime*2;
            canvasGroup.alpha = t;
            yield return null;
        }

        // Ẩn đối tượng hướng dẫn khi đã làm giảm dần Alpha xong
        Hướng_dẫn.SetActive(false);
        Loading.đã_tải = false;
    }
}