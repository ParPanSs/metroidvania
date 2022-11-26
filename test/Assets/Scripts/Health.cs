using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{ 
    [SerializeField] private int startingHealth;
    private int _currentHealth;
    
    public int numberOfHearts;
    [SerializeField] private Image[] hearth;

    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = startingHealth;
    }

    void Update()
    {
        for (int i = 0; i < hearth.Length; i++)
        {
            if (i < numberOfHearts)
            {
                hearth[i].enabled = true;
            }
            else
            {
                hearth[i].enabled = false;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, startingHealth);
        for (int i = 0; i < hearth.Length; i++)
        {
            if(i == _currentHealth)
            {
                Destroy(hearth[i]);
                Debug.Log("Taking Damage, health: " + _currentHealth);
                //player hurt
                _animator.SetTrigger("Damaged");
            }
            if(_currentHealth <= 0)
            {
                //player dead
                Debug.Log(this.name + " is Dead");
                Destroy(gameObject,1f);
            }
        }
        
    }
}
