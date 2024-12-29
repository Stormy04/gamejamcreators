using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialPosition;
    private bool isLocked = false; // Add this flag

    [Header("Drag Settings")]
    [Tooltip("Sorting order to use while dragging.")]
    public int dragSortingOrder = 10;

    [Header("Visual Feedback")]
    [Tooltip("Color applied to the object while dragging.")]
    public Color dragColor = Color.yellow;

    private int originalSortingOrder;
    private Color originalColor;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;

        if (spriteRenderer != null)
        {
            originalSortingOrder = spriteRenderer.sortingOrder;
            originalColor = spriteRenderer.color;
        }
    }

    private void OnMouseDown()
    {
        if (isLocked) return; // Prevent dragging if the object is locked

        if (spriteRenderer != null)
        {
            // Change the sorting order to bring the object to the foreground
            spriteRenderer.sortingOrder = dragSortingOrder;

            // Change the object's color for visual feedback
            spriteRenderer.color = dragColor;
        }

        // Disable physics
        if (rb != null)
            rb.isKinematic = true;

        // Calculate the offset
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePosition;
        offset.z = 0; // Ensure the object stays in the same z-plane
    }

    private void OnMouseDrag()
    {
        if (isLocked) return; // Prevent dragging if the object is locked

        // Update the position to follow the mouse
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Maintain the original Z position
        transform.position = mousePosition + offset;
    }

    private void OnMouseUp()
    {
        if (isLocked) return; // Prevent dragging if the object is locked

        if (spriteRenderer != null)
        {
            // Reset the sorting order to its original value
            spriteRenderer.sortingOrder = originalSortingOrder;

            // Reset the color
            spriteRenderer.color = originalColor;
        }

        // Re-enable physics
        if (rb != null)
            rb.isKinematic = false;
    }

    private void Update()
    {
        // Check if the object has fallen below a certain Y position
        if (transform.position.y < -5) // You can adjust the threshold value as needed
        {
            // Reset the object's position to the initial position
            transform.position = initialPosition;

            // Reset the sorting order and color
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = originalSortingOrder;
                spriteRenderer.color = originalColor;
            }

            // Re-enable physics
            if (rb != null)
                rb.isKinematic = false;
        }
    }

    public void LockObject()
    {
        isLocked = true; // Set the flag to lock the object
    }
}
