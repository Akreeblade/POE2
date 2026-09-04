using System;
using UnityEngine;

public class Tower_Placement : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab; 
    private Transform towerParent;
    [SerializeField] private float checkRadius = 5f;
    public string targetTag = "Placement";
    private bool enough_money= false;
    private int tower_cost = 50;
    private bool placing_tower_mode = false;
    private Moneyholdscript moneyHoldScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyHoldScript = GetComponent<Moneyholdscript>();
    }

    public int  getTowercost()
    {
        return tower_cost;
    }
   
    // Update is called once per frame
    void Update()
    {
        if (placing_tower_mode)
        {
           
             if (Input.GetMouseButtonDown(0))
             {
                
                if (CanPlaceTower())
                {
                    if (moneyHoldScript.purchasetower(tower_cost))
                    {
                        PlaceTower();
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
    public void SetPlacingTowerMode(bool isPlacing)
    {
        placing_tower_mode = isPlacing;
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

    public void PlaceTower()
    {
        if (towerPrefab != null)
        {
            GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);//spawns the tower then 
            tower.transform.SetPositionAndRotation(towerParent.position, transform.rotation);// makes it transfrom the towerparent
            placing_tower_mode=false;
        }
        else
        {
            Debug.LogError("Tower prefab is not assigned!");
        }
    }

}
