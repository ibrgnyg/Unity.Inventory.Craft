using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public bool canMove = true;

    [HideInInspector] public InputController inputController;

    Rigidbody rb;

    Vector3 direction = Vector3.zero;

    private void Awake()
    {
        inputController = GetComponent<InputController>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (inputController.isMove && canMove)
        {
            direction = new Vector3(inputController.directionV2.x, 0f, inputController.directionV2.y);
            direction = transform.TransformDirection(direction);
            rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
        }
    }

}
