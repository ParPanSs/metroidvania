using System.Collections;
using UnityEngine;
public class Movement : MonoBehaviour
{
    private Rigidbody2D _rb;
    public SpriteRenderer _sprite;
    public Transform feetPosition;
    public LayerMask whatIsGround;
    
    private float _horizontalMove;
    private bool _doubleJump;
    private bool _isHiding;
    private bool _canDash = true;
    private bool _isDashing;
    private float _dashingPower = 20f;
    private float _dashingTime = 0.2f;
    private float _dashCooldown = 1f;
    private bool _isFacingRight = true;
    
    [SerializeField] private float jumpForce;
    [SerializeField] private float runSpeed;
    [SerializeField] private float checkRadius;
    [SerializeField] private TrailRenderer trail;

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
        _rb.velocity = new Vector2(transform.localScale.x * _dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        trail.emitting = false;
        _rb.gravityScale = originalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
}
