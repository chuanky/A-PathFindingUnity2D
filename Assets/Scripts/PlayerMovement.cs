using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        move_velocity();
    }

    private void move_velocity() {
        Vector2 moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.A)) {
            moveDir.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDir.x += 1;
        }
        if (Input.GetKey(KeyCode.W)) {
            moveDir.y += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDir.y -= 1;
        }
        moveDir = moveDir.normalized;

        rb.velocity = moveDir * moveSpeed * Time.deltaTime;
    }
}
