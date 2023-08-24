using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI powerUpText;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject pauseScreen;

    public bool isGameActive;
    private bool isGamePaused;

    // ENCAPSULATION
    private int m_playerHealth;
    public int playerHealth
    {
        get { return m_playerHealth; }
        set
        {
            if (value < 0 )
            {
                Debug.LogError("Player health can not be below 0");
            }
            else
            {
                m_playerHealth = value;
            }
        }
    }

    public int score;
    public int prevBestScore;

    public GameObject playerToSpawn;
    private GameObject player;
    public GameObject[] enemyPrefabs;
    public GameObject[] powerUpPrefabs;
    public GameObject[] clouds;

    public ParticleSystem explosionFX;
    public AudioClip explosionSound;

    private float spawnRate = 1.0f;
    private float powerSpawnDelay = 7.0f;
    private float xRange = 10.0f;
    private float spawnPosZ = 13.0f;
    private Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        prevBestScore = PlayerDataManager.Instance.bestScore;
        StartCoroutine(SpawnClouds());
    }

    // Update is called once per frame
    void Update()
    {
        // ABSTRACTION
        if (Input.GetKeyDown(KeyCode.E) && !isGameActive)
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isGameActive)
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isGamePaused = !isGamePaused;
            Time.timeScale = isGamePaused ? 0 : 1;
            pauseScreen.gameObject.SetActive(isGamePaused);
        }
    }

    // Spawn a random enemy at a random position above the screen
    private IEnumerator SpawnEnemy()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            spawnPos = new Vector3(GenerateSpawnPosX(), 1.0f, spawnPosZ);
            int enemyToSpawn = Random.Range(0, enemyPrefabs.Length);

            Instantiate(enemyPrefabs[enemyToSpawn], spawnPos, enemyPrefabs[enemyToSpawn].transform.rotation);
        }

    }

    // Spawn a random powerup at a random position above the screen
    private IEnumerator SpawnPowerUp()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(powerSpawnDelay);
            int i = Random.Range(0, powerUpPrefabs.Length);
            spawnPos = new Vector3(GenerateSpawnPosX(), 1.0f, spawnPosZ);

            Instantiate(powerUpPrefabs[i], spawnPos, powerUpPrefabs[i].transform.rotation);
        }

    }

    private IEnumerator SpawnClouds()
    {
        for (; ;)
        {
            yield return new WaitForSeconds(3.0f);
            int cloudInt = Random.Range(0, clouds.Length);
            spawnPos = GenerateCloudSpawnPos();

            Instantiate(clouds[cloudInt], spawnPos, clouds[cloudInt].transform.rotation);
        }
       
    }

    // Generate a random spawn position across the x-axis
    private float GenerateSpawnPosX()
    {
        float spawnPosX = Random.Range(-xRange, xRange);
        return spawnPosX;
    }

    // Generate random spawn position specifically for clouds
    private Vector3 GenerateCloudSpawnPos()
    {
        Vector3 cloudPos = new Vector3(Random.Range(-20, 20), -1.0f, 25);
        return cloudPos;
    }

    private void GameOver()
    {
        SaveBestScore();
        isGameActive = false;
        gameOverScreen.SetActive(true);
        
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealth()
    {
        m_playerHealth--;
        healthText.text = "HP: " + playerHealth;
        if (playerHealth == 0)
        {
            Instantiate(explosionFX, player.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            Destroy(player);
            GameOver();
        }
    }

    public void StartGame()
    {
        player = GameObject.Find("Player");
        isGameActive = true;
        m_playerHealth = 5;
        healthText.text = "HP: " + playerHealth;
        powerUpText.text = "Power Up: None";
        UpdateScore(0);
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
        titleScreen.SetActive(false);
    }

    public void UpdatePowerUp(PowerUpType currentPowerUp)
    {
        powerUpText.text = "Power Up: " + currentPowerUp; 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveBestScore()
    {
        if(score > prevBestScore)
        {
            PlayerDataManager.Instance.bestScore = score;
            PlayerDataManager.Instance.SaveHighScore();
        }
    }
}
