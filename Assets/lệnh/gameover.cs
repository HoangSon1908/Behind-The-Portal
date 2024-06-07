using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class gameover : MonoBehaviour
{
    public Text Lượt;

    public Loading loading;

    private void OnEnable()
    {
        Lượt.text = chỉ_số_người_chơi.lượt + "/16";
        StartCoroutine(AnimateText());
    }

    public void thử_lại()
    {
        loading.tải(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        loading.tải("menu");
    }

    IEnumerator AnimateText()
    {
        Lượt.text = "0";
        int round = 0;

        yield return new WaitForSeconds(.7f);

        while (round < chỉ_số_người_chơi.lượt)
        {
            round++;
            Lượt.text = round.ToString();

            yield return new WaitForSeconds(.05f);
        }
    }
}
