using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHeli : EnemyController
{
    // Update is called once per frame
    void Update()
    {
         EnemyMovement();
         Fire();
    }
}
