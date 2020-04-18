using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public Camera mainCamera;
    private Plate lastPlateClicked = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectHit = hit.transform.gameObject;

                Debug.Log("Clicked " + objectHit.name);

                // Check the behaviour depending on the object clicked

                // If player clicked a disk after clicking a plate
                if (objectHit.GetComponent<Disk>() != null && lastPlateClicked != null)
                {
                    // Equip disk!
                    Disk clickedDisk = objectHit.GetComponent<Disk>();
                    lastPlateClicked.EquipDisk(clickedDisk);
                    lastPlateClicked = null;
                    DiskManager.diskManagerRef.UseDisk(clickedDisk);
                }

                // Player clicked a plate!
                else if (lastPlateClicked == null && objectHit.GetComponent<Plate>() != null)
                {
                    lastPlateClicked = objectHit.GetComponent<Plate>();
                    DiskManager.diskManagerRef.ShowDisksInDeck(true);
                }

                /*
                // Player clicked somewhere else
                else
                {
                    lastPlateClicked = null;
                }
                */
                // Hide the deck
                if(!lastPlateClicked)
                {
                    DiskManager.diskManagerRef.ShowDisksInDeck(false);
                }
            }
        }
    }
}
