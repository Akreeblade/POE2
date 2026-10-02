using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class canvasscript : MonoBehaviour
{
    TextMeshProUGUI Money;
    GameObject manager;
    Moneyholdscript moneyholdscript;
    [SerializeField]Button towerbutton1;
    [SerializeField] Button towerbutton2;
    [SerializeField] Button towerbutton3;

    int money;
    string moneystring;
    Tower_Placement towerplacement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   manager = GameObject.Find("GameManager");
        towerplacement = GameObject.Find("GameManager").GetComponent<Tower_Placement>();
       
        towerbutton1.onClick.AddListener(TowerActivation1);
        towerbutton2.onClick.AddListener(TowerActivation2);
        towerbutton3.onClick.AddListener(TowerActivation3);
        moneyholdscript = manager.GetComponent<Moneyholdscript>();
        Debug.Log(moneyholdscript);
        Money = GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log("Manager: " + manager);
        Debug.Log("Money Script: " + moneyholdscript);
        Debug.Log("Money Text: " + Money);
        Debug.Log("Tower Button: " + towerbutton1);
    }

    // Update is called once per frame
    void Update()
    {
        money = moneyholdscript.GetMoney();
        moneystring = "Money: " + money + "$";
        Money.text = moneystring;
    }

     public void TowerActivation1()
    {
        towerplacement.SetPlacingTower1Mode(true);
    }
    public void TowerActivation2()
    {
        towerplacement.SetPlacingTower2Mode(true);
    }
    public void TowerActivation3()
    {
        towerplacement.SetPlacingTower3Mode(true);
    }
}
