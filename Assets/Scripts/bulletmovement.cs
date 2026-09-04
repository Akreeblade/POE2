using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class bulletmovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private GameObject enemy;

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy_walking enemy = other.GetComponentInParent<Enemy_walking>();

            if (enemy != null)
            {
                enemy.takedamage(1);
                Destroy(gameObject);
            }
        }
    }
}
