using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class canvasscript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Money;
    GameObject manager;
    Moneyholdscript moneyholdscript;
    Button towerbutton;
    int money;
    string moneystring;
    [SerializeField] TextMeshProUGUI towercost;

    Tower_Placement towerplacement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()

    {   manager = GameObject.Find("GameManager");
        towerplacement = GameObject.Find("GameManager").GetComponent<Tower_Placement>();
        towerbutton = GetComponentInChildren<Button>();
        towerbutton.onClick.AddListener(TowerActivation);
        moneyholdscript = manager.GetComponent<Moneyholdscript>();
        Debug.Log(moneyholdscript);
        
        

        Debug.Log("Manager: " + manager);
        Debug.Log("Money Script: " + moneyholdscript);
        Debug.Log("Money Text: " + Money);
        Debug.Log("Tower Button: " + towerbutton);
    }

    // Update is called once per frame
    void Update()
    {
        towercost.text = "Tower Cost: " + towerplacement.getTowercost() + "$";
        money = moneyholdscript.GetMoney();
        moneystring = "Money: " + money + "$";
        Money.text = moneystring;
    }

     public void TowerActivation()
    {
        towerplacement.SetPlacingTowerMode(true);
        
    }
}
