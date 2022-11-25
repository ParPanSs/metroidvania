using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [Header("FireTrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activTimer;
    //private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active; 

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)            
                StartCoroutine(ActivateFireTrap());            
            if (active) 
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
    private IEnumerator ActivateFireTrap()
    {
        triggered = true;
        spriteRend.color = Color.yellow;
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.red;
        active = true;
        //anim.SetBool("Active", true);
        yield return new WaitForSeconds(activTimer);
        active = false;
        triggered = false;
        spriteRend.color = Color.white;
        //anim.SetBool("Active", false);
    }
}
