using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    public float jumpSpeed;
    public Rigidbody2D rb;
    private Vector2 dir;
    private bool canjump;
    public LayerMask layer;
    private int count;
    private float timer;

    public ScreenShake sc;


    // Start is called before the first frame update
    void Start()
    {
        dir.x = 0f;
        dir.y = 0f;
        count = 0;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        canjump = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.up * -1,2.5f, layer).collider;
        Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + new Vector3(0, -2.5f, 0), Color.magenta, 1f);
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!canjump) dir.y = 0f; 
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        rb.velocity += Vector2.up * dir.y * jumpSpeed;
        //timer -= Time.fixedDeltaTime;
    }

    public void HorizontalMovement(InputAction.CallbackContext ctx)
    {
        dir.x = ctx.ReadValue<float>();
    }

    public void VerticalMovement(InputAction.CallbackContext ctx)
    {
        dir.y = ctx.ReadValue<float>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "caja") Debug.Log("Caja");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "star")
        {
            count++;
            Destroy(collision.gameObject, 0.5f);
            sc.Shake(6, 10, 5);
        }
        else if (collision.gameObject.tag == "pasanivel")
        {
            timer = 2f;
        }
    }
}
