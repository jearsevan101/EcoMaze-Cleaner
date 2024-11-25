using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Freeze, Slow, Teleport }
    public TrapType trapType;

    public float slowDuration = 5f;       // Duration of slow effect
    public float slowMultiplier = 0.5f;  // Speed reduction factor
    public float freezeDuration = 5f;    // Duration of freeze effect

    public Transform teleportDestinationParent; // Parent object containing teleport destinations
    
    // setup audio for the trap
    private AudioSource audioSource;
    public AudioClip trapSound;

    private void OnTriggerEnter(Collider other)
    {
        Transform parentTransform = other.transform.parent;
        if (parentTransform != null && parentTransform.CompareTag("Player"))
        {
            Debug.Log($"detected player on {trapType}");
            PlayerMovement player = parentTransform.GetComponent<PlayerMovement>();
            if (player != null)
            {
                switch (trapType)
                {
                    case TrapType.Freeze:
                        StartCoroutine(ApplyFreezeEffect(player));
                        playTrapSound();
                        break;
                    case TrapType.Slow:
                        StartCoroutine(ApplySlowEffect(player));
                        playTrapSound();
                        break;
                    case TrapType.Teleport:
                        ApplyTeleportEffect(player);
                        playTrapSound();
                        break;
                }
            }
        }
    }

    private IEnumerator ApplyFreezeEffect(PlayerMovement player)
    {
        player.UpdateStatus($"Frozen for {freezeDuration:F1}s", freezeDuration);
        player.canMove = false; // Disable player movement
        yield return new WaitForSeconds(freezeDuration);
        player.canMove = true;  // Re-enable player movement
    }

    private IEnumerator ApplySlowEffect(PlayerMovement player)
    {
        player.UpdateStatus($"Slowed for {slowDuration:F1}s", slowDuration);
        float originalSpeed = player.moveSpeed;
        player.moveSpeed *= slowMultiplier; // Slow down the player
        yield return new WaitForSeconds(slowDuration);
        player.moveSpeed = originalSpeed;   // Restore original speed
    }

    private void ApplyTeleportEffect(PlayerMovement player)
    {
        player.UpdateStatus("Teleported!", 0f);
        if (teleportDestinationParent != null && teleportDestinationParent.childCount > 0)
        {
            int randomIndex = Random.Range(0, teleportDestinationParent.childCount);
            Transform randomDestination = teleportDestinationParent.GetChild(randomIndex);
            player.transform.position = randomDestination.position;
            Debug.Log($"Teleported to: {randomDestination.name}");
        }
        else
        {
            Debug.LogWarning("No teleport destinations available!");
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        //{
        //    audioSource = gameObject.AddComponent<AudioSource>();
        //    Debug.LogWarning("No AudioSource component found on trap object!");
        //}   
    }

    private void playTrapSound()
    {
        if (trapSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(trapSound);
        }
    }
}
