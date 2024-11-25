using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint3Handler : MonoBehaviour
{
    // Start is called before the first frame update
    public ChecklistManager checklistManager;
    public int requiredTrashPickup = 4;
    private int currentTrashPickup = 0;
    public PlayerMovement playerMovement;
    public void OnTrashPickedUp()
    {
        currentTrashPickup = playerMovement.GetCurrentTrashCount();
        Debug.Log("Trash picked up: " + currentTrashPickup);

        if (currentTrashPickup >= requiredTrashPickup)
        {
            checklistManager.CompleteCurrentStep();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnTrashPickedUp();
            UpdateTextPickup();
        }
        if (currentTrashPickup == requiredTrashPickup)
        {
            checklistManager.CompleteCurrentStep();
        }
        
    }

    private void UpdateTextPickup()
    {
        if (checklistManager.step3 != null)
        {
            checklistManager.step3.text = $"Press E to pick up trash \n {currentTrashPickup} / {requiredTrashPickup} items";
        }
    }

}
