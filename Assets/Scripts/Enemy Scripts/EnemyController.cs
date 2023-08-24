using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE; parent class EnemyController has 3 child classes: HeavyHeli, NormalHeli, and LightHeli.
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
    protected Vector3 initialPos;

    public bool isShooting;
    protected bool introDone;
    
    // Start is called before the first frame update
    void Start()
    {
        introDone = false;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // POLYMORPHISM
    // Moves enemy down the screen
    public virtual void EnemyMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * enemySpeed);
        OutofBounds();
    }

    // POLYMORPHISM
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

    // ABSTRACTION
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

    public IEnumerator EnemyIntro()
    {
        Vector3 startPos = initialPos;
        Vector3 endPos = initialPos - new Vector3(0, 0, Random.Range(8,14));

        float lerpSpeed = 5.0f;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;

        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
            yield return null;
        }
        introDone = true;

    }

    // ABSTRACTION
    protected IEnumerator SidetoSideMovement(Vector3 pointA, float speed, float xRange)
    {
        var pointB = pointA - new Vector3(xRange, 0, 0);
        
        while (true)
        {
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, speed));
            yield return StartCoroutine(MoveObject(transform, pointB, pointA, speed));
        }

    }
    protected IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    // Add instance of enemy to list of entities (for MissileBehavior script)
    private void Awake()
    {
        initialPos = transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Entities.Add(this);
    }

    private void OnDestroy()
    {
        Entities.Remove(this);
    }
}
