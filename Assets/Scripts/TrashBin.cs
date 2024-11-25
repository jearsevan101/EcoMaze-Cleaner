using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public TrashBinData trashBinData;
    public Checkpoint4Handler checkpoint4Handler;

    [Header("Interaction")]
    public float interactionRadius = 1.5f;
    public KeyCode interactKey = KeyCode.E;
    
    
    public TrashBin[] trashBins;

    [Header("Audio")]
    public AudioClip trashSound;
    public AudioSource audioSource;

    // Add a static flag to track if we're currently processing trash
    private static bool isProcessingTrash = false;
    private void Start()
    {
        trashBins = FindObjectsOfType<TrashBin>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            // If we're already processing trash, skip
            if (isProcessingTrash) return;

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
            if (closestBin != null && closestDistance <= 3f)
            {
                Debug.Log($"masuk closestBin {closestBin} and closest Distance");
                // Check first slot for trash
                Transform firstSlot = player.trashSlots[0];
                if (firstSlot.childCount > 0)
                {
                    Trash trashObject = firstSlot.GetChild(0).GetComponent<Trash>();
                    if (trashObject != null)
                    {
                        // Set the flag before processing
                        isProcessingTrash = true;
                        ProcessTrash(player, trashObject, closestBin);
                        // Reset the flag after a short delay
                        closestBin.playTrashSFX();
                        StartCoroutine(ResetProcessingFlag());
                    }
                }
                if (player == null) return;
            }
        }
    }

    private IEnumerator ResetProcessingFlag()
    {
        // Wait for a short time to ensure all potential calls have completed
        yield return new WaitForSeconds(0.1f);
        isProcessingTrash = false;
    }

    private void ProcessTrash(PlayerMovement player, Trash trashObject, TrashBin closestBin)
    {
        bool isCorrectBin = trashObject.trashData.trashType == closestBin.trashBinData.acceptedType;

        // Update score and checkpoint
        if (isCorrectBin)
        {
            ScoreManager.Instance.AddScore(trashBinData.pointsForCorrectTrash);
            Debug.Log($"Correct bin! +{trashBinData.pointsForCorrectTrash} points");
        }
        else
        {
            ScoreManager.Instance.AddScore(trashBinData.penaltyForWrongTrash);
            Debug.Log($"Wrong bin! {trashBinData.penaltyForWrongTrash} points");
        }
        player.DestroyFirstTrash();
        checkpoint4Handler?.OnTrashDisposed();

        // Destroy the trash object and shift remaining trash
        //Destroy(trashObject.gameObject);
        // player.ShiftTrashAfterDisposal();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void playTrashSFX()
    {
        if (audioSource != null && trashSound != null)

        {
            Debug.Log("suara masuk dimainkan");
            audioSource.PlayOneShot(trashSound);
        }
        else
        {
            Debug.LogWarning("No AudioSource or AudioClip specified for TrashBin!");
        }
    }
}
