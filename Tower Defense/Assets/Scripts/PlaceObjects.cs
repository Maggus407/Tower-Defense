using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public GameObject placeTurretPrefab;

    private GameObject currentPlaceableObject;
    RaycastHit placableHit;

    // Update is called once per frame
    public void Update()
    {
         HandleNewObjectKey();   
         if(currentPlaceableObject != null){
            MoveCurrentPlaceableObjectToMouse();
            ReleaseIfClicked();
         }
    }

    void ReleaseIfClicked(){
        if(Input.GetMouseButtonDown(0) && placableHit.transform.tag == "Placeable"){
            currentPlaceableObject.GetComponent<Collider>().enabled = true;
            currentPlaceableObject = null;
            this.enabled = false;
        }
    }

    void HandleNewObjectKey(){
            if(currentPlaceableObject == null){
                currentPlaceableObject = Instantiate(placeTurretPrefab);
            }
    }

    void MoveCurrentPlaceableObjectToMouse(){
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo)){
            placableHit = hitInfo;
            currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
        }
    }
}
