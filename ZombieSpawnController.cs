using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiePerWave = 2;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldowntime = 10f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<Zombie> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI waveoverUI;
    public TextMeshProUGUI cooldownUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        currentZombiesPerWave = initialZombiePerWave;
        cooldownUI.gameObject.SetActive(false);
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;

        currentWaveUI.text = "Wave: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-20, 20), 0f, Random.Range(-20, 20));
            Vector3 spawnPosition = transform.position + spawnOffset;

            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Zombie zombieScript = zombie.GetComponent<Zombie>();

            currentZombiesAlive.Add(zombieScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Zombie> zombiesToRemove = new List<Zombie>();
        foreach (Zombie zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        foreach (Zombie zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        if (currentZombiesAlive.Count == 0 && !inCooldown)
        {
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
            cooldownCounter = Mathf.Max(cooldownCounter, 0);
            cooldownUI.text = $"{cooldownCounter:F0}"; 
            cooldownUI.gameObject.SetActive(true); 
        }
        else
        {
            cooldownUI.gameObject.SetActive(false); 
        }
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        waveoverUI.gameObject.SetActive(true);

        cooldownCounter = waveCooldowntime;
        while (cooldownCounter > 0)
        {
            yield return null;
        }

        inCooldown = false;
        waveoverUI.gameObject.SetActive(false);

        currentZombiesPerWave *= 2;
        StartNextWave();
    }
}
