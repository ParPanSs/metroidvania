using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parametrs")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parametrs")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float coolDownTimer = Mathf.Infinity;
   
    //private Animator animator;
    private EnemyPatrol enemyPatrol;
        private void Awake()
    {
        //animator = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (coolDownTimer >= attackCooldown)
            {
                coolDownTimer = 0;
                RangedAttack();
                //animator.SetTrigger("rangeAttack");
                //DamagePlayer();

            }
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }

    }

    private void RangedAttack()
    {
        coolDownTimer = 0;
        fireballs[FindRireball()].transform.position = firepoint.position;
        fireballs[FindRireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }
    private int FindRireball()
    {
        for(int i = 0; i< fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
            , 0, Vector2.left, 0, playerLayer);
        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

}
 