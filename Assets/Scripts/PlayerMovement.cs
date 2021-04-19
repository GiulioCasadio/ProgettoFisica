using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerBody;
    public CharacterController controller;
    //public GameObject menu;
    public float speed = 12f;

    //private static bool gameIsPaused;

    private void Start()
    {
        //gameIsPaused = false;
        Time.timeScale = 1;
        //menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;

        controller.Move(move * speed * Time.deltaTime);

       /* if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }*/
    }

   /* public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        menu.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    public void Demo1()
    {
        SceneManager.LoadScene("Scenes/FruitNinjaLike");
    }

    public void Demo2()
    {
        SceneManager.LoadScene("Scenes/ShooterLike");
    }

    public void Demo3()
    {
        SceneManager.LoadScene("Scenes/WallDestroyable");
    }

    public void Demo4()
    {
        SceneManager.LoadScene("Scenes/WallUndestroyable");
    }

    public void QuitDemo()
    {
        Application.Quit();
    }*/
}
