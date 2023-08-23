using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 20.0f;
    private float zBound = 25.0f;

    AudioSource audioSource;
    public AudioClip soundToPlay;
   
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (soundToPlay != null)
        {
            audioSource.pitch = Random.Range(1.0f, 2.0f);
            audioSource.PlayOneShot(soundToPlay);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // Move this object forward, destroying it if the object reaches out of bounds
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.z > zBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < -zBound)
        {
            Destroy(gameObject);
        }
       
    }
    
    
 
   
}
