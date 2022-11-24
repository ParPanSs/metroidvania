using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
        private void Awake()
    {
        currentHealth = startingHealth;   
    }
    public void TakeDamage(float _damage)
    {
     currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
     if(currentHealth > 0)
        {
            Debug.Log("Taking Damage, health: " + currentHealth);
            //player hurt
        }
        else
        {
            //player dead
            Debug.Log(this.name + " is Dead");
            Destroy(gameObject,1f);
        }
    }
}
