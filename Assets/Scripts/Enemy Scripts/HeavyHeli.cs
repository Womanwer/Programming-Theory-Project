using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyHeli : EnemyController
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
        if(introDone)
        {
            EnemyMovement();
            Fire();
        }
                
    }

    public override void EnemyMovement()
    {
        StartCoroutine(SidetoSideMovement());
    }

    IEnumerator SidetoSideMovement()
    {
        Vector3 pointB = transform.position - new Vector3(10,0,0);
        var pointA = transform.position;
        while (true)
        {
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, 2.0f));
            yield return StartCoroutine(MoveObject(transform, pointB, pointA, 2.0f));
        }

    }
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
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

}
