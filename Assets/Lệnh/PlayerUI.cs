using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Lệnh để xử lý UI cho màn hình player (Trong PlayerUI)
public class PlayerUI : MonoBehaviour {

    public static PlayerUI instance;

    public Loading loading;

    public GameObject Winner;

    public GameObject Lose;
    public Text Lượt;

    //Thanh máu của Boss khi xuất hiện
    public RectTransform healthBarBossFill;
    public GameObject healthBarBoss;

    //Thanh năng lượng của Player
    public RectTransform EnergyFill;

    [SerializeField]
    GameObject pauseMenu;

    public Player player;

    public static bool isPaused;

    void Awake ()
    {
        instance = this;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }
        //Lệnh để xử lý việc tạm dừng bằng nút esc
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        Energy(Player.instance.GetCurrentEnergy());//cập nhật năng lượng của người chơi
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Menu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = false;
        Time.timeScale = 1f;
        if (nhạc_nền.instance.Nhạc_win.isPlaying)
        {
            nhạc_nền.instance.nhạc();
        }
        loading.tải("menu");
    }

    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        Time.timeScale = 1f;
        if (nhạc_nền.instance.Nhạc_win.isPlaying)
        {
            nhạc_nền.instance.nhạc();
        }
        loading.tải("Gameplay");
    }

    public void BossHealth(float health,float maxHealth)
    {
        float currentHealth = health / maxHealth;
        healthBarBossFill.localScale = new Vector3(1f, currentHealth, 1f);
    }
    public void Energy(float energy)
    {
        EnergyFill.localScale = new Vector3(1f, energy, 1f);
    }

    public void WINNER()
    {
        StartCoroutine(winner());
    }
    public IEnumerator winner()
    {
        yield return new WaitForSeconds(1f);
        nhạc_nền.instance.WinMusic();
        Winner.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void youLose()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Lose.SetActive(true);
        StartCoroutine(AnimateText());
        isPaused = true;
    }

    IEnumerator AnimateText()
    {
        Lượt.text = "0";
        int round = 0;
        int currentRound = sinh_quái.instance.Wave();

        yield return new WaitForSeconds(.7f);

        while (round < currentRound)
        {
            round++;
            Lượt.text = round.ToString();

            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(.1f);
        Time.timeScale = 0f;
    }
}
