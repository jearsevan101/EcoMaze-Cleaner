using System.Collections;
using System.Collections.Generic;
using TMPro; // Add TextMeshPro namespace
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canMove = true;
    private bool readyToJump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    [Header("Object Rotation")]
    [SerializeField] private Transform objectParent;

    [Header("Trash Placement")]
    [SerializeField] private Transform trashSlot1;
    [SerializeField] private Transform trashSlot2;
    [SerializeField] private Transform trashSlot3;
    [SerializeField] private Transform trashSlot4;

    // Make trashSlots public so TrashBin can access it
    public Transform[] trashSlots { get; private set; }
    [Header("Drop Setting")]
    public Transform trashParent;
    public float dropDistance = 2f;
    public float dropHeightCheck = 1f; // Height for checking collisions
    public LayerMask collisionCheckMask;
    public LayerMask collisionCheckMask2;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    [Header("UI")]
    public TextMeshProUGUI statusText;
    [Header("Interaction")]
    public float pickupRadius = 1.5f;

    [Header("Audio")]
    public AudioSource walkingAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource grabAudioSource;
    public AudioClip walkingClip;
    public AudioClip jumpClip;
    public AudioClip grabClip;
    public AudioClip dropClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        // Initialize trash slots array
        trashSlots = new Transform[] { trashSlot1, trashSlot2, trashSlot3, trashSlot4 };

        // Clear status text at start
        if (statusText != null) statusText.text = string.Empty;
        
        //setup walking audio
        if (walkingAudioSource != null)
        {
            walkingAudioSource.clip = walkingClip;
            walkingAudioSource.loop = true;
        }
    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (canMove)
        {
            MyInput();
            SpeedControl();
        }

        if (grounded)
        {
            rb.drag = groundDrag;

            // play the walking sound
            if (( horizontalInput != 0 || verticalInput != 0) && !walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Play();
            }
            else if (horizontalInput == 0 && verticalInput == 0)
            {
                walkingAudioSource.Pause();
            }
        }
        else
        {
            rb.drag = 0;

            if (walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Pause();
            }
        }

        RotateObjectParent(); // Rotate objectParent
        HandleInteraction(); // Check for trash pickup
        HandleDrop(); // Add this new method call
    }
    private void HandleDrop()
    {
        if (Input.GetKeyDown(dropKey))
        {
            DropFirstTrash();
        }
    }
    private bool IsPositionInsideBoxCollider(Vector3 position)
    {
        // Create a small overlap box at the position
        Collider[] colliders = Physics.OverlapBox(
            position,
            Vector3.one * 0.1f, // Small box size for point check
            Quaternion.identity,
            collisionCheckMask
        );

        foreach (Collider collider in colliders)
        {
            if (collider is BoxCollider)
            {
                return true;
            }
        }

        return false;
    }

    private Vector3 FindValidDropPosition(Vector3 initialPosition)
    {
        Vector3 dropPosition = initialPosition;
        float checkRadius = 1f; // Start checking 1 unit around
        int maxAttempts = 8; // Maximum number of positions to check
        float angleStep = 45f; // Check every 45 degrees around the point

        // First check the initial position
        if (!IsPositionInsideBoxCollider(dropPosition))
        {
            return dropPosition;
        }

        // If initial position is invalid, check in a circular pattern
        for (int i = 0; i < maxAttempts; i++)
        {
            float angle = i * angleStep;
            float radian = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(radian) * checkRadius,
                0f,
                Mathf.Sin(radian) * checkRadius
            );

            Vector3 testPosition = initialPosition + offset;

            if (!IsPositionInsideBoxCollider(testPosition))
            {
                return testPosition;
            }
        }

        // If no valid position found, return the original position
        // You might want to handle this case differently
        return initialPosition;
    }

    private void DropFirstTrash()
    {
        if (trashSlots[0].childCount > 0)
        {
            Transform trashTransform = trashSlots[0].GetChild(0);

            // Calculate the direction and intended drop position
            Vector3 dropDirection = orientation.forward;
            dropDirection.y = 0; // Keep the drop direction on the horizontal plane
            dropDirection = dropDirection.normalized;

            // Use the player's x, z position and set y to 0
            Vector3 intendedDropPosition = new Vector3(
                transform.position.x + dropDirection.x * dropDistance,
                0f, // Force y position to 0
                transform.position.z + dropDirection.z * dropDistance
            );

            // Perform a raycast to check for obstacles between the player and the intended drop position
            RaycastHit hit;
            bool hasObstacle = Physics.Raycast(
                transform.position + Vector3.up * 0.5f, // Start slightly above the ground to avoid floor collisions
                dropDirection,
                out hit,
                dropDistance,
                collisionCheckMask // Layer mask for obstacles
            );

            // If there's an obstacle, place the trash in front of the obstacle instead of the intended position
            Vector3 finalDropPosition;
            if (hasObstacle)
            {
                finalDropPosition = hit.point - dropDirection * 0.5f; // Slightly pull back from the obstacle
                finalDropPosition.y = 0f; // Ensure the Y position is set to 0
            }
            else
            {
                finalDropPosition = intendedDropPosition;
            }

            // Reparent the trash object and position it at the calculated drop position
            trashTransform.SetParent(trashParent);
            trashTransform.position = finalDropPosition;
            trashTransform.rotation = Quaternion.identity;
            trashTransform.localScale = Vector3.one * 3f; // Set the scale to 3

            // Enable physics and collider for the dropped object
            Rigidbody trashRb = trashTransform.GetComponent<Rigidbody>();
            if (trashRb != null)
            {
                trashRb.isKinematic = false;
            }

            Collider trashCollider = trashTransform.GetComponent<Collider>();
            if (trashCollider != null)
            {
                trashCollider.enabled = true;
            }

            // Play the drop sound
            if (grabAudioSource != null && dropClip != null)
            {
                grabAudioSource.clip = dropClip;
                grabAudioSource.time = 0.2f;
                grabAudioSource.Play();
            }
            else
            {
                Debug.LogWarning("Drop sound not set!");
            }

            // Shift remaining trash
            StartCoroutine(ShiftTrashWithDelay());
        }
    }
    private void RotateObjectParent()
    {
        if (objectParent != null)
        {
            float yRotation = orientation.eulerAngles.y; // Get camera's Y rotation
            objectParent.rotation = Quaternion.Euler(0, yRotation, 0); // Rotate objectParent
        }
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Play jump sound
        if (jumpAudioSource != null && jumpClip != null)
        {
            Debug.Log("PLAYING JUMP SOUND");
            jumpAudioSource.PlayOneShot(jumpClip);
        }
        else
        { Debug.LogWarning("Jump sound not set!");}
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(interactKey))
        {
            // Find closest trash within pickup radius
            Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius);
            float closestDistance = float.MaxValue;
            Trash closestTrash = null;

            foreach (Collider hit in hits)
            {
                Trash trash = hit.GetComponent<Trash>();
                if (trash != null)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTrash = trash;
                    }
                }
            }

            if (closestTrash != null)
            {
                Transform emptySlot = GetEmptyTrashSlot();
                if (emptySlot != null)
                {
                    // Parent the trash to the slot and reset its local position
                    Collider collider = closestTrash.GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.enabled = false; // Disables the collider
                    }
                    closestTrash.transform.SetParent(emptySlot);
                    closestTrash.transform.localPosition = new Vector3(0f, 0f, 1f);
                    closestTrash.transform.localRotation = Quaternion.identity;
                    closestTrash.transform.localScale = closestTrash.transform.localScale * 0.5f;

                    // Disable physics on the trash object
                    Rigidbody trashRb = closestTrash.GetComponent<Rigidbody>();
                    if (trashRb != null)
                    {
                        trashRb.isKinematic = true;
                    }

                    Debug.Log($"Picked up {closestTrash.trashData.trashName}");

                    // play the grab sound
                    if (grabAudioSource != null && grabClip != null)
                    {
                        grabAudioSource.PlayOneShot(grabClip);
                    }
                    else
                    {
                        Debug.LogWarning("Grab sound not set!");
                    }
                }
                else
                {
                    Debug.Log("Inventory is full!");
                }
            }
        }
    }
    public void DestroyFirstTrash()
    {
        if (trashSlots[0].childCount != 0)
        {
            // Get the first child of the first trash slot
            Transform trashTransform = trashSlots[0].GetChild(0);

            // Destroy the GameObject
            Destroy(trashTransform.gameObject);

            // Shift remaining trash objects
            StartCoroutine(ShiftTrashWithDelay());
        }

    }
    private IEnumerator ShiftTrashWithDelay()
    {
        // Wait until the next frame to ensure the object is destroyed
        yield return null;

        // Now shift the remaining trash
        ShiftTrashAfterDisposal();
    }
    public int GetCurrentTrashCount()
    {
        int count = 0;
        foreach (Transform slot in trashSlots)
        {
            if (slot.childCount > 0)
            {
                count++;
            }
        }
        return count;
    }
    private Transform GetEmptyTrashSlot()
    {
        foreach (Transform slot in trashSlots)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    public void ShiftTrashAfterDisposal()
    {
        // Loop through the trash slots
        for (int i = 0; i < trashSlots.Length - 1; i++)
        {
            Transform currentSlot = trashSlots[i];
            Transform nextSlot = trashSlots[i + 1];

            // If current slot is empty and the next slot has trash, move it to the current slot
            if (currentSlot.childCount == 0 && nextSlot.childCount > 0)
            {
                Transform trashToMove = nextSlot.GetChild(0); // Get the first trash object
                trashToMove.SetParent(currentSlot);          // Reparent to current slot
                trashToMove.localPosition = new Vector3(0f, 0f, 1f);
                trashToMove.localRotation = Quaternion.identity; // Reset rotation
            }
        }
    }
    public void UpdateStatus(string message, float duration)
    {
        StartCoroutine(DisplayStatus(message, duration));
    }

    private IEnumerator DisplayStatus(string message, float duration)
    {
        if (statusText != null)
        {
            statusText.text = message;
            yield return new WaitForSeconds(duration);
            yield return new WaitForSeconds(3f);
            statusText.text = string.Empty;
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Visualize pickup radius in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
