using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ninjaDoStuff : MonoBehaviour
{
    // Start is called before the first frame update
    Animator swordAnimator;
    Rigidbody2D rid;
    bool ded = false;
    int knockCount = 0;
    int knockDir = 1;
    public const float OGhealth = 50;
    float health = OGhealth;
    public GameObject healthObj;
    bool onFloor = true;
    bool letGo = true;
    const int intialJumpCount = 20;
    int jumpCount = intialJumpCount;
    int atkCoolDown = 0;
    enum dir
    {
        left = 1,
        right = -1,
    }
    void Start()
    {
        swordAnimator = GetComponent<Animator>();
        rid = GetComponent<Rigidbody2D>();
        rid.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!ded)
        {
            if (knockCount <= 0)
            {
                if (Input.GetKeyDown("space") && atkCoolDown<0)
                {
                    swordAnimator.SetTrigger("atk");
                    atkCoolDown = 100;
                }
                atkCoolDown--;


                if (Input.GetKey("right"))
                {
                    transform.localScale = new Vector3((float)dir.right, 1, 1);
                    transform.position = new Vector3(transform.position.x + .1f, transform.position.y, transform.position.z);
                    swordAnimator.SetBool("walkBool", true);
                }
                else if (Input.GetKey("left"))
                {

                    transform.localScale = new Vector3((float)dir.left, 1, 1);
                    transform.position = new Vector3(transform.position.x - .1f, transform.position.y, transform.position.z);
                    swordAnimator.SetBool("walkBool", true);
                }

                else
                {
                    
                    swordAnimator.SetBool("walkBool", false);
                }
                if (Input.GetKey("up") && (onFloor || !letGo) && jumpCount > 0)
                {
                    onFloor = false;
                    letGo = false;
                    jumpCount--;
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 150), ForceMode2D.Force);
                    swordAnimator.SetBool("walkBool", false);
                }
                else
                {
                    letGo = true;
                }
                if(Mathf.Abs(this.GetComponent<Rigidbody2D>().velocity.y) <.001)
                {
                    onFloor = true;
                    jumpCount = intialJumpCount;
                }
            }
            else
            {
                knockCount--;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockDir * 1000, 25), ForceMode2D.Force);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Animator otherAnimator = collision.gameObject.GetComponentInParent<Animator>();
        print("play hit "+collision.gameObject.tag);
        if (collision.gameObject.tag == "floor")
        {
            onFloor = true;
            //jumpCount = 10;
        }
        if (collision.gameObject.tag == "Enemy Weapon" && otherAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack2") && !swordAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            if (collision.gameObject.transform.position.x > this.transform.position.x)
                knockDir = -1;
            else
                knockDir = 1;
            knockCount = 10;
            health--;
            var per = health / OGhealth;
            if (per < 0)
                per = 0;
            healthObj.transform.localScale = new Vector2(per, healthObj.transform.localScale.y);
            if (health <= 0)
            {
                ded = true;
                swordAnimator.SetTrigger("ded");

                this.GetComponent<EyesBlink>().enabled = false;
            }
            
        }

    }
}
