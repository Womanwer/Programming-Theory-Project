using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHeli : EnemyController
{
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
 }
