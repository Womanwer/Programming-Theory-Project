using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    public float cameraPosY;
    private float cameraSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Moves camera relative to player position
        if (player != null)
        {
            float cameraPosX = player.transform.position.x / cameraSpeed;
            float cameraPosZ = player.transform.position.z / cameraSpeed;

            transform.position = new Vector3(cameraPosX, cameraPosY, cameraPosZ);
        }
    }
}
