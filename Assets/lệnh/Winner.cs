using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Winner : MonoBehaviour
{
    public Loading loading;

    public void thử_lại()
    {
        loading.tải(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        loading.tải("menu");
    }
}
