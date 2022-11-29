using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{ 
    [SerializeField] private int startingHealth;
    public int _currentHealth;
    
    public int numberOfHearts;
    [SerializeField] private Image[] hearth;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;


    private Animator _animator;
    private Rigidbody2D _rb;
    [SerializeField] private float pushForce;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = startingHealth;
        _rb = GetComponent<Rigidbody2D>();
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
                hearth[i].sprite = emptyHeart;
                _rb.velocity = new Vector2(pushForce * 10, pushForce + 5);
                //player hurt
                _animator.SetTrigger("Damaged");
            }
            if(_currentHealth <= 0)
            {                
                //player dead
                Destroy(gameObject,1f);
                SceneManager.LoadScene(0);      
            }
           
        }
         //enemy dead
            if (GetComponentInParent<EnemyPatrol>() != null)
                GetComponentInParent<EnemyPatrol>().enabled = false;
            if (GetComponentInParent<MeleeEnemy>() != null)
                GetComponent<MeleeEnemy>().enabled = false;           
    }
}
