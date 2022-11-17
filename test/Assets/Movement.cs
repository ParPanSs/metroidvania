using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D _rb;
    public Transform feetPosition;
    public LayerMask whatIsGround;
    
    private float _horizontalMove;
    private bool _doubleJump;
    
    [SerializeField] private float jumpForce;
    [SerializeField] private float runSpeed;
    [SerializeField] private float checkRadius;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
    }

    void FixedUpdate()
    {
        if (!Mathf.Approximately(_horizontalMove, 0))
        {
            transform.rotation = _horizontalMove < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }
        
        _rb.velocity = new Vector2(_horizontalMove, _rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPosition.position, checkRadius, whatIsGround);
    }
    
    

}
