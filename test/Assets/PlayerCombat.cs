using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //public Animator characterAnimator;//аниматор главного персонажа
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    int attackDamage = 20;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    void Update()
    {
        if(Time.time >= nextAttackTime)
       if (Input.GetMouseButtonDown(0))
        {
            Attack();
            nextAttackTime = Time.time + 2f/attackRate;
        } 
    }

    void Attack()
    {
        //characterAnimator.SetTrigger("Attack");//параметр trigger attack для анимации атаки (стрелка от any state)

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
}
