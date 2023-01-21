using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    private GameObject placeTurretPrefab;
    [SerializeField] 
    private Camera mainCamera;

    private GameObject currentPlaceableObject;
    RaycastHit placableHit;

    public GameObject PlaceTurretPrefab{
        get
        {
            return placeTurretPrefab;
        }
        set
        {
            placeTurretPrefab = value;
        }
    }

    void Start(){
        Physics.IgnoreLayerCollision(6,6);
    }

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
            currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y+0.2f, hitInfo.point.z);
        }
    }
}
