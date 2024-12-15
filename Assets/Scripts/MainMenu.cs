using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highscore;
    string newGameScene="SampleScene";
    public AudioClip bg_sound;
    public AudioSource mainChannel;
    // Start is called before the first frame update
    void Start()
    {
        mainChannel.PlayOneShot(bg_sound);
        int highscoree = SaveLoadManager.Instance.load();
        highscore.text=$"Top Wave Survived: {highscoree}";
    }
    public void StartnewGame()
    {
        mainChannel.Stop();
        SceneManager.LoadScene(newGameScene);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
