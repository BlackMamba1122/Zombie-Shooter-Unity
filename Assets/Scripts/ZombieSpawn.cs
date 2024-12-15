using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int initialzombieperWave=3;
    public int currentzombieperWave;
    public float spawnDelay=0.5f;
    public int currentWave=0;
    public float waveCooldown=10.0f;
    public bool isCooldown;
    public float cooldownCounter=0;
    public List<Zombie> currentZombieAlive;
    public GameObject zombiePrefab;
    public TextMeshProUGUI waveoverUI;
    public TextMeshProUGUI CooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start() {
        currentzombieperWave = initialzombieperWave;
                GlobalRefrences.Instance.waveNumber=0;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombieAlive.Clear();
        currentWave++;
        GlobalRefrences.Instance.waveNumber=currentWave;
        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for(int i=0;i<currentzombieperWave;i++)
        {
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1f,1f),0,UnityEngine.Random.Range(-1f,1f));
            Vector3 spawnPosition = transform.position + spawnOffset;
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Zombie zombieScript = zombie.GetComponent<Zombie>();
            currentZombieAlive.Add(zombieScript);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    private void Update() {
        List<Zombie> zombietoremove = new List<Zombie>();
        foreach(Zombie zombie in currentZombieAlive)
        {
            if(zombie.IsDead)
            {
                zombietoremove.Add(zombie);
            }
        }
        foreach(Zombie z in zombietoremove)
        {
            currentZombieAlive.Remove(z);
        }
        zombietoremove.Clear();
        if(currentZombieAlive.Count == 0 && isCooldown == false)
        {
            StartCoroutine(waveCoolDown());
        }
        if(isCooldown)
        {
            cooldownCounter -=Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }
        CooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator waveCoolDown()
    {
        isCooldown =true;
        if(currentWave != 0 && currentWave%2==0)
        {
            WeaponManager.Instance.AddAmmo("pistol",20);
        }
        if(currentWave != 0 && currentWave%4==0)
        {
            WeaponManager.Instance.AddAmmo("ak",40);
        }
        waveoverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCooldown);
        waveoverUI.gameObject.SetActive(false);
        isCooldown=false;
        currentzombieperWave*=2;
        StartNextWave();
    }
}
