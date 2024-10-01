using UnityEngine;

public class EntityMovement : MonoBehaviour {


    // is public so we can change in in Unity
    public float speed = 1f;
     // is public so we can change in in Unity
    public Vector2 direction = Vector2.left;
    private  Rigidbody2D m_rigidbody;
    private Vector2 velocity;

    private void Awake() {
        m_rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    // Entitys move at the moment their are visible
    private void OnBecameVisible() {
        enabled = true;
    }
    // Entitys dont move at the moment their are invisible
    private void OnBecameInvisible() {
        enabled = false;
    }
    // Entitiy Wakes up and move 
    private void OnEnable() {
        m_rigidbody.WakeUp();
    }

    private void OnDisable() {
        //if you jump on an Entity it stopes and dies
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.Sleep();
    }

    // Physics of Entitiy 
    private void FixedUpdate() {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        m_rigidbody.MovePosition(m_rigidbody.position + velocity * Time.fixedDeltaTime);

        //flip the direction at collission using Raycast from Extension
        if(m_rigidbody.Raycast(direction)) {
            direction = -direction;
            if (velocity.x > 0f) {
            transform.eulerAngles = Vector3.zero;
            } else if (velocity.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        }

        //setting the Y-Velocity to max 0 so the gravity dont add up all the time and ends at zero
        if (m_rigidbody.Raycast(Vector2.down)) {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }


    }

    


}