using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Maintowerfiring : MonoBehaviour
{
    [SerializeField] protected float checkRadius;
    protected string Tagtofind = "Enemy";
    [SerializeField] GameObject bullet;
    protected List<Collider> enemy = new List<Collider>();
    protected List<Transform> enemyTransform = new List<Transform>();

    [SerializeField] protected float fireRate;
    protected float fireTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (enemy == null)
        {
            enemy = null;
            findenemies();
            return;
        }
        else
        {
            if (fireTimer <= 0f)
            {
                fireATenemy();
                fireTimer = fireRate;
            }
        }
        IfOutOfRaduis();
    }
    public void fireATenemy()
    {
        foreach (Collider col in enemy)
        {
            
            GameObject newBullet = Instantiate(
                bullet,
                transform.position,
                Quaternion.identity
            );

            bulletmovement bulletScript = newBullet.GetComponent<bulletmovement>();

            if (bulletScript == null)
            {
                Debug.LogError("The bullet prefab does not have a bulletmovement component!");
                return;
            }

            bulletScript.SetEnemy(col.gameObject);
        }
    }

    public void IfOutOfRaduis()
    {
       foreach (Collider col in enemy)
        {
            if (col == null)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance > checkRadius)
            {
                enemy.Remove(col);
                enemyTransform.Remove(col.transform);
                break; // Exit the loop to avoid modifying the collection while iterating
            }
        }

    }

    private void findenemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(Tagtofind))
            {
                enemy.Add(col);
                enemyTransform.Add(col.transform);

            }
        }
    }
}
