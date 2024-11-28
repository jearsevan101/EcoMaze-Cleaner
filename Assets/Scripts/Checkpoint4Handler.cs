using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint4Handler : MonoBehaviour
{
    public ChecklistManager checklistManager;
    public int requiredTrashDisposal = 4;
    private int currentTrashDisposal = 0;
    public GameObject[] trashPrefab;

    public void OnTrashDisposed()
    {
        currentTrashDisposal++;
        UpdateText();
        Debug.Log("Trash disposed: " + currentTrashDisposal);
        if (currentTrashDisposal == requiredTrashDisposal)
        {
            // buat step4 teks jadi strikethrough
            checklistManager.step4.fontStyle = TMPro.FontStyles.Strikethrough;
            // ganti image step 4 dengan gameobjek checklistimage3
            checklistManager.CompleteCurrentStep();
        }
    }

    public void UpdateText() {
        if (checklistManager.step4 != null) { 
            checklistManager.step4.text = $"Put the trash inside the trash bin \n {currentTrashDisposal} / {requiredTrashDisposal} items";
        }
    }

}

