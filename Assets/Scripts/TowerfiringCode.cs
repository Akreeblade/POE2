using System;
using UnityEngine;

public class TowerfiringCode : MonoBehaviour
{
    [SerializeField] private float checkRadius = 5f;
    private string Tagtofind = "Enemy";
    [SerializeField] GameObject bullet;
    Collider enemy;
    Transform enemyTransform;

    [SerializeField] private float fireRate = 1f;
    private float fireTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if(enemy == null )
        {
            enemy = null;
            findEnemy();
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

    public bool findEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider col in colliders)
        {
           
            if (col.CompareTag(Tagtofind))
            {
                enemy = col;
                enemyTransform = col.transform;
                return true; 
            }
        }
        return false;
    }

    public void fireATenemy()
    {
        if (enemy == null)
        {
            Debug.LogError("There is no enemy assigned!");
            return;
        }

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

        bulletScript.SetEnemy(enemy.gameObject);
    }

    public void IfOutOfRaduis()
    {
        if (enemy == null)
        {
            return;
        }
        Vector3 offset = enemyTransform.position - transform.position;
        float sqrDistance = offset.sqrMagnitude;
        float sqrRadius = checkRadius * checkRadius;

        if (sqrDistance > sqrRadius)
        {
            enemy = null;
            //Debug.Log("Enemy is OUTSIDE the radius!");
            return ;
        }
        return;

    }
}
