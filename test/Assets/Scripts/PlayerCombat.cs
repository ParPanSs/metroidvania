using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    private Animator _animator;
    public int numberOfClicks;
    private float _lastClickedTime;
    private float _maxComboDelay = 1.5f;
    [SerializeField] private float nextAttackTime;
    [SerializeField] private int attackDamage = 20;
    private Rigidbody2D _rb;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.time - _lastClickedTime > _maxComboDelay)
            numberOfClicks = 0;
        
        if (Input.GetMouseButtonDown(0) && _rb.velocity == new Vector2(0,0))
        {
            _lastClickedTime = Time.time;
            numberOfClicks++;
            if(numberOfClicks == 1)
                _animator.SetBool("Attack1", true);
            else
            {
                _animator.SetBool("Attack1", true);
            }
            numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 3);
            Attack();
            nextAttackTime = Time.time + 2f / attackRate;
        }
        
        
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy is hit " + enemy.GetComponent<Enemy>().currentHealth);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawSphere(attackPoint.position, attackRange); 
    }

    public void return1()
    {
        if (numberOfClicks >= 2)
        {
            _animator.SetBool("Attack2", true);
        }
        else
        {
            _animator.SetBool("Attack1", false);
            numberOfClicks = 0;
        }
    }
    public void return2()
    {
        if (numberOfClicks >= 3)
        {
            _animator.SetBool("Attack3", true);
        }
        else
        {
            _animator.SetBool("Attack2", false);
            numberOfClicks = 0;
        }
    }

    public void return3()
    {
        _animator.SetBool("Attack1", false);
        _animator.SetBool("Attack2", false);
        _animator.SetBool("Attack3", false);
        numberOfClicks = 0;
    }
}
