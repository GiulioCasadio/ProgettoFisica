using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerBody;
    public CharacterController controller;
    public GameObject gameOver, hud;
    public float speed = 12f;

    //private static bool gameIsPaused;

    private void Start()
    {
        hud.SetActive(true);
        gameOver.SetActive(false);
        Time.timeScale = 1;
        gameOver.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;

        controller.Move(move * speed * Time.deltaTime);

    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        hud.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Scenes/Start");
    }

    public void RetartScene()
    {
        SceneManager.LoadScene("Scenes/MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
