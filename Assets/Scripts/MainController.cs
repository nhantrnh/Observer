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
    private float currentHealth;


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
    // public AudioSource runAudio;
    // public AudioSource jumpAudio;
    // public AudioSource healAudio;
    // public AudioSource hurtAudio;

    private float speedBoostDuration = 3f; // Thời gian tăng tốc
    private float speedBoostTimer = 0f; // Bộ đếm thời gian của hiệu ứng tăng tốc
    private bool isCooldownActive = false;
    private bool isSpeedBoosted = false; // Kiểm tra xem nhân vật có đang được tăng tốc hay không

    void Start()
    {
         currentHp = healthPoint;
         currentHealth = speedBoostDuration; // Khởi tạo cooldown đầy đủ
         UICooldown.instance.Hide(); // Ẩn UI ban đầu
         UICooldown.instance.SetValue(currentHealth / speedBoostDuration); // Cập nhật UI ban đầu
    }

    void Awake(){

        controller = new MainCharISA();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerRenderer =  GetComponent<SpriteRenderer>();

        currentHp = healthPoint;
        animator.SetInteger("HP", currentHp);
           
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

        if (isSpeedBoosted)
        {
            speedBoostTimer -= Time.deltaTime;

            if (speedBoostTimer <= 0f)
            {
                RemoveSpeedBoost();
            }
        }

        if (currentHp == 0) {
            OnDisable();
            playerRenderer.enabled = true;
            isImmortal = false;
        }

        if (isCooldownActive)
        {
            // Giảm dần currentHealth theo thời gian
            UICooldown.instance.Show(); // Hiển thị thanh cooldown
            currentHealth = Mathf.Clamp(currentHealth - Time.deltaTime, 0, speedBoostDuration);
            UICooldown.instance.SetValue(currentHealth / speedBoostDuration);
            Debug.Log("cur: "+currentHealth);
            Debug.Log("speed: "+ currentHealth/speedBoostDuration);



            // Kiểm tra nếu cooldown đã hết
            if (currentHealth <= 0)
            {
                EndCooldown();
            }
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
    
        
        if (move != Vector3.zero) {
            transform.Translate(move, Space.World);
            // if (runAudio != null) {
            //     runAudio.Play();
            // }
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

    public void CollectionItem()
    {
        // Tăng tốc độ trong 3 giây
        ApplySpeedBoost();
    }

    // Tăng tốc độ khi nhặt giày
    void ApplySpeedBoost()
    {
        if (!isSpeedBoosted) // Kiểm tra xem có đang tăng tốc không
        {
            isSpeedBoosted = true;
            isCooldownActive = true;
            speedBoostTimer = speedBoostDuration;
            speed += 3f; // Tăng tốc độ lên 1 đơn vị
            Debug.Log("Speed Boosted!");
        }

    }

    // Loại bỏ hiệu ứng tăng tốc sau khi hết thời gian
    void RemoveSpeedBoost()
    {
        isSpeedBoosted = false;
        speed -= 1f; // Trở lại tốc độ ban đầu
        Debug.Log("Speed Boost Ended!");
    }

    public void StartCooldown()
    {
        isCooldownActive = true;
        currentHealth = speedBoostDuration; // Reset cooldown
        UICooldown.instance.Show(); // Hiển thị thanh cooldown
        UICooldown.instance.SetValue(1); // Đặt UI thành đầy đủ
    }

    private void EndCooldown()
    {
        isCooldownActive = false;
        UICooldown.instance.SetValue(0); // Đặt UI thành rỗng
       UICooldown.instance.Hide(); // Ẩn UI cooldown
       currentHealth = speedBoostDuration; // Reset cooldown
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
        
    }


    //--------------------------------------------------------------------
    //Private ------------------------------------------------------------
    private void Jump()
    {
        isGrounded = false;
        animator.SetBool("isJumping", !isGrounded);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // if (jumpAudio != null) {
        //     jumpAudio.Play();
        // }
        
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

            // PlayAudio(playerHit);
        }
        if (value > 0) {
            //Already equal max
            if (currentHp == healthPoint) return; 
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
