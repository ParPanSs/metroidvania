using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;
    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;
        fireBalls[FindFireBall()].transform.position = firePoint.position;
        fireBalls[FindFireBall()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireBall()
    {
        for(int i = 0; i < fireBalls.Length; i++)
        {
            if(!fireBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void Update()
    {

        cooldownTimer += Time.deltaTime;

        if(cooldownTimer >= attackCooldown)
            Attack();
    }
}
