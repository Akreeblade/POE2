using UnityEngine;

public class TowerClass : MonoBehaviour
{
    [SerializeField] protected int health ;
    [SerializeField] protected float checkRadius;
    protected string Tagtofind = "Enemy";
    [SerializeField] GameObject bullet;
    protected Collider enemy;
    protected Transform enemyTransform;
    public string targetTag = "Placement";

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
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
            return;
        }
        return;

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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void setspawnertoflase()
    {
        GameObject spawn=GetClosestObjectWithTag();
        spawn.GetComponent<SpawnPlace>().SetOccupied(false);
    }
    GameObject GetClosestObjectWithTag()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject Spawn = null;
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in taggedObjects)
        {
            float distanceToSq = (obj.transform.position - currentPosition).sqrMagnitude;
            if (distanceToSq < shortestDistance)
            {
                shortestDistance = distanceToSq;
                Spawn = obj;
            }
        }

        return Spawn;
    }
}
