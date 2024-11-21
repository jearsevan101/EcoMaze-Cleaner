using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public TrashBinData trashBinData; // ScriptableObject to define the bin's properties
    public Checkpoint4Handler checkpoint4Handler;

    [Header("Interaction")]
    public float interactionRadius = 1.5f; // Interaction radius
    public KeyCode interactKey = KeyCode.E; // Key for interacting with the trash bin

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            // Find all TrashBin scripts in the scene
            TrashBin[] trashBins = FindObjectsOfType<TrashBin>();

            // Find the closest TrashBin to the player
            TrashBin closestBin = null;
            float closestDistance = float.MaxValue;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();

            if (player == null)
            {
                Debug.LogError("No PlayerMovement script found in the scene!");
                return;
            }

            foreach (TrashBin bin in trashBins)
            {
                float distance = Vector3.Distance(player.transform.position, bin.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBin = bin;
                }
            }

            // Check if player is close enough and has trash
            if (closestBin != null && closestDistance <= 3f && player.carriedTrash.Count > 0)
            {
                TrashData carriedTrash = player.carriedTrash[0];

                Debug.Log($"Closest Bin: {closestBin.trashBinData.binName}");
                Debug.Log($"Carried Trash Type: {carriedTrash.trashType}");
                Debug.Log($"Bin Accepted Type: {closestBin.trashBinData.acceptedType}");

                if (carriedTrash.trashType == closestBin.trashBinData.acceptedType)
                {
                    checkpoint4Handler.OnTrashDisposed();
                    ScoreManager.Instance.AddScore(closestBin.trashBinData.pointsForCorrectTrash);
                    Debug.Log("Correct trash deposited!");
                }
                else
                {
                    checkpoint4Handler.OnTrashDisposed();
                    ScoreManager.Instance.AddScore(closestBin.trashBinData.penaltyForWrongTrash);
                    Debug.Log("Wrong trash deposited!");
                }

                // Remove the trash from the player's inventory
                player.carriedTrash.RemoveAt(0);
                player.carriedTrashObjects.RemoveAt(0);
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

            // Remove the trash from the player's inventory
            player.carriedTrash.RemoveAt(0);
            player.carriedTrashObjects.RemoveAt(0);
        }
        else
        {
            // Wrong trash deposited
            ScoreManager.Instance.AddScore(trashBinData.penaltyForWrongTrash);
            Debug.Log($"Wrong! {carriedTrash.trashName} does not belong in {trashBinData.binName}.");

            // Remove the trash from the player's inventory
            player.carriedTrash.RemoveAt(0);
            player.carriedTrashObjects.RemoveAt(0);
        }

        Debug.Log($"Trash deposited into {trashBinData.binName}. Remaining trash: {player.carriedTrash.Count}");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw interaction radius in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
