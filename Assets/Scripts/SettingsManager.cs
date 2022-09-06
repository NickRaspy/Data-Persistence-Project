using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public struct Resolution
{
    public int x;
    public int y;
    public Resolution(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
public class SettingsManager : MonoBehaviour
{
    public Dropdown changeRes;
    public Dropdown changeFSM;
    public Dropdown changeRefresh;
    public Dropdown changeGameSpeed;
    public Button saveButton;
    public List<Resolution> resolutions = new List<Resolution>();
    public List<int> refreshRates = new List<int>();

    public int x; public int y; public FullScreenMode fsm; public int refresh; public float gameSpeed;
    void Start()
    {
        DataManager.Instance.LoadSettings();
        SettingsData settingsData = DataManager.Instance.settingsData;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].y == settingsData.y)
            {
                changeRes.value = i;
                break;
            }
        }
        FullScreenMode s_FSM = settingsData.fsm;
        switch (s_FSM)
        {
            case FullScreenMode.Windowed:
                changeFSM.value = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                changeFSM.value = 1;
                break;
            case FullScreenMode.ExclusiveFullScreen:
                changeFSM.value = 2;
                break;
        }
        for (int i = 0; i < refreshRates.Count; i++)
        {
            if (refreshRates[i] == settingsData.refresh)
            {
                changeRefresh.value = i;
                break;
            }
        }
        switch (settingsData.gameSpeed)
        {
            case 1f:
                changeGameSpeed.value = 0;
                break;
            case 1.25f:
                changeGameSpeed.value = 1;
                break;
            case 1.5f:
                changeGameSpeed.value = 2;
                break;
        }
        saveButton.gameObject.SetActive(false);
    }
    public void ResolutionChange()
    {
        x = resolutions[changeRes.value].x;
        y = resolutions[changeRes.value].y;
        Debug.Log(x + " " + y);
    }
    public void FSMChange()
    {
        switch (changeFSM.value)
        {
            case 0:
                fsm = FullScreenMode.Windowed;
                break;
            case 1:
                fsm = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                fsm = FullScreenMode.ExclusiveFullScreen;
                break;
        }
        Debug.Log(changeFSM.value);
    }
    public void RefreshRate()
    {
        refresh = refreshRates[changeRefresh.value];
        Debug.Log(refresh);
    }
    public void GameSpeedChange()
    {
        switch (changeGameSpeed.value)
        {
            case 0:
                gameSpeed = 1f;
                break;
            case 1:
                gameSpeed = 1.25f;
                break;
            case 2:
                gameSpeed = 1.5f;
                break;
        }
        Debug.Log(gameSpeed);
    }
    public void Menu()
    {
        SceneManager.LoadSceneAsync("menu");
    }
    public void Save()
    {
        DataManager.Instance.SaveSettings(x, y, fsm, refresh, gameSpeed);
        DataManager.Instance.LoadSettings();
        saveButton.gameObject.SetActive(false);
    }
    public void ShowSave()
    {
        saveButton.gameObject.SetActive(true);
    }
    public void Test()
    {
        changeRes.value = 5;
    }
    public void Wipe()
    {
        DataManager.Instance.WipeData();
    }
    public void Default()
    {
        DataManager.Instance.DefaultSettings();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

