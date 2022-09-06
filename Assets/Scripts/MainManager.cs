using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public GameObject brickHolder;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    public GameObject pauseUI;
    public AudioSource soundMaker;
    
    private bool m_Started = false;
    private bool m_Paused = false;
    public int m_Points;
    public int m_BestScore = 0;
    public float m_gameSpeed;
    
    private bool m_GameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        m_gameSpeed = DataManager.Instance.settingsData.gameSpeed;
        Time.timeScale = m_gameSpeed;
        HighScoreText.text = "Best Score: " + DataManager.Instance.d_BestName + ": " + DataManager.Instance.d_BestScore;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.transform.SetParent(brickHolder.transform);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                brick.playSound.AddListener(PlaySound);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_Paused = !m_Paused;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadSceneAsync("menu");
            }
        }
        if (m_Paused)
        {
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = m_gameSpeed;
            pauseUI.SetActive(false);
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }
    void PlaySound()
    {
        soundMaker.Play();
    }

    public void GameOver()
    {
        m_GameOver = true;
        DataManager.Instance.d_Score = m_Points;
        DataManager.Instance.SaveData();
        DataManager.Instance.LoadData();
        HighScoreText.text = "Best Score: " + DataManager.Instance.d_BestName + ": " + DataManager.Instance.d_BestScore;
        GameOverText.SetActive(true);
    }
    public void Continue()
    {
        m_Paused = !m_Paused;
    }
    public void ExitToMenu()
    {
        SceneManager.LoadSceneAsync("menu");
    }
}
