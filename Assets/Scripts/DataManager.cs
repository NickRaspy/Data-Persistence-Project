using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private MainManager mainManager;
    public int d_BestScore = 0;
    public string d_Name;
    public string d_NewName;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(SceneManager.GetActiveScene().name == "main")
        {
            mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    class Data
    {
        public int Score;
        public string Name;
    }
    public void SaveData()
    {
        Data data = new Data();
        data.Score = d_BestScore;
        data.Name = d_Name;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data data = JsonUtility.FromJson<Data>(json);

            d_BestScore = data.Score;
            d_Name = data.Name;
        }
    }
}
