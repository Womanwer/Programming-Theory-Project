using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip soundToPlay;

    private float speed = 15.0f;
    private float aliveTimer = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(soundToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
        Destroy(gameObject, aliveTimer);
    }

    // Find and track the closest enemy
    public void FindClosestEnemy()
    {
        Vector3 pos = transform.position;
        float distance = float.PositiveInfinity;
        EnemyController targ = null;

        foreach (var enemy in EnemyController.Entities)
        {
            var d = (pos - enemy.transform.position).sqrMagnitude;
            if (d < distance)
            {
                targ = enemy;
                distance = d;
            }
        }

        if (targ != null)
        {
            Vector3 moveDirection = (targ.transform.position - transform.position).normalized;

            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(targ.transform.position);
        } else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    // Destroy missile and enemy when colliding
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
