using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }
    string highScoreKey="BestWavedSavedValue";
        private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public void save(int score)
    {
        PlayerPrefs.SetInt(highScoreKey,score);
    }
    public int load()
    {
        if(PlayerPrefs.HasKey(highScoreKey))
        {
            return PlayerPrefs.GetInt(highScoreKey);
        }
        return 0;
    }
}
