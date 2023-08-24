using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyHeli : EnemyController
{
    private float heliSpeed = 2.0f;
    private float xRange = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if(introDone)
        {
            EnemyMovement();
            Fire();
        }
    }

    // POLYMORPHISM
    public override void EnemyMovement()
    {
        StartCoroutine(SidetoSideMovement(transform.position, heliSpeed, xRange));
    }
}
