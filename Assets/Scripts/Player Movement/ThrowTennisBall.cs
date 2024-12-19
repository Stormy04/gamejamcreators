using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTennisBall : MonoBehaviour, IThrowObject
{
    [SerializeField] private GameObject tennisBallPrefab;
    [SerializeField] private float throwForce = 10f;
    private Transform throwPoint;


    public void ThrowObject()
    {
        GameObject tennisBall = Instantiate(tennisBallPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = tennisBall.GetComponent<Rigidbody2D>();

        //Get the vector between the player and the mouse position

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePosition - (Vector2)transform.position;

        rb.AddForce(distance.normalized * throwForce, ForceMode2D.Impulse);
    }

}