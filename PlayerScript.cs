using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour

{


    float currentTime = 0f;
    float startingTime = 60f;
    [SerializeField] Text countdownText; //modifications for adding timer

    public float speed;


    public Text countText;
    public Text winText;
    public Text loseText;
    public Text livesText;

    public AudioSource musicSource;
    public AudioClip winMusic;
    public AudioClip bgMusic;
    public AudioClip jumpMusic;
    public AudioClip coinMusic;


    private bool facingRight = true;
    



    private Rigidbody2D rd2d;
    private int count;
    private int lives;
    private int water; 

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;

        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        water = 0;
        count = 0;
        lives = 3;
        winText.text = "";
        loseText.text = "";
        SetCountText();
        SetLivesText();
        Flip();
        musicSource.clip = bgMusic;
        musicSource.Play();

        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
       
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        //idle
        if (hozMovement == 0)
        {
            anim.SetBool("walking", false);
        }
        else
        {
            anim.SetBool("walking", true); //run
        }
        if (verMovement > 0)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }//flipping

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
          currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");


            if (currentTime <= 0) //timer game over
        {
            currentTime = 0;

        }
      
            
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            musicSource.PlayOneShot(coinMusic, 0.7F);

        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetLivesText();
        }
        { if (other.gameObject.CompareTag("Water")) //water respawn
        {
            
            water = water + 1;

        }
        if (water == 1)
        {
            transform.position = new Vector2(72.18f, 1.64f);
                water = 0;
                
        }
    }
        if (count == 4 && other.gameObject.CompareTag("Coin"))
        {
            transform.position = new Vector2(69.64f, -.41f);
            
            lives = 3;
            SetLivesText();
            
        }

        if (lives == 0 || count == 11)
        {
            Destroy(this);
          

        }

       
        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
               
                musicSource.PlayOneShot(jumpMusic, 0.6F);
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
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 9)
        {
            winText.text = "You win! Game created by Rebecca Navas.";
            musicSource.clip = winMusic;
            musicSource.Play();
            Destroy(this);
        }

    }
    void SetLivesText()
    {

        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            loseText.text = "You lose! Better luck next time...";
        }

    }
}


