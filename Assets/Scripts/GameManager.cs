using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using NUnit.Framework.Constraints;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string playerName;
    public TMP_InputField nameInput;
    public string nameInputText;
    public int HighestScore;

    public TextMeshProUGUI BestHighScoreText;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        BestScoreOfAll();
    }
    public void StartGame()
    {
        nameInputText = nameInput.text;
        SceneManager.LoadScene("main");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    [Serializable]
    public class PlayerData
    {
        public string playerName;
        public int playerHighScore;
    }

    public void SavePlayerData(int highScoreGameOver)
    {
        PlayerData data = new PlayerData();
        data.playerName = nameInputText;
        data.playerHighScore = highScoreGameOver;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + $"/{nameInputText}-savefile.json", json);

    }

    public int LoadPlayerData(string nameInputText2)
    {
        string path = Application.persistentDataPath + $"/{nameInputText2}-savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            nameInputText2 = data.playerName;
            HighestScore = data.playerHighScore;

            return HighestScore;
        }
        return 0;
    }

    public void BestScoreOfAll()
    {
        string path = Application.persistentDataPath;
        string[] jsonFiles = Directory.GetFiles(path, "*.json");

        int BestHighScore =0;
        string BestHighName = null;

        foreach (string filePath in jsonFiles)
        {
            string jsonText = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonText);
            
            if (data.playerHighScore > BestHighScore)
            {
                BestHighScore = data.playerHighScore;
                BestHighName = data.playerName;
            }
        }
        BestHighScoreText.text = $"Best Score of all Time : {BestHighName} : {BestHighScore}";

    }
}
