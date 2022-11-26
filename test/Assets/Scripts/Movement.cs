using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private Animator _animator;

    private float _inPutVertical;
    private float _horizontalMove;
    
    private bool _doubleJump;
    private bool _isHiding;
    private bool _canDash = true;
    private bool _isDashing;
    private bool _isFacingRight = true;
    private bool _isClimbing;
    
    private bool _doubleJumpAbility;
    private bool _dashAbility;
    private bool _climbAbility;
    private bool _invisibleAbility;
    
    private bool _inAbility;
    private bool _takingAbility;
    
    private bool _spawnDust;
    public GameObject dust;

    [SerializeField] private float jumpForce;
    [SerializeField] private float runSpeed;
    [SerializeField] private float checkRadius;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float distRayforWallCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isDashing)
            return;

        _horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            _doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || (_doubleJump && _doubleJumpAbility))
            {
                _doubleJump = !_doubleJump;
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
        }
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f) 
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && _dashAbility)
        {
            StartCoroutine(Dash());
        }

        if (IsGrounded())
        {
            if (_spawnDust)
            {
                Instantiate(dust, feetPosition.position, Quaternion.identity);
                _spawnDust = false;
                _animator.SetBool("isJumping", false);
            }
        }
        else
        {
            _spawnDust = true;
            _animator.SetBool("isJumping", true);
        }

        if (IsGrounded() && _horizontalMove == 0)
        {
            if (Input.GetKeyDown(KeyCode.Z) && _invisibleAbility)
            {
                _sprite.color = new Color(1f, 1f, 1f, 0.3f);
            }
            if(Input.GetKeyUp(KeyCode.Z))
            {
                _sprite.color = new Color(1f,1f,1f,1f);
            }
        }
        else
        {
            _sprite.color = new Color(1f,1f,1f,1f);
        }

        if (Input.GetKeyDown(KeyCode.E) && _inAbility)
            _takingAbility = true;
        
        if (_horizontalMove != 0)
        {
            _animator.SetBool("isWalk", true);
        }
        else
        {
            _animator.SetBool("isWalk", false);
        }

        Flip();
    }

    void FixedUpdate()
    {
        if (_isDashing)
            return;
        
        _rb.velocity = new Vector2(_horizontalMove, _rb.velocity.y);
        
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distRayforWallCheck, whatIsWall);
        if (hitInfo.collider != null && _climbAbility)
        {
            _isClimbing = true;
        }
        else
        {
            _isClimbing = false;
        }
        if (_isClimbing)
        {
            _inPutVertical = Input.GetAxisRaw("Vertical");
            _rb.velocity = new Vector2(_rb.velocity.x, _inPutVertical * runSpeed);
            _rb.gravityScale = 0;
        }
        else
        {
            _rb.gravityScale = 3;
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPosition.position, checkRadius, whatIsGround);
    }
    
    private void Flip()
    {
        if (_isFacingRight && _horizontalMove < 0 || !_isFacingRight && _horizontalMove > 0)
        {
            Vector3 localScale = transform.localScale;
            _isFacingRight = !_isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        _rb.gravityScale = originalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _inAbility = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ability") && _takingAbility)
        {
            switch (other.name)
            {
                case "Dash":
                    _dashAbility = true;
                    _takingAbility = !_takingAbility;
                    break;
                case "DoubleJump":
                    _doubleJumpAbility = true;
                    _takingAbility = !_takingAbility;
                    break;
                case "Climb":
                    _climbAbility = true;
                    _takingAbility = !_takingAbility;
                    break;
                case "Invisibility":
                    _invisibleAbility = true;
                    _takingAbility = !_takingAbility;
                    break;
            }
            Destroy(other);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _inAbility = false;
    }
}
