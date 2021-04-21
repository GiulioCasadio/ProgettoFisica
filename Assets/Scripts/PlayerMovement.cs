using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerController")]
    public Transform playerBody;
    public CharacterController controller;
    public GameObject gameOver, hud, pause;
    public float speed = 12f;

    [Header("Hud")]
    public TextMeshProUGUI timer;
    public TextMeshProUGUI punteggio;
    public TextMeshProUGUI comboCounter;
    public GameObject comboImages;
    public TextMeshProUGUI extraTime;

    [Header("Colors")]
    public Sprite red;
    public Sprite blue;
    public Sprite green;

    [Header("GameOver")]
    public TextMeshProUGUI nuovoPunteggio;
    public TextMeshProUGUI record;

    [Header("Pause")]
    public TextMeshProUGUI nuovoPunteggioPausa;
    public TextMeshProUGUI recordPausa;

    private float timeRemaining;
    private int punteggioCounter, combo;
    private int[] color = new int[3] { 4, 5, 5 };
    private int oldScore;
    private static bool gameIsPaused;


    private void Start()
    {
        gameIsPaused = false;
        oldScore = PlayerPrefs.GetInt("score", 0);
        punteggioCounter = 0;
        combo = 1;
        hud.SetActive(true);
        gameOver.SetActive(false);
        pause.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        timeRemaining = 60;
        UpdateTime(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (oldScore < punteggioCounter)
        {
            PlayerPrefs.SetInt("score", punteggioCounter);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime(timeRemaining);
        }
        else
        {
            GameOver();
        }
    }

    public void Pause()
    {
        pause.SetActive(true);
        hud.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        gameIsPaused = true;
        
        recordPausa.text = "Record: " + PlayerPrefs.GetInt("score", 0);
        nuovoPunteggioPausa.text = "Punteggio: " + punteggioCounter;
    }

    public void Resume()
    {
        pause.SetActive(false);
        hud.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        hud.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        record.text = "Record: " + PlayerPrefs.GetInt("score", 0);
        nuovoPunteggio.text = "Punteggio: " + punteggioCounter;
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

    public void AddPoint(int colorHit) // 1 = red, 2 = blue, 3 = green
    {
        color[2] = color[1];
        color[1] = color[0];
        color[0] = colorHit;

        // Assegno i giusti sprite
        for(int i=0; i<3; i++)
        {
            Image im = comboImages.transform.GetChild(i).transform.GetComponent<Image>();
            var v = im.color;
            v.a = 1f;
            im.color = v;
            switch (color[i])
            {
                case 1:
                    im.sprite = red;
                    break;
                case 2:
                    im.sprite = blue;
                    break;
                case 3:
                    im.sprite = green;
                    break;
                default:
                    v.a = 0.1f;
                    im.color = v;
                    break;
            }
        }
        
        // Controllo Combo
        if (color[0] == color[1] && color[0] == color[2])
        {
            // Se avevo fatto la stessa combo allora aggiungo tempo extra
            if ((comboCounter.color == Color.red && color[0] == 1) ||
                (comboCounter.color == Color.blue && color[0] == 2) ||
                (comboCounter.color == Color.green && color[0] == 3))
            {
                timeRemaining += 10;
                TextMeshProUGUI ex = Instantiate(extraTime) as TextMeshProUGUI;
                ex.transform.SetParent(hud.transform);
                ex.transform.position = new Vector2(timer.transform.position.x+110, timer.transform.position.y);
            }

            // cambio il colore della combo
            switch (color[0])
            {
                case 1:
                    comboCounter.color = Color.red;
                    break;
                case 2:
                    comboCounter.color = Color.blue;
                    break;
                case 3:
                    comboCounter.color = Color.green;
                    break;
            }

            // eliminio dall'hud i colori
            for (int i = 0; i < 3; i++)
            {
                Image im = comboImages.transform.GetChild(i).transform.GetComponent<Image>();
                var v = im.color;
                v.a = 0.1f;
                im.color = v;
                im.sprite = null;
            }
            
            // Aumnto del moltiplicatore
            comboCounter.text = "Combo x" + ++combo;
            color[0] = 4;
            color[1] = color[2] = 5;
        }

        punteggioCounter = punteggioCounter + 10*combo;
        punteggio.text = "Punteggio: " + punteggioCounter;
    }
}
