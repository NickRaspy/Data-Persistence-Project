using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
[Serializable]
public struct SettingsData
{
    public int x; public int y; public FullScreenMode fsm; public int refresh; public float gameSpeed;
    public SettingsData(int x, int y, FullScreenMode fsm, int refresh, float gameSpeed)
    {
        this.x = x; this.y = y; this.fsm = fsm; this.refresh = refresh; this.gameSpeed = gameSpeed;
    }
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public int d_BestScore = 0;
    public int d_Score = 0;
    public string d_Name;
    public string d_BestName;
    public float d_gameSpeed;

    public List<Data> data = new List<Data>();

    public SettingsData settingsData = new SettingsData();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Serializable]
    public class Data
    {
        public int Score;
        public string Name;
    }
    [Serializable]
    public class Settings
    {
        public int ScreenX;
        public int ScreenY;
        public FullScreenMode FullScreenMode;
        public int refreshRate;
        public float gameSpeed;
    }
    public void SaveData()
    {
        if(data.Count > 0)
        {
            bool isExist = false;
            for (int i = 0; i <  data.Count; i++)
            {
                if(data[i].Name == d_Name)
                {
                    if (data[i].Score < d_Score)
                    {
                        data[i].Score = d_Score;
                    }
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                Data newData = new Data();
                newData.Score = d_Score;
                newData.Name = d_Name;
                data.Add(newData);
            }
        }
        else
        {
            Data newData = new Data();
            newData.Score = d_Score;
            newData.Name = d_Name;
            data.Add(newData);
        }
        BubbleSort(data.ToArray());
        string json = JsonHelper.ToJson(data.ToArray());
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if(JsonHelper.FromJson<Data>(json) != null)
            {
                data = new List<Data>(JsonHelper.FromJson<Data>(json));
                d_BestScore = data[0].Score;
                d_BestName = data[0].Name;
            }
        }
    }
    public void LoadAllData(List<string> allData)
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = new List<Data>(JsonHelper.FromJson<Data>(json));
            for(int i = 0; i < data.Count; i++)
            {
                string newData = (i + 1).ToString() + ". " + data[i].Name + ": " + data[i].Score.ToString();
                allData.Add(newData);
            }
        }
    }
    public void SaveSettings(int x, int y, FullScreenMode fsm, int refresh, float gameSpeed)
    {
        Settings settings = new Settings
        {
            ScreenX = x,
            ScreenY = y,
            FullScreenMode = fsm,
            refreshRate = refresh,
            gameSpeed = gameSpeed
        };
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.persistentDataPath + "/settings.json", json);
    }
    public void LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Settings settings = JsonUtility.FromJson<Settings>(json);
            Screen.SetResolution(settings.ScreenX, settings.ScreenY, settings.FullScreenMode, settings.refreshRate);
            d_gameSpeed = settings.gameSpeed;
            settingsData = new SettingsData
            {
                x = settings.ScreenX,
                y = settings.ScreenY,
                fsm = settings.FullScreenMode,
                refresh = settings.refreshRate,
                gameSpeed = settings.gameSpeed
            };
        }
        else
        {
            DefaultSettings();
        }
    }
    public void WipeData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            data.Clear();
            d_BestName = null;
            d_BestScore = 0;
            d_Score = 0;
            d_Name = null;
        }
    }
    public void DefaultSettings()
    {
        Settings settings = new Settings
        {
            ScreenX = Display.main.systemWidth,
            ScreenY = Display.main.systemHeight,
            FullScreenMode = FullScreenMode.Windowed,
            refreshRate = 60,
            gameSpeed = 1f
        };
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.persistentDataPath + "/settings.json", json);
    }
    private static void BubbleSort(Data[] array)
    {
        for (int i = 0; i < array.Length; i++)
            for (int j = 0; j < array.Length - 1; j++)
                if (array[j].Score < array[j + 1].Score)
                {
                    (array[j].Score, array[j + 1].Score) = (array[j + 1].Score, array[j].Score);
                    (array[j].Name, array[j + 1].Name) = (array[j + 1].Name, array[j].Name);
                }
    }
}
