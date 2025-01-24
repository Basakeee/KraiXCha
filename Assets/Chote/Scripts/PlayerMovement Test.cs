using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction;
    public float speed = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed,Input.GetAxisRaw("Vertical")* speed);
    }
}
