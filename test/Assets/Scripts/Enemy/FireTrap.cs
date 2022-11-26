using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private int damage;
    [Header("FireTrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activTimer;
    //private Animator anim;
    private SpriteRenderer _spriteRend;

    private bool _triggered;
    private bool _active; 

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        _spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_triggered)            
                StartCoroutine(ActivateFireTrap());            
            if (_active) 
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
    private IEnumerator ActivateFireTrap()
    {
        _triggered = true;
        _spriteRend.color = Color.yellow;
        yield return new WaitForSeconds(activationDelay);
        _spriteRend.color = Color.red;
        _active = true;
        //anim.SetBool("Active", true);
        yield return new WaitForSeconds(activTimer);
        _active = false;
        _triggered = false;
        _spriteRend.color = Color.white;
        //anim.SetBool("Active", false);
    }
}
