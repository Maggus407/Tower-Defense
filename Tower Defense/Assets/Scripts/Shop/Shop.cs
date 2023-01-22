using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param);
        });
    }
}

public class Shop : MonoBehaviour
{
    PlaceObjects placeTurret;
    [SerializeField]
    List<TurretBluePrint> turretBluePrint;

    void Awake(){
        placeTurret = GameObject.Find("PlaceTurrets").GetComponent<PlaceObjects>();
    }

    void Start()
    {
        GameObject template = transform.GetChild(0).gameObject;
        for (int i = 0; i < turretBluePrint.Count; i++)
        {
            GameObject go = Instantiate(template, transform);
            go.transform.GetChild(0).GetComponent<Image>().sprite = turretBluePrint[i].icon;
            go.transform.GetChild(1).GetComponent<TMP_Text>().text = turretBluePrint[i].cost.ToString();

            go.GetComponent<Button>().AddEventListener(i, PurchaseTurret);
            
        }
        Destroy(template);
    }

    public void PurchaseTurret(int i){
        if (PlayerStats.Money >= turretBluePrint[i].cost)
        {
            PlayerStats.Money -= turretBluePrint[i].cost;
            placeTurret.PlaceTurretPrefab = turretBluePrint[i].prefab;
            placeTurret.enabled = true;
            placeTurret.Update();
        }
    }
}
