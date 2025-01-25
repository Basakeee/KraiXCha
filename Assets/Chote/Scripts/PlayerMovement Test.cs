using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    public float playerSpeed;
    public int maxHP;
    public int curHP;

    [Header("Bubble Settings")]
    public int bubbleCharge;
    public int maxBubbleCharge;
    public bool bubbleReady;

    [Header("Raycast Settings")]
    public float checkRange;
    public Vector2 OFFSET;
    public bool canCharge;
    public LayerMask mask;

    [Header("Additional Setting")]

    public int HurtAmount;
    public int RegenAmount;

    public bool isHurtCD;
    public bool isRegenCD;

    public float hurtCooldown;
    public float regenCooldown;

    [Header("Gravity Scale")]
    public float ascendGravity;
    public float descendGravity;

    public float timeToChange;
    public float maxGravitySpeed;

    [Header("Player Score")]
    public Transform startPos;
    public int curDepth;
    public int maxDepth;

    [Header("CheckHit")]
    public float radius;
    public LayerMask hitMask;
    public bool canHit;

    [Header("Bubble Sprite")]
    public SpriteRenderer bubbleRenderer;

    private float ratio;
    private Rigidbody2D rb;
    private Vector3 offset => OFFSET;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHP = maxHP;
        playerSpeed = 8;
        maxBubbleCharge = 5;
        checkRange = 1.25f;
        HurtAmount = 1;
        RegenAmount = 1;
        ascendGravity = -0.5f;
        timeToChange = 2;
        maxGravitySpeed = 2;
        radius = 0.4f;
        canHit = true;
        bubbleRenderer = transform.Find("Bubble").GetComponent<SpriteRenderer>();
    }

    private async void Update()
    {
        BubbleCharge();
        await HurtandRegen();
        HandleDeath();
        PlayerDepthHandle();
        GravityHandle();
        await OnTakeDMG();
        BubbleShow();
    }

    private void BubbleShow()
    {
        if (bubbleReady)
        {
            bubbleRenderer.enabled = true;
        }
        else if (!bubbleReady)
        {
            bubbleRenderer.enabled = false;
        }
    }

    private async Task OnTakeDMG()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, hitMask) && canHit)
        {
            if (bubbleReady)
            {
                bubbleReady = false;
                bubbleCharge = 0;
                canHit = false;
                rb.linearVelocityY = 0;
                rb.gravityScale = descendGravity;
                await Task.Delay(1500);
                canHit = true;
                return;
            }
            curHP = 0;
            bubbleCharge = 0;
        }
    }

    private void GravityHandle()
    {
        // Calculate gravity resistance based on depth
        ratio = curDepth / 10000f; // Ratio of current depth to max reference depth
        descendGravity = Mathf.Lerp(0.5f, maxGravitySpeed, ratio); // Increase gravity with depth

        // Ascend gravity should remain consistent for smoother upward movement
        if (bubbleReady)
        {
            rb.gravityScale = Mathf.Lerp(rb.gravityScale, ascendGravity, timeToChange * Time.deltaTime);
        }
        else
        {
            // Use water-like behavior: resistance increases as depth increases
            rb.gravityScale = Mathf.Lerp(rb.gravityScale, descendGravity, timeToChange * Time.deltaTime);
        }

        // Cap gravity to prevent erratic behavior at extreme depths
        rb.gravityScale = Mathf.Clamp(rb.gravityScale, -maxGravitySpeed, maxGravitySpeed);
    }

    private void PlayerDepthHandle()
    {
        curDepth = (int)Mathf.Abs(startPos.position.y - transform.position.y);
        if (curDepth > maxDepth)
        {
            maxDepth = curDepth;
        }
    }

    private void HandleDeath()
    {
        if (curHP == 0)
        {
        }
    }

    private async Task HurtandRegen()
    {
        await HurtnoBubble();
        await HealBubble();
    }

    private async Task HealBubble()
    {
        if (bubbleReady && !isRegenCD && curHP > 0)
        {
            isRegenCD = true;
            curHP = Mathf.Min(curHP + RegenAmount, maxHP);
            await Task.Delay((int)regenCooldown * 1000);
            isRegenCD = false;
        }
    }

    private async Task HurtnoBubble()
    {
        if (!bubbleReady && !isHurtCD)
        {
            isHurtCD = true;
            curHP = Mathf.Max(curHP - RegenAmount, 0);
            await Task.Delay((int)hurtCooldown * 1000);
            isHurtCD = false;
        }
    }

    private void BubbleCharge()
    {
        canCharge = true;

        if (canCharge)
        {
            if (Input.GetKeyDown(KeyCode.Z) && curHP > 0)
            {
                bubbleCharge++;
                if (bubbleCharge >= maxBubbleCharge)
                {
                    bubbleReady = true;
                    bubbleCharge = maxBubbleCharge;
                }
            }
        }
        else
            Debug.DrawRay(transform.position + offset, Vector3.down * checkRange, Color.red);
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = Input.GetAxisRaw("Horizontal") * playerSpeed;
        ClampPosition();
    }
    private void ClampPosition()
    {
        // Get the player's current position
        Vector3 clampedPosition = rb.position;

        // Clamp the X and Y coordinates
        clampedPosition.x = Mathf.Clamp(clampedPosition.x,
            SpawnManager.instance.referenceCamera.ViewportToWorldPoint(new Vector3(SpawnManager.instance.minSpawnRange, 0, 0)).x,
            SpawnManager.instance.referenceCamera.ViewportToWorldPoint(new Vector3(SpawnManager.instance.maxSpawnRange, 0, 0)).x);

        // Apply the clamped position back to the Rigidbody2D
        rb.position = clampedPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}