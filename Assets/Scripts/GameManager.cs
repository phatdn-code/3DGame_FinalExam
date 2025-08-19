using System.Collections;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent OnGameWin;

    [Header("Game Settings")]
    [SerializeField] private TextMeshProUGUI enemyKillText;
    [SerializeField] private TextMeshProUGUI enemySpawnedText;
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private int enemyKillTarget = 100;
    private int enemyKill = 0;
    private int enemySpawned = 0;

    private void Awake()
    {
        Instance = this;

        enemyKillText.text = $"{enemyKill} / {enemyKillTarget}";
    }

    public void EnemyKilled()
    {
        enemyKill++;
        enemyKillText.text = $"{enemyKill} / {enemyKillTarget}";

        EnemySpawned(-1);

        if (enemyKill >= enemyKillTarget)
            GameWin();
    }

    public void EnemySpawned(int count)
    {
        enemySpawned += count;
        enemySpawnedText.text = $"{enemySpawned}";
    }

    private void GameWin()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.StopSpawn();
        }
        OnGameWin?.Invoke();
        StartCoroutine(QuitRoutine());
    }

    private IEnumerator QuitRoutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
}
