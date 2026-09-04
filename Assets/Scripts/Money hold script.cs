using UnityEngine;

public class Moneyholdscript : MonoBehaviour
{

    [SerializeField] int money = 100;
    Tower_Placement Tower_Placement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tower_Placement = GetComponent<Tower_Placement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetMoney()
    {
        return money;
    }

    public int addmoney(int amount)
    {
        money += amount;
        return money;
    }


    public bool purchasetower(int tower_cost)
    {
        
        if (money >= tower_cost)
        {
            money -= tower_cost;
            Tower_Placement.SetEnoughMoney(true);
            Debug.Log("Tower purchased! Remaining money: " + money);
            return true;
        }
        else
        {
            Tower_Placement.SetEnoughMoney(false);
            Debug.Log("Not enough money to purchase the tower!");
            return false;
        }
    }
}
