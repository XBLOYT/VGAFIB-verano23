using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

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
    private bool paused;
    public GameObject pauseMenu;

    public float health;
    public float maxHealth;
    public bool dead;

    public Image healthBar;
    public Slider healthSlider;

    public int counter;
    public TMP_Text counterText;

    public GameObject particleDamage;

    // Start is called before the first frame update
    void Start()
    {
        dir.x = 0f;
        dir.y = 0f;
        count = 0;
        timer = 0f;
        paused = false;
        pauseMenu.SetActive(false);
        health = 3;
        maxHealth = health;
        dead = false;
        healthBar.fillAmount = health/maxHealth;
        healthSlider.value = health;
        counter = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && !dead)
        {
            bool a = Physics2D.Raycast(this.gameObject.transform.position, this.gameObject.transform.up * -1, 2.5f, layer).collider;
            if(counter == 0) canjump = false;
            else if(!a)
            {
                Debug.Log("khe");
                counter--;
                counterText.text = counter.ToString();
            }
            else if(a)
            {
                canjump = true;
                counter = 3;
            }
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
    }

    private void FixedUpdate()
    {
        if (!paused && !dead)
        {
            if (!canjump) dir.y = 0f;
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
            rb.velocity += Vector2.up * dir.y * jumpSpeed;
            //timer -= Time.fixedDeltaTime;
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

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Pause();
    }

    public void Pause()
    {
        paused = !paused;
        if(paused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "caja")
        {
            updateLife(-1);
            Instantiate(particleDamage, transform.position, Quaternion.identity);
        }
    }
    
    public void updateLife(float value)
    {
        health += value;
        if (health <= 0)
        {
            dead = true;
        }
        else if (health > maxHealth) health = maxHealth;
        healthBar.fillAmount = health / maxHealth;
        healthSlider.value = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "star")
        {
            count++;
            Destroy(collision.gameObject, 0.5f);
            sc.Shake(6, 10, 5);
            updateLife(1);
        }
        else if (collision.gameObject.tag == "pasanivel")
        {
            timer = 2f;
        }
    }
}
