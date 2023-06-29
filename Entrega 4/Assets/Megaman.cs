using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Megaman : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public Rigidbody2D rb;
    private Vector2 dir;
    public Animator animator;

    private bool canjump;
    public LayerMask layer;

    private bool shoot;
    public GameObject bullet;
    public float bulletspeed;
    public float dirbullet;
    public Transform spawnbullet;

    // Start is called before the first frame update
    void Start()
    {
        dir.x = 0f;
        dir.y = 0f;
        canjump = true;
        shoot = false;
        dirbullet = -1;
    }

    // Update is called once per frame
    void Update()
    {
        canjump = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.up * -1, 0.4f, layer).collider;
        Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + new Vector3(0, -0.4f, 0), Color.magenta, 1f);
        animator.SetFloat("speed", dir.x);
        animator.SetBool("salto", !canjump);
        animator.SetFloat("isjumping", canjump ? 0f : 1f);
        if (dir.x > 0) transform.eulerAngles = new Vector3(0, 180, 0);
        else if (dir.x < 0) transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (!canjump) dir.y = 0f;
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        rb.velocity += Vector2.up * dir.y * jumpSpeed;

        if (shoot)
        {
            var bulletCont = Instantiate(bullet, spawnbullet.position, Quaternion.identity);
            bulletCont.GetComponent<Bullet>().dir = dirbullet;
            bulletCont.gameObject.SetActive(true);
            Destroy(bulletCont.gameObject, 1f);
            shoot = false;
        }
    }

    public void HorizontalMovement(InputAction.CallbackContext ctx)
    {
        dir.x = ctx.ReadValue<float>();
        if (dir.x != 0) dirbullet = dir.x;
    }

    public void VerticalMovement(InputAction.CallbackContext ctx)
    {
        dir.y = ctx.ReadValue<float>();
    }

    public void onShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) shoot = true;
    }
}
