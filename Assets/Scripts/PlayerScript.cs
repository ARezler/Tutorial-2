using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public float speed;
    public Text winText;
    public Text score;
    public Text livesText;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    private int scoreValue = 0;
    private int lives;
    private bool levelTwo;
    private bool facingRight = true;
    private bool isOnGround;
    Animator anim;
    
    

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Coins: " + scoreValue.ToString();
        winText.text = "";
        lives = 3;
        livesText.text = "Lives: " + lives.ToString ();
        levelTwo = false;
        anim = GetComponent<Animator>();
        isOnGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        //If the GameObject is not jumping, send that the Boolean “Jump” is false to the Animator. The jump animation does not play.
        if (isOnGround == false)
        {
            anim.SetBool("isOnGround", false);
        }
        //The GameObject is jumping, so send the Boolean as enabled to the Animator. The jump animation plays.
        if (isOnGround == true)
        {
            anim.SetBool("isOnGround", true);
        }
        if (lives == 0)
       {
           winText.text = "Sorry, you lost";
           Destroy(gameObject);
       }
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip(); 
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Coins: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if (scoreValue == 4 && levelTwo == false)
        {
            transform.position = new Vector3 (0f, 20f, 0f);
            levelTwo = !false;
            lives = 3;
            livesText.text = "Lives: " + lives.ToString ();
        }
        if (scoreValue >= 8)
        {
            winText.text = "You Won, Congratulations from Ava Rezler!";
            musicSource.clip = musicClipOne;
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
        if (collision.collider.tag == "Enemy")
        {
            lives -=1;
            livesText.text = "Lives: " + lives.ToString ();
            Destroy(collision.collider.gameObject);
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
    
    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
        
}
