using UnityEngine;

//Lệnh để phát nhạc cho từng trường hợp (Bên trong đối tượng Quản lý ở cảnh Menu)
public class nhạc_nền : MonoBehaviour
{
    public static nhạc_nền instance;

    public AudioSource Nhạc_nền;

    public AudioSource Nhạc_win;

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
        Nhạc_win.Stop();
        Nhạc_nền.Play();
    }

    public void WinMusic()
    {
        Nhạc_nền.Stop();
        Nhạc_win.Play();
    }
}
