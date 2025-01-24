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
    public float baseGravity;

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

    [Header("Bubble Bubbles")]
    public SpriteRenderer bubbleRenderer;
    public Animator bubbleAnim;
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
    }

    private async Task OnTakeDMG()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, hitMask) && canHit)
        {
            Debug.Log("Hit");
            if (bubbleReady)
            {
                bubbleReady = false;
                bubbleCharge = 0;
                bubbleRenderer.gameObject.GetComponent<BubbleAnimationController>().GotPOP();
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
        ratio = curDepth / 10000f;
        descendGravity = baseGravity + ratio * (maxGravitySpeed - baseGravity);

        if (curDepth <= 10000)
        {
            if (bubbleReady)
                rb.gravityScale = Mathf.Lerp(rb.gravityScale, ascendGravity, timeToChange * Time.deltaTime);
            else if (!bubbleReady)
                rb.gravityScale = Mathf.Lerp(rb.gravityScale, descendGravity, timeToChange * Time.deltaTime);
            //Debug.Log($"Current Gravity Scale : {rb.gravityScale}");
        }
        else
        {
            //Debug.Log(rb.gravityScale);
            rb.gravityScale = 0;
        }
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
            Debug.Log("Gameover");
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
            Debug.Log(curHP);
            await Task.Delay((int)hurtCooldown * 1000);
            isHurtCD = false;
        }
    }

    private void BubbleCharge()
    {
        canCharge = Physics2D.Raycast(transform.position + offset, Vector2.down, checkRange, mask);

        if (canCharge)
        {
            Debug.DrawRay(transform.position + offset, Vector3.down * checkRange, Color.green);
            if (Input.GetKeyDown(KeyCode.Z) && curHP > 0)
            {
                bubbleCharge++;
                if (bubbleCharge >= maxBubbleCharge)
                {
                    bubbleReady = true;
                    bubbleCharge = maxBubbleCharge;
                    bubbleRenderer.gameObject.GetComponent<BubbleAnimationController>().StartBubbleAnimation();
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
            SpawnManager.instance.referenceCamera.ViewportToWorldPoint(new Vector3(SpawnManager.instance.minSpawnRange,0,0)).x,
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
