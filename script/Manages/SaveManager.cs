using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName ="";
    public string SceneName
    {
        get
        {
            return PlayerPrefs.GetString(sceneName);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneControl.Instance.TransitionToMainScene();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStat.characterData, GameManager.Instance.playerStat.characterData.name);
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStat.characterData, GameManager.Instance.playerStat.characterData.name);
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void Save(object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }

    }
}
