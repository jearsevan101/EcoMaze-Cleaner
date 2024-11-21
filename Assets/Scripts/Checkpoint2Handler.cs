using UnityEngine;

public class Checkpoint2Handler : MonoBehaviour
{
    public ChecklistManager checklistManager;
    public int requiredJumps = 3;
    private int currentJumps = 0;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Deteksi lompatan  
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Lakukan lompatan  
        rb.AddForce(Vector3.up * 0f, ForceMode.Impulse); // Sesuaikan kekuatan lompatan  

        currentJumps++;
        Debug.Log("Jump!");
        Debug.Log($"Jump {currentJumps} / {requiredJumps} times");

        // Update teks step untuk menunjukkan progress lompatan  
        UpdateJumpText();

        // Periksa apakah sudah mencapai jumlah lompatan yang dibutuhkan  
        if (currentJumps == requiredJumps)
        {
            checklistManager.CompleteCurrentStep();
        }
    }

    private bool IsGrounded()
    {
        // Cek apakah pemain menyentuh tanah  
        return Physics.Raycast(transform.position, Vector3.down, 1.1f); // Sesuaikan jarak raycast  
    }

    private void UpdateJumpText()
    {
        // Perbarui teks step dengan progress lompatan  
        if (checklistManager.step2 != null)
        {
            checklistManager.step2.text = $"Press space to jump \n {currentJumps} / {requiredJumps} times";
        }
    }
}