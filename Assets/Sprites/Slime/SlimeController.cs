using UnityEngine;

public class SlimeControl : MonoBehaviour
{
    public float moveSpeed = 3f;  // Tốc độ di chuyển
    public float moveDistance = 5f;  // Khoảng cách tối đa di chuyển

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;
    
    //Spin:
    private bool isSpinning = false;
    public float spinDuration = 1f;
    private float spinTimer;
    private float noSpinDuration = 5f;
    
    private Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Animator:
        animator = gameObject.GetComponent<Animator>();

        // Lưu vị trí ban đầu của bot
        startPosition = transform.position;
        targetPosition = new Vector3(startPosition.x + moveDistance, startPosition.y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Spin calculate
        spinTimer -= Time.deltaTime;
        if (spinTimer < 0) {
            isSpinning = !isSpinning;
            animator.SetBool("Spin", !isSpinning);
            //Reverse meaning:
            if (isSpinning) {
                spinTimer = noSpinDuration;
            } else {
                spinTimer = spinDuration;
            }
        }

        // Di chuyển bot qua lại
        if (movingRight)
        {   
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position.x >= targetPosition.x)
                movingRight = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
         
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
            if (transform.position.x <= startPosition.x)
                movingRight = true;
        }
         
        
    }

    private void OnTriggerStay2D(Collider2D collider){

        MainController mainController = collider.GetComponent<MainController>();
        if (mainController != null){
            if (mainController.HealthPoint > 0) {
                mainController.ChangeHealthPoint(-1);
            }
        }
    }

    public bool GetIsSpinning(){
        return isSpinning;
    }
}
