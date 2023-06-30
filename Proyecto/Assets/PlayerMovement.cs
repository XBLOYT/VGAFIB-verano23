using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    public float jumpSpeed;
    private Vector2 dir;
    public Rigidbody2D rb;

    public int numerosaltos;
    private bool canjump;
    public LayerMask layer;
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        dir.x = 0f;
        dir.y = 0f;
        rb = GetComponent<Rigidbody2D>();
        numerosaltos = 2;
        canjump = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        grounded = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.up * -1, 2.5f, layer).collider;
        if (canjump)
        {
            rb.AddForce(Vector2.up * dir.y * jumpSpeed, ForceMode2D.Impulse);
            numerosaltos--;
        }
        if(numerosaltos == 0)
        {
            canjump = false;
        }
    }

    public void HorizontalMovement(InputAction.CallbackContext ctx)
    {
        dir.x = ctx.ReadValue<float>();
    }

    public void VerticalMovement(InputAction.CallbackContext ctx)
    {
        dir.y = ctx.ReadValue<float>();
    }
}
