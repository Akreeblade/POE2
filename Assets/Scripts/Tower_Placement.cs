using System;
using UnityEngine;

public class Tower_Placement : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab1;
    [SerializeField] private GameObject towerPrefab2;
    [SerializeField] private GameObject towerPrefab3;

    private Transform towerParent;
    [SerializeField] private float checkRadius = 5f;
    public string targetTag = "Placement";
    private bool enough_money= false;
    private int tower_cost = 50;
    private int tower_cost2 = 150;
    private int tower_cost3 = 250;
    private bool placing_tower1_mode = false;
    private bool placing_tower2_mode = false;
    private bool placing_tower3_mode = false;
    private Moneyholdscript moneyHoldScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyHoldScript = GetComponent<Moneyholdscript>();
    }

   
    // Update is called once per frame
    void Update()
    {
        if (placing_tower1_mode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CanPlaceTower())
                {
                    if (moneyHoldScript.purchasetower(tower_cost))
                    {
                        PlaceTower1();
                    }
                    else
                    {
                        Debug.Log("Not enough money to place tower!");
                    }
                }
                else
                {
                    Debug.Log("cant place tower here");
                }
            }
        }
        if (placing_tower2_mode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CanPlaceTower())
                {
                    if (moneyHoldScript.purchasetower(tower_cost2))
                    {
                        // FIXED: Calls PlaceTower2 now
                        PlaceTower2();
                    }
                    else
                    {
                        Debug.Log("Not enough money to place tower!");
                    }
                }
                else
                {
                    Debug.Log("cant place tower here");
                }
            }
        }
        if (placing_tower3_mode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CanPlaceTower())
                {
                    if (moneyHoldScript.purchasetower(tower_cost3))
                    {
                        // FIXED: Calls PlaceTower3 now
                        PlaceTower3();
                    }
                    else
                    {
                        Debug.Log("Not enough money to place tower!");
                    }
                }
                else
                {
                    Debug.Log("cant place tower here");
                }
            }
        }

    }

    public int GetTowerCost()
    {
        return tower_cost;
    }
    public void SetPlacingTower1Mode(bool isPlacing)
    {
        placing_tower1_mode = isPlacing;
        placing_tower2_mode = !isPlacing;
        placing_tower3_mode = !isPlacing;
    

}
    public void SetPlacingTower2Mode(bool isPlacing)
    {
        placing_tower1_mode = !isPlacing;
        placing_tower2_mode =isPlacing;
        placing_tower3_mode = !isPlacing;

    }
    public void SetPlacingTower3Mode(bool isPlacing)
    {
        placing_tower1_mode = !isPlacing;
        placing_tower2_mode = !isPlacing;
        placing_tower3_mode = isPlacing;
    }

    public void SetEnoughMoney(bool hasEnough)
    {
        enough_money = hasEnough;
    }

    public bool CanPlaceTower()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
                if(Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("raycast hit");
                    Vector3 mouseWorldPosition = hit.point;
        
                    Collider[] colliders = Physics.OverlapSphere(mouseWorldPosition, checkRadius);
                    foreach (Collider col in colliders)
                    {
        
                        if (col.CompareTag(targetTag))
                        {
                            if(col.GetComponent<SpawnPlace>().IsOccupied()==false)
                            {
                                towerParent = col.transform;
                                col.GetComponent<SpawnPlace>().SetOccupied(true);
                                return true; //can place the tower
                            }
                        }
                    }
                }
                Debug.Log("cant place tower here!");
                towerParent = null;
                return false; // no place tower
        
        
                 
    }

    public void PlaceTower1()
    {
        if (towerPrefab1 != null)
        {
            GameObject tower = Instantiate(towerPrefab1, transform.position, Quaternion.identity);//spawns the tower then 
            tower.transform.SetPositionAndRotation(towerParent.position, transform.rotation);// makes it transfrom the towerparent
            placing_tower1_mode=false;
        }
        else
        {
            Debug.LogError("Tower prefab is not assigned!");
        }
    }
    public void PlaceTower2()
    {
        if (towerPrefab2 != null)
        {
            GameObject tower = Instantiate(towerPrefab2, transform.position, Quaternion.identity);//spawns the tower then 
            tower.transform.SetPositionAndRotation(towerParent.position, transform.rotation);// makes it transfrom the towerparent
            placing_tower2_mode = false;
        }
        else
        {
            Debug.LogError("Tower prefab is not assigned!");
        }
    }
    public void PlaceTower3()
    {
        if (towerPrefab3 != null)
        {
            GameObject tower = Instantiate(towerPrefab3, transform.position, Quaternion.identity);//spawns the tower then 
            tower.transform.SetPositionAndRotation(towerParent.position, transform.rotation);// makes it transfrom the towerparent
            placing_tower3_mode = false;
        }
        else
        {
            Debug.LogError("Tower prefab is not assigned!");
        }
    }

}
