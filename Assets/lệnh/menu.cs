using System.Collections;
using UnityEngine;

//Lệnh cho đồ họa menu (Trong đối tượng UI cảnh Menu)
public class menu : MonoBehaviour
{
    public CanvasGroup canvasGroup;//độ đậm của Hướng_dẫn
    public GameObject Hướng_dẫn;
    public GameObject Continue;
    public Loading loading;

    public void bắt_đầu()
    {
        loading.tải("gameplay");//gọi lệnh hiệu ứng loading
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
        StartCoroutine(đậm_dần());
    }

    private IEnumerator đậm_dần()//tạo hiệu ứng mở Hướng_dẫn
    {
        Hướng_dẫn.SetActive(true);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime*2;
            canvasGroup.alpha = t;
            yield return null;
        }
        yield return new WaitForSeconds(3);
        Continue.SetActive(true);
    }
}