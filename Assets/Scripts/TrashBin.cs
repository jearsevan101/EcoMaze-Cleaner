using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public TrashBinData trashBinData; // ScriptableObject to define the bin's properties

    [Header("Interaction")]
    public float interactionRadius = 1.5f; // Interaction radius
    public KeyCode interactKey = KeyCode.E; // Key for interacting with the trash bin

    private void Update()
    {
        // Check for interact key press
        if (Input.GetKeyDown(interactKey))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius); // Interaction radius

            foreach (Collider hit in hits)
            {
                PlayerMovement player = hit.GetComponent<PlayerMovement>();
                if (player != null && player.carriedTrash.Count > 0)
                {
                    // Player has trash to deposit
                    DepositTrash(player);
                    break; // Stop checking after interacting with one player
                }
            }
        }
    }

    private void DepositTrash(PlayerMovement player)
    {
        TrashData carriedTrash = player.carriedTrash[0]; // Assume the player deposits the first trash in their list

        if (carriedTrash.trashType == trashBinData.acceptedType)
        {
            // Correct trash deposited
            ScoreManager.Instance.AddScore(trashBinData.pointsForCorrectTrash);
            Debug.Log($"Correct! {carriedTrash.trashName} placed in {trashBinData.binName}.");
        }
        else
        {
            // Wrong trash deposited
            ScoreManager.Instance.AddScore(trashBinData.penaltyForWrongTrash);
            Debug.Log($"Wrong! {carriedTrash.trashName} does not belong in {trashBinData.binName}.");
        }

        // Remove the trash from the player's inventory
        player.carriedTrash.RemoveAt(0);

        Debug.Log($"Trash deposited into {trashBinData.binName}. Remaining trash: {player.carriedTrash.Count}");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw interaction radius in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
