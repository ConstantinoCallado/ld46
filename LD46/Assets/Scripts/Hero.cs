using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static Hero heroRef;
    public Camera mainCamera;
    private Plate lastPlateClicked = null;
    
    void Awake()
    {
        heroRef = this;    
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
                if (lastPlateClicked != null && objectHit.GetComponent<Disk>() != null)
                {
                    
                    // Equip disk!
                    Disk clickedDisk = objectHit.GetComponent<Disk>();
                    lastPlateClicked.EquipDisk(clickedDisk);
                    lastPlateClicked = null;
                }

                // Player clicked a plate not spinning
                else if (lastPlateClicked == null && objectHit.GetComponent<Plate>() != null && !objectHit.GetComponent<Plate>().isSpinning)
                {
                    lastPlateClicked = objectHit.GetComponent<Plate>();
                    DiskManager.diskManagerRef.ShowDisksInDeck(true);
                }

                else if (lastPlateClicked == null && objectHit.GetComponent<CrossFader>() != null)
                {
                    CrossFader crossFader = objectHit.GetComponent<CrossFader>();
                    crossFader.Interact();
                }
                else if (lastPlateClicked == null && objectHit.GetComponent<Plate>() != null && objectHit.GetComponent<Plate>().isSpinning)
                {
                    AudioManager.audioManagerRef.PlayScratch();
                }
                
                // Player clicked somewhere else
                else
                {
                    lastPlateClicked = null;
                }
                
                // Hide the deck
                if (!lastPlateClicked)
                {
                    DiskManager.diskManagerRef.ShowDisksInDeck(false);
                }
            }
        }
    }
}
