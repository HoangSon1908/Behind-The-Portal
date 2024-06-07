using UnityEngine.SceneManagement;
using UnityEngine;

public class Tạm_dừng : MonoBehaviour
{
    public GameObject pause;

    public Loading loading;
    
    private void Update()
    {
        if (Quản_lý.kết_thúc == true)
            {
                this.enabled = false;
                return;
            }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            tạm_dừng();
        }
    }

    public void tạm_dừng()
    {
        nhạc_nền.instance.Đăng_mở();
        pause.SetActive(!pause.activeSelf);//đảo giữa bật và tắt tạm dừng.

        if (pause.activeSelf)
        {
            nhạc_nền.instance.Nhạc_nền.Pause();
            nhạc_nền.instance.boss_theme.Pause();
            Time.timeScale = 0f;//tạm dừng thời gian
        }
        else
        {
            nhạc_nền.instance.Nhạc_nền.UnPause();
            nhạc_nền.instance.boss_theme.UnPause();
            Time .timeScale = 1f;//đặt thời gian vè bình thường
        }
    }
    
    public void thử_lại()
    {
        tạm_dừng();
        loading.tải(SceneManager.GetActiveScene().name); 
    }

    public void Menu()
    {
        tạm_dừng();
        loading.tải("menu");
    }
}
