using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatLength;
    private float backgroundSpeed = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        repeatLength = transform.lossyScale.z * 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * backgroundSpeed);
        if (transform.position.z < startPos.z - repeatLength)
        {
            transform.position = startPos;
        }
    }
}
