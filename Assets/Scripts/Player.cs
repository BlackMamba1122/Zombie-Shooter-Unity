using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public int HP=100;
    public GameObject bloodyScreen;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI end;
    public bool isdead;

    private void Start() {
        healthText.text = $"Health: {HP}";
    }
    public void TakeDAmage(int damage)
    {
        HP-=damage;
        if(HP<=0)
        {
            print("killed");
            PlayerDead();
        }
        else
        {
            healthText.text = $"Health: {HP}";
            StartCoroutine(BloodScreenEffect());
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerhurt);
        }
    }

    private void PlayerDead()
    {
        isdead = true;
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.palyerdie);
        SoundManager.Instance.playerChannel.clip=SoundManager.Instance.gameover;
        SoundManager.Instance.playerChannel.PlayDelayed(2f);

        GetComponent<RigidbodyFirstPersonController>().enabled=false;
        GetComponentInChildren<Animator>().enabled=true;
        healthText.gameObject.SetActive(false);
        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOver());

    }

    private IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1f);
        end.gameObject.SetActive(true);
        int waveSurvived = GlobalRefrences.Instance.waveNumber;
        if (waveSurvived-1>SaveLoadManager.Instance.load())
        {
            SaveLoadManager.Instance.save(waveSurvived-1);        
        }
        StartCoroutine(ReturntoAminMenu());
    }

    private IEnumerator ReturntoAminMenu()
    {
        yield return new WaitForSeconds(6f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator BloodScreenEffect()
    {
       if(bloodyScreen.activeInHierarchy == false)
       {
            bloodyScreen.SetActive(true);
       }
        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible)
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration= 1.5f;
        float elapsedTime = 0f;

        // Fade out over the specified duration
        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }
       if(bloodyScreen.activeInHierarchy == true)
       {
        bloodyScreen.SetActive(false);
       }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("ZombieHand"))
        {
            if(isdead == false && other.gameObject.GetComponentInParent<Zombie>().IsDead == false)
            {
                TakeDAmage(25);
            }
        }
    }
}
