using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerBody;
    public CharacterController controller;
    public GameObject gameOver, hud;
    public float speed = 12f;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI punteggio;
    public TextMeshProUGUI comboCounter;
    public GameObject comboImage;
    private float timeRemaining;

    //private static bool gameIsPaused;

    private void Start()
    {
        hud.SetActive(true);
        gameOver.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        timeRemaining = 90;
        UpdateTime(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime(timeRemaining);
        }
        else
        {
            hud.SetActive(false);
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
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

    public void UpdateTime(float currentTime)
    {
        float minutes, seconds;
        minutes = Mathf.FloorToInt(timeRemaining / 60);
        seconds = Mathf.FloorToInt(timeRemaining % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
