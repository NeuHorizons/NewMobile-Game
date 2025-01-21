using UnityEngine;

public class RainDroplet : MonoBehaviour
{
    public float fallSpeed = 5f;  
    public float lifetime = 10f; 

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
       
        Destroy(gameObject);
    }
}
