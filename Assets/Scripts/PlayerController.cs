using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 15.0f;
    private float xBound = 20.0f;
    private float zBound = 12.0f;

    private GameManager gameManager;
      
    AudioSource playerAudio;
    public AudioClip powerUpSound;

    private float timer;
    public float fireRate = 0.5f;
    public bool isShooting;
    public GameObject[] projectilePrefabs;
    public int weaponType;

    public bool hasPowerUp = false;
    public PowerUpType currentPowerUp = PowerUpType.None;
    private Coroutine powerUpCountdown;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // ABSTRACTION
        MovePlayer();
        ConstrainPlayerPosition();
        FireProjectile();

        if (currentPowerUp == PowerUpType.Rotor)
        {
            playerSpeed = 20.0f;      
        }
        if (currentPowerUp == PowerUpType.Missiles)
        {
            weaponType = 1;
        }  
    }

    // Fire a projectile using the space key
    private void FireProjectile()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            isShooting = true;
            
        }
        else
        {
            isShooting = false;
            
        }
        if (isShooting && timer >= fireRate)
        {
            Instantiate(projectilePrefabs[weaponType], transform.position, transform.rotation);
            
            timer = 0;
        }
    }
    
    // Moves player based on horizontal and vertical inputs
    private void MovePlayer()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * playerSpeed);
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerSpeed);
    }

    // Constrains player movement within set boundaries
    private void ConstrainPlayerPosition()
    {
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }

        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
        else if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
    }

    // Detects collisions and call methods depending on collision object
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            Debug.Log("Player Hit by " + collision.gameObject.name);
            gameManager.UpdateHealth();
            Destroy(collision.gameObject);
        }
        
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            currentPowerUp = collision.gameObject.GetComponent<PowerUp>().powerUpType;
            powerUpCountdown = StartCoroutine(PowerUpCountdownCoroutine());

            playerAudio.PlayOneShot(powerUpSound);
            gameManager.UpdatePowerUp(currentPowerUp);
            Destroy(collision.gameObject);
            Debug.Log("Picked up PowerUp " + currentPowerUp);
            if (powerUpCountdown != null)
            {
                StopCoroutine(PowerUpCountdownCoroutine());
            }
            
            StartCoroutine(PowerUpCountdownCoroutine());
        }
    }

    //Starts powerup cooldown timer
    IEnumerator PowerUpCountdownCoroutine()
    {
        yield return new WaitForSeconds(5);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        gameManager.UpdatePowerUp(currentPowerUp);
        weaponType = 0;
        playerSpeed = 15.0f;
    }
}
