using UnityEngine;

public class RainDroplet : MonoBehaviour
{
    public float fallSpeed = 5f;  
    public float lifetime = 10f; 
    public float damageAmount = 10f; 

    void Start()
    {
        
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            CharacterHealth playerHealth = other.GetComponent<CharacterHealth>();

            if (playerHealth != null)
            {
               
                playerHealth.TakeDamage(damageAmount);
            }
        }

        
        Destroy(gameObject);
    }
}