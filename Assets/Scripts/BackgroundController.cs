using System.Numerics;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate(){
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1-parallaxEffect);
        transform.position = new UnityEngine.Vector3(startPos + distance, transform.position.y, transform.position.z);
    

        if (movement > startPos + length) {
            startPos += length;
        }
        else if (movement < startPos + length){
            startPos -=length;
        }
    }
}
