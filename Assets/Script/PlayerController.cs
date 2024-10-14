using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool bAttacking;
    private bool bComboExist;
    private bool bComboEnable;
    private int comboIndex;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] float scale = 3f;
    [SerializeField] float health = 100f;
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float attackCooldown = 1f;

    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] bool isGrounded;
    [SerializeField] float jumpForce = 3f;          // 기본 점프력
    [SerializeField] float maxJumpForce = 5f;       // 최대 점프력
    [SerializeField] float jumpHoldDuration = 0.2f; // 길게 눌렀을 때 추가 점프 지속시간
    [SerializeField] bool isJump;
    private int jumpCount = 0;
    private int maxJumpCount = 2;

    [SerializeField] float gravityScale = 1f;       // 기본 중력 값
    [SerializeField] float slowMotionGravityScale = 0.5f; // 슬로우 모션 상태일 때 중력 값

    [SerializeField] private int comboStep = 0;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackTimer = 0f;

    private bool isSlowMotion = false;
    private float originalMoveSpeed;
    private float originalRunSpeed;
    private float originalAttackCooldown;
    private float originalJumpForce;
    private float originalGravityScale;
    public float slowMotionFactor = 0.5f;

    private float jumpButtonHeldTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        originalMoveSpeed = moveSpeed;
        originalRunSpeed = runSpeed;
        originalAttackCooldown = attackCooldown;
        originalJumpForce = jumpForce;
        originalGravityScale = rb.gravityScale; // 현재 중력 값 저장
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
        HandleJump();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleSlowMotion();
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && (isGrounded || jumpCount < maxJumpCount))
        {
            if (!isGrounded)
            {
                jumpCount++;
                animator.SetTrigger("DoubleJump");
            }
            else
            {
                isGrounded = false;
                jumpCount = 1;
                animator.SetTrigger("Jump");
            }

            animator.SetBool("isJump", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpButtonHeldTime = 0f;
        }

        if (Input.GetKey(jumpKey) && !isGrounded && rb.velocity.y > 0)
        {
            jumpButtonHeldTime += Time.deltaTime;

            if (jumpButtonHeldTime < jumpHoldDuration)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(jumpForce, maxJumpForce, jumpButtonHeldTime / jumpHoldDuration));
            }
        }

        if (Input.GetKeyUp(jumpKey) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void HandleMovement()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), 0f);

        float moveSpeedToUse = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        rb.velocity = new Vector2(moveInput.x * moveSpeedToUse, rb.velocity.y);

        if (moveInput.x != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isRun", true);
                animator.SetBool("isWalk", false);
            }
            else
            {
                animator.SetBool("isRun", false);
                animator.SetBool("isWalk", true);
            }
        }
        else
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isWalk", false);
        }

        if (moveInput.x > 0)
            transform.localScale = new Vector3(1, 1, 1) * scale;

    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("isAttack", true);
            comboStep++;
            isAttacking = true;
            animator.SetTrigger("Attack");
        }

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                animator.SetBool("isAttack", false);
                isAttacking = false;
                comboStep = 0;
                attackTimer = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void ToggleSlowMotion()
    {
        if (isSlowMotion)
        {
            moveSpeed = originalMoveSpeed;
            runSpeed = originalRunSpeed;
            attackCooldown = originalAttackCooldown;
            jumpForce = originalJumpForce; // 점프력 복원
            rb.gravityScale = originalGravityScale; // 중력 값 복원
            animator.speed = 1.0f;
            isSlowMotion = false;
        }
        else
        {
            moveSpeed = originalMoveSpeed * slowMotionFactor;
            runSpeed = originalRunSpeed * slowMotionFactor;
            attackCooldown = originalAttackCooldown / slowMotionFactor;
            jumpForce = originalJumpForce * slowMotionFactor; // 점프력 줄이기
            rb.gravityScale = slowMotionGravityScale; // 중력도 줄이기
            animator.speed = slowMotionFactor;
            isSlowMotion = true;
        }
    }
}
