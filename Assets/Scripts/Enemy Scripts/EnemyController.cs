using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static readonly HashSet<EnemyController> Entities = new HashSet<EnemyController>();

    protected GameManager gameManager;

    public GameObject enemyProjectile;
    public ParticleSystem explosionFX;
    public AudioClip explosionSound;

    [SerializeField] int pointValue;
    [SerializeField] float enemySpeed;
    [SerializeField] float enemyFireRate;
    [SerializeField] float fireDelay;
    private float zBound = 30.0f;
    private float timer;

    public bool isShooting;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        Fire();
    }

    // Moves enemy down the screen
    public virtual void EnemyMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * enemySpeed);
        OutofBounds();
    }

    // Enemy fires projectiles at a specific firerate
    public virtual void Fire()
    {
        timer += Time.deltaTime;
        if (timer >= enemyFireRate)
        {
            
            Instantiate(enemyProjectile, transform.position, transform.rotation);
            timer = 0.0f;
        }
    }

    // Destroy enemy if colliding with player projectile
    private void OnCollisionEnter(Collision collision)
    {
        DetectCollision(collision);
    }

    public void DetectCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            gameManager.UpdateScore(pointValue);
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    //Destroy gameobject if it leaves boundaries
    public void OutofBounds()
    {
        if (transform.position.z > zBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < -zBound)
        {
            Destroy(gameObject);
        }
    }

    // Add instance of enemy to list of entities (for MissileBehavior script)
    private void Awake()
    {
        Entities.Add(this);
    }

    private void OnDestroy()
    {
        Entities.Remove(this);
    }
}
