using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHeli : EnemyController
{
    private float heliSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(EnemyIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if (introDone)
        {
            
            EnemyMovement();
            Fire();
            
        }
    }

    // POLYMORPHISM
    public override void EnemyMovement()
    {
        
        if (gameManager.isGameActive)
        {
            Transform target = FindObjectOfType<PlayerController>().transform;
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, heliSpeed * Time.deltaTime);
        }
        
    }
}
