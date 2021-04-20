using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Scenes/MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
