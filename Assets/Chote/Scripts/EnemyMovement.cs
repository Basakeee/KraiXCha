using System.Threading.Tasks;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType
    {
        BetweenPoints,
        ScreenCollision
    }

    public MovementType movementType = MovementType.BetweenPoints;


    // For "BetweenPoints" movement
    public Vector3 pointA;
    public Vector3 pointB;

    // For both movement types
    public float speed = 2f;
    private Vector3 targetDirection;

    // For "ScreenCollision" movement
    private Vector3 velocity;
    bool isWaitToSwitch = false;

    void Start()
    {
        if (movementType == MovementType.BetweenPoints)
        {

            pointA = transform.position + pointA;
            pointB = transform.position + pointB;


            // Initialize the first target to point B
            targetDirection = pointA;

        }
        else if (movementType == MovementType.ScreenCollision)
        {
            // Initialize random horizontal velocity
            velocity = new Vector3(speed, 0, 0);
            velocity.x *= -1;
        }
    }

    void Update()
    {
        if (movementType == MovementType.BetweenPoints)
        {
            PatrolBetweenPoints();
        }
        else if (movementType == MovementType.ScreenCollision)
        {
            PatrolWithScreenCollision();
        }
    }

    void PatrolBetweenPoints()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetDirection, speed * Time.deltaTime);

        // If the enemy reaches the target point, switch target
        if (Vector3.Distance(transform.position, targetDirection) < 0.1f)
        {
            targetDirection = targetDirection == pointA ? pointB : pointA;
        }
    }

    async void PatrolWithScreenCollision()
    {
        transform.position += velocity * Time.deltaTime;


        // Reverse direction on screen edge collision
        if (transform.position.x <= Camera.main.ViewportToWorldPoint(new Vector3(SpawnManager.instance.minSpawnRange, 0, 0)).x ||
            transform.position.x >= Camera.main.ViewportToWorldPoint(new Vector3(SpawnManager.instance.maxSpawnRange, 0, 0)).x)
        {
            if (!isWaitToSwitch)
            {
                velocity.x *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                isWaitToSwitch = true;
            }

            await Task.Delay(1000);
            isWaitToSwitch = false;
        }

    }
}
