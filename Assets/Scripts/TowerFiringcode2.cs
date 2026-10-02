using UnityEngine;

public class TowerFiringcode2 : TowerClass
{
    //[SerializeField] GameObject bullet;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

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
}
