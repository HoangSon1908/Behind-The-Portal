using UnityEngine;
using System.Collections;

public class nhạc_nền : MonoBehaviour
{
    public static nhạc_nền instance;

    public AudioSource Nhạc_nền;

    public AudioSource đổi_trụ_súng;

    public AudioSource đăng_tải;

    public AudioSource đăng_mở;

    public AudioSource đã_thua;

    public AudioSource yatta;

    public AudioSource boss_theme;

    private void Awake()
    {
        // Đảm bảo chỉ có một nhạc_nền tồn tại trong trò chơi.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không hủy GameObject này khi chuyển cảnh.
        }
        else
        {
            Destroy(gameObject); // Đã tồn tại nhạc_nền khác, hủy bản sao này.
            return;
        }
        nhạc();
    }

    public void nhạc()
    {
        Nhạc_nền.Play();
    }
    public void đổi()
    {
        đổi_trụ_súng.Play();
    }

    public void nhạc_boss()
    {
        đã_thua.Stop();
        Nhạc_nền.Stop();
        boss_theme.Play();
    }

    public void Đã_thua()
    {
        boss_theme.Stop();
        Nhạc_nền.Stop();
        đã_thua.Play();
    }

    public void Đăng_tải()
    {
        đăng_tải.Play();
    }

    public void Đăng_mở()
    {
        đăng_mở.Play();
    }
    public void đã_thắng()
    {
        yatta.Play();
    }

    public void phát_lại()
    {
        StartCoroutine(chờ_phát_nhạc());
    }

    public IEnumerator chờ_phát_nhạc()
    {
        đã_thua.Stop();
        Nhạc_nền.Stop();
        boss_theme.Stop();
        yield return new WaitForSeconds(3);
        nhạc_nền.instance.nhạc();
    }
}
