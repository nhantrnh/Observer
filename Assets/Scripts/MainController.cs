using UnityEngine;
using main_isa;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public Rigidbody2D rb;
    public bool isLookingLeft = false;
    public bool isGrounded = true;
    public float regconizedSpeed;



    private Vector2 moveInput;
    private Vector3 move;
    private MainCharISA controller;
    private Animator animator;
    private SpriteRenderer playerRenderer;
 

    //Damaging setup
    public float immortalDuration = 2f;
    private float timer;
    private bool isImmortal = false;
    private int currentHp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Heal
    public int healthPoint = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite lostHeart;

    // //SFX:
    private AudioSource mainSfx;
    public AudioClip runClip, jumpClip, healClip, hurtClip;

    //For footstep
    public float stepRate = 0.3f;
	public float stepCoolDown;


    void Start()
    {
         currentHp = healthPoint;
    }

    void Awake(){

        controller = new MainCharISA();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerRenderer =  GetComponent<SpriteRenderer>();

        currentHp = healthPoint;
        animator.SetInteger("HP", currentHp);
        
        mainSfx = GetComponent<AudioSource>();

           
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Image image in hearts) {
            image.sprite = lostHeart;
        }
        for (int i = 0; i < currentHp; i++) {
            hearts[i].sprite = fullHeart;
        }

        if (currentHp == 0) {
            OnDisable();
            playerRenderer.enabled = true;
            isImmortal = false;
        }

        move = new Vector3(moveInput.x, 0, 0)*speed*Time.deltaTime;
        regconizedSpeed = Math.Abs(move.magnitude *speed);
        animator.SetFloat("Speed", regconizedSpeed);
        animator.SetFloat("xVel", Math.Abs(rb.linearVelocityX));

        if (move.x > 0) {
            isLookingLeft = true;
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
        }

        if (move.x < 0) {
            isLookingLeft = false;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
    
        
        //Run audio
        if (move != Vector3.zero) {
            transform.Translate(move, Space.World);
            //footstep calculate
            stepCoolDown -= Time.deltaTime;
            if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f && isGrounded){
                mainSfx.pitch = 1f + UnityEngine.Random.Range (-0.2f, 0.2f);
                mainSfx.PlayOneShot (runClip, 3.0f);
                stepCoolDown = stepRate;
		    }
        }

        //If immortal
        if (isImmortal) {
            playerRenderer.enabled = !playerRenderer.enabled;
            timer -= Time.deltaTime;
            if (timer < 0) {
                isImmortal = false;
                playerRenderer.enabled =true;
            }
        }
    }

    void FixedUpdate(){
     
        
    }

    void OnEnable()
    {

        // Đăng ký các hành động từ Action Map "Player"
        controller.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();

        controller.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controller.Player.Jump.performed += ctx => {
            if (isGrounded && rb.linearVelocityY >=0) {
                Jump();
            }
        };

        controller.Enable(); // Bật hệ thống input
    }

    void OnDisable()
    {
        controller.Disable(); // Tắt input khi không sử dụng
    }



    void OnCollisionEnter2D(Collision2D collision) {
        //Portal
        if (collision.gameObject.CompareTag("Portal 1")) {
            SceneManager.LoadScene(2);
            return;
        }
        if (collision.gameObject.CompareTag("Portal 2")) {
            SceneManager.LoadScene(3);
            return;
            
        }
        if (collision.gameObject.CompareTag("Portal 3")) {
            SceneManager.LoadScene(4);
            return;
        }
        if (collision.gameObject.CompareTag("Portal")){
            SceneManager.LoadScene(1);
            return;
        }

        //Papers
        if (collision.gameObject.CompareTag("Paper")){
            PaperShake paper = collision.gameObject.GetComponent<PaperShake>();
            Debug.Log("Paper:" + paper.GetPaperContent());
            Destroy(collision.gameObject);
        }

        //Death Border
        if (collision.gameObject.CompareTag("Border")){
            Debug.Log("Death");
            //Show "You died" UI here.
            
        }
        
    }


    //--------------------------------------------------------------------
    //Private ------------------------------------------------------------
    private void Jump()
    {
        isGrounded = false;
        animator.SetBool("isJumping", !isGrounded);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (jumpClip != null) {
            mainSfx.PlayOneShot(jumpClip);
        }
        
    }

    //Public -------------------------------------------------------------

    public void SetIsGrounded(bool value) {
        isGrounded = value;
        animator.SetBool("isJumping", !isGrounded);
    }

    public bool GetIsGrouded() {
        return this.isGrounded;
    }

    public void ChangeHealthPoint(int value){
        if (value < 0) {
            if (isImmortal) return;

            // //Set hit.
            // animator.SetBool("Hit", true);

            isImmortal = true;
            timer = immortalDuration;

            if (hurtClip != null){
                mainSfx.PlayOneShot(hurtClip);
            }
        }
        if (value > 0) {
            //Already equal max
            if (currentHp == healthPoint) return; 
                if (healClip != null){
                mainSfx.PlayOneShot(healClip);
            }
        }
      
        //Calculate
        currentHp = Math.Clamp(currentHp + value, 0, healthPoint);

        //Set hp
        animator.SetInteger("HP", currentHp);

        // HPBarController.instance.SetSize(currentHp*1.0f/hp);
        Debug.Log(animator.GetInteger("HP"));
    }
    public int HealthPoint{
        get {
            return currentHp;
        }
        set {
            ChangeHealthPoint(value);
        }
    }
    
   
}
