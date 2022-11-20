using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    
    private float _inPutVertical;
    private float _horizontalMove;
    
    private bool _doubleJump;
    private bool _isHiding;
    private bool _canDash = true;
    private bool _isDashing;
    private bool _isFacingRight = true;
    private bool _isClimbing;
    
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
    }

    void Update()
    {
        if (_isDashing)
            return;
        
        _horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

        if (IsGrounded() && !Input.GetButton("Jump"))
            _doubleJump = false;
        
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || _doubleJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _doubleJump = !_doubleJump;  
            }
        }

        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f) 
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }
        
        if (IsGrounded() && _horizontalMove == 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
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

        Flip();
    }

    void FixedUpdate()
    {
        if (_isDashing)
            return;
        
        _rb.velocity = new Vector2(_horizontalMove, _rb.velocity.y);
        
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distRayforWallCheck, whatIsWall);
        if (hitInfo.collider != null)
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
            Debug.Log("Flip");
        }
    }
    
    private IEnumerator Dash()
    {
        Debug.Log("Dashing");
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
}
