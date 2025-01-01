using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private MainController mainChar;
    public float moveSpeed;
    public float leftBoundary = -24f;
    public float lookAheadDistance = 5f;
    public float lookAheadSpeed = 3f;
    private float lookAheadOffset;
    private Vector3 targetPoint = Vector3.zero;

    //Paralex camera:
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        mainChar = player.GetComponent<MainController>();
        targetPoint = new Vector3(mainChar.transform.position.x, mainChar.transform.position.y, -1);
        oldPosition = transform.position.x;
    }

    void LateUpdate(){
        
        if (mainChar.rb.linearVelocityY >= 0 && mainChar.GetIsGrouded()) {
            targetPoint.y = mainChar.transform.position.y;
        }
        

        if (targetPoint.y < 0) {
            targetPoint.y = 0;
        }

        if (mainChar.rb.linearVelocityX > 0f) {
            lookAheadOffset = Mathf.Lerp(lookAheadOffset, lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }
        if (mainChar.rb.linearVelocityX < 0f) {
            lookAheadOffset = Mathf.Lerp(lookAheadOffset, -lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }

        targetPoint.x = player.transform.position.x + lookAheadOffset;
        if (targetPoint.x <= leftBoundary) {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
