using UnityEngine;

public class Shop : MonoBehaviour
{
    PlaceObjects placeTurret;

    void Awake(){
        placeTurret = GameObject.Find("PlaceTurrets").GetComponent<PlaceObjects>();
    }

    public void PurchaseStandardTurret(){
        Debug.Log("1");
        placeTurret.enabled = true;
        placeTurret.Update();
    }
}
