using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;
    

    public static WaveManager Instance;

    bool waveRunning = true;
    int currentWave = 0;
    int currentWaveTime = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        
        StartNewWave();
        timeText.text = "30";
        waveText.text = "Wave: 1";
        
    }

    private void Update()
    {
        //for testing
        if (currentWaveTime == 0)
        if (Input.GetKeyUp(KeyCode.E))
            StartNewWave();
    }

    public bool WaveRunning() => waveRunning;

    private void StartNewWave()
    {
        StopAllCoroutines();
        timeText.color = Color.black;
        currentWave++;
        waveRunning = true;
        currentWaveTime = 30;
        waveText.text = "Wave: " + currentWave;
        StartCoroutine(WaveTimer());
    }

    IEnumerator WaveTimer()
    {
        while (waveRunning)
        {
            yield return new WaitForSeconds(1f);
            currentWaveTime--;

            timeText.text = currentWaveTime.ToString();

            if (currentWaveTime <= 0)
                WaveComplete();
        }

        yield return null;
    }
    void WaveComplete()
    {
        StopAllCoroutines();
        EnemyManager.Instance.DestroyAllEnemies();
        waveRunning = false;
        currentWaveTime = 0;
        timeText.text = currentWaveTime.ToString();
        timeText.color = Color.red;
    }
   

     
}
    
    