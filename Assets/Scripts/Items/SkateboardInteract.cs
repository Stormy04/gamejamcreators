using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardInteract : MonoBehaviour
{
    public float interactionDistance = 2f;
    private CharacterController playerController;
    private Rigidbody2D rb;

    void Start()
    {
        playerController = FindObjectOfType<CharacterController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                rb.isKinematic = true;
                playerController.MountSkateboard(transform);
            }
        }
    }
}
