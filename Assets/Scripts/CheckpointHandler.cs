// Script untuk Step 1: Berjalan ke Checkpoint
using UnityEngine;

public class WalkToCheckpoint : MonoBehaviour
{
    public ChecklistManager checklistManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint reached!");
            checklistManager.CompleteCurrentStep();
            Destroy(gameObject);
        }
    }
}

