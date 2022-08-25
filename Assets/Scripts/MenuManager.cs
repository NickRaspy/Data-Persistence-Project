using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
#if(UNITY_EDITOR)
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public Text bestScoreText;
    public DataManager dataManager;
    public InputField nameInput;
    // Start is called before the first frame update
    void Start()
    {
        dataManager.LoadData();
        bestScoreText.text = dataManager.d_Name + ": " + dataManager.d_BestScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            SceneManager.LoadSceneAsync("main");
        }
    }
    public void SetName()
    {
        dataManager.d_NewName = nameInput.text;
    }
    public void Quit()
    {
        dataManager.SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
