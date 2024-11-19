using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.E; // Key to pick up trash

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Trash Collection")]
    public int maxTrashCapacity = 2; // Max trash items the player can carry
    public List<TrashData> carriedTrash = new List<TrashData>(); // List of collected trash
    public List<Trash> carriedTrashObjects = new List<Trash>(); // List of Trash objects (GameObjects)

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        HandleInteraction(); // Check for trash pickup
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
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleInteraction()
    {
        // Check for interact key press
        if (Input.GetKeyDown(interactKey))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f); // Interaction radius

            foreach (Collider hit in hits)
            {
                Trash trash = hit.GetComponent<Trash>();
                if (trash != null)
                {
                    if (carriedTrash.Count < maxTrashCapacity)
                    {
                        // Pick up the trash
                        carriedTrash.Add(trash.trashData);  // Add TrashData to the list
                        carriedTrashObjects.Add(trash);  // Add Trash object (GameObject) to the list
                        Destroy(hit.gameObject); // Remove the trash object from the scene
                        Debug.Log($"Picked up {trash.trashData.trashName}");
                    }
                    else
                    {
                        Debug.Log("Trash capacity reached!");
                    }
                    break; // Stop checking after picking up one item
                }
            }
        }
    }
}
