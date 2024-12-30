using UnityEngine;
using TMPro;

public class SetPositionByTag : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private GameObject objectToInstantiate;
    [SerializeField]
    private Vector3 instantiatePosition;
    [SerializeField]
    private TMP_Text plankCountText;

    private int plankCount = 0;
    private const int requiredPlankCount = 5;
    private bool canInstantiate = false;
    [SerializeField]
    private GameObject questline;
    [SerializeField]
    private GameObject questdot;
    private void Start()
    {
        UpdatePlankCountText();
    }

    private void Update()
    {
        if (canInstantiate && plankCount >= requiredPlankCount && Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(objectToInstantiate, instantiatePosition, Quaternion.identity);
            questline.SetActive(false);
            questdot.SetActive(false);
            plankCount = 0; // Reset the count if needed
            UpdatePlankCountText();
            canInstantiate = false; // Reset the flag
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bed"))
        {
            other.transform.position = targetPosition;

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            DraggableObject draggableObject = other.GetComponent<DraggableObject>();
            if (draggableObject != null)
            {
                draggableObject.LockObject();
            }
        }
        else if (other.CompareTag("Plank"))
        {
            plankCount++;
            Destroy(other.gameObject);
            UpdatePlankCountText();
        }
        else if (other.CompareTag("Player"))
        {
            canInstantiate = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInstantiate = false;
        }
    }

    private void UpdatePlankCountText()
    {
        if (plankCountText != null)
        {
            plankCountText.text = "Planks Collected: " + plankCount + "/" + requiredPlankCount;
        }
    }
}
