using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject mainChar;
    private Transform mcTransform;
    private MainController mcController;
    private Vector3 offset;
    private Animator animator;
    public float distance;
    private float followSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mcTransform = mainChar.GetComponent<Transform>();
        mcController = mainChar.GetComponent<MainController>();
        animator = GetComponent<Animator>();
        followSpeed =mcController.speed;
    }

    // Update is called once per frame
    void Update()
    {
         if (mainChar != null)
        {
            if (mcController.isLookingLeft) {
                offset = new Vector3(-distance, distance,0);
            } else {
                offset = new Vector3( distance, distance, 0);
            }
            Vector3 targetPosition = mcTransform.position + offset;
            Vector3 move = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.position = move;
            animator.SetFloat("Speed", mcController.regconizedSpeed);
        }
    }
}
