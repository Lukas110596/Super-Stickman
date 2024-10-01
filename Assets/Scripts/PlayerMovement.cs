using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private Vector2 velocity;
    private float inputAxis;
    private CapsuleCollider2D capsuleCollider;
  
    // variables
    public float movementSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => 2f * maxJumpHeight / (maxJumpTime / 2f);
    public float gravity => -2f * maxJumpHeight / Mathf.Pow(maxJumpTime / 2f,2);
    // boolean with public getter but private setter
    public bool grounded { get; private set;}
    public bool jumping { get; private set;}
    public bool running  => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);
    public bool ducking { get; private set; } = false;

    [SerializeField] private AudioSource jumpingSound;
   
    // unity awake method -> get player rigidbody and set camera
    private void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        camera = Camera.main;

    }

    // unity update method to update rigidbody based on movement
    private void Update() 
    {
        HorizontalMovement();

        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded) {
            GroundedMovement();
        }


        ApplyGravity();
    }

    // horizontal movement (x-axis, left or right), ducking logic
    private void HorizontalMovement()  
    {
        if (Input.GetButton("Vertical")) {
           ducking = true;
           // movementSpeed = 4f;
           velocity.x = Mathf.MoveTowards(velocity.x, 0f , movementSpeed*Time.deltaTime);
        } else {
            // slowly set velocity to 1 while sliding -> makes sliding time shorter and gameplay smoother and easier to control the character
            if (sliding) {
                velocity.x = Mathf.MoveTowards(velocity.x, 1f, movementSpeed*Time.deltaTime/2);            
            }

            ducking = false;
            // movementSpeed = 8f;
        
            inputAxis = Input.GetAxis("Horizontal");
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * movementSpeed, movementSpeed * Time.deltaTime);

            // set velocity to 0 when running against wall
            if (rigidbody.Raycast(Vector2.right * velocity.x)) {
                // jumping = false;
                velocity.x = 0f;
            }

            if (velocity.x > 0f) {
                transform.eulerAngles = Vector3.zero;
            } else if (velocity.x < 0f) {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
    }

    // movement when you are on the ground, jumping logic
    private void GroundedMovement() 
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumpingSound.Play();
            jumping = true;
        } 

        jumping = false;
    }

    // gravity for player (for example when jumping and then falling down)
    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    // set screenborders (can't run out of screen), set fixed position of rigidbody
    private void FixedUpdate() 
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x+ 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    // collision logic with power ups (power ups later developed ...) and bumping head on blocks
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp")) 
        {
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y=0f;
            }
        }

        jumping = false;
    }
}
