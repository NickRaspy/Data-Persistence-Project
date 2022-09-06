using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using System.IO;
#if (UNITY_EDITOR)
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public Text bestScoreText;
    public InputField nameInput;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.LoadData();
        DataManager.Instance.LoadSettings();
        bestScoreText.text = DataManager.Instance.d_BestName + ": " + DataManager.Instance.d_BestScore;
    }
    public void Play()
    {
        if (!string.IsNullOrEmpty(nameInput.text) && DataManager.Instance.d_Name !=null)
        {
            SceneManager.LoadSceneAsync("main");
        }
    }
    public void HighScore()
    {
        SceneManager.LoadSceneAsync("highscore");
    }
    public void Settings()
    {
        SceneManager.LoadSceneAsync("settings");
    }
    public void SetName()
    {
        DataManager.Instance.d_Name = nameInput.text;
    }
    public void Quit()
    {
        DataManager.Instance.SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
