using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badGuyAI : MonoBehaviour
{
    Animator swordAnimator;
    public GameObject player;
    Animator otherAnimator;
    float speed = .02f;
    bool ded = false;
    public const float OGhealth = 3;
    float health = OGhealth;
    int knockCount = 0;
    public GameObject healthObj;


    enum dir
    {
        left = 1,
        right = -1,
    }
    void Start()
    {
        swordAnimator = GetComponent<Animator>();
        otherAnimator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ded && player.GetComponent<EyesBlink>().enabled)
        {
            if (knockCount <= 0) {
                if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < 4)
                    swordAnimator.SetTrigger("atk");

                if (player.transform.position.x > this.transform.position.x && Mathf.Abs(player.transform.position.x - this.transform.position.x)<15)
                {
                    transform.localScale = new Vector3((float)dir.right, 1, 1);
                    transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                    swordAnimator.SetBool("walkBool", true);
                }
                else if (player.transform.position.x < this.transform.position.x && Mathf.Abs(player.transform.position.x - this.transform.position.x) < 15)
                {

                    transform.localScale = new Vector3((float)dir.left, 1, 1);
                    transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
                    swordAnimator.SetBool("walkBool", true);
                }
                else
                {
                    swordAnimator.SetBool("walkBool", false);
                }
            }
            else
            {
                knockCount--;
                var knockDir = 1;
                if (player.transform.position.x > this.transform.position.x)
                    knockDir = -1;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockDir * 1000, 25), ForceMode2D.Force);
            }
        }
        else
        {
            swordAnimator.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player Weapon" && otherAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            knockCount = 10;
            health--;
            var per = health / OGhealth;
            if (per < 0)
                per = 0;
            healthObj.transform.localScale = new Vector2(per, healthObj.transform.localScale.y);
            if (health <= 0) { 
                ded = true;
                swordAnimator.enabled = false;
                this.GetComponent<EyesBlink>().enabled = false;
                for (int i = this.transform.GetChild(0).gameObject.transform.GetChild(0).transform.childCount - 1; i != -1; i--)
                {
                    GameObject child = this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                    Destroy(child, 4 + Random.Range(0.5f, 0.5f));
                    expodChild(child);
                }
                for (int i = this.transform.GetChild(0).gameObject.transform.childCount - 1; i != -1; i--)
                {
                    GameObject child = this.transform.GetChild(0).transform.GetChild(0).gameObject;
                    Destroy(child, 5 + Random.Range(0.5f, 0.5f));
                    expodChild(child);
                }
                doorGoDown.killedBois++;
                Destroy(this.gameObject, 6 + Random.Range(0.5f, 0.5f));
            }
        }
    }
    void expodChild(GameObject child)
    {
        //print(child);
        child.tag = null;
        Rigidbody2D childRigidBody = child.GetComponent<Rigidbody2D>();
        if(child.GetComponent<BoxCollider2D>())
            child.GetComponent<BoxCollider2D>().enabled = true;
        if (!childRigidBody)
            childRigidBody = child.AddComponent<Rigidbody2D>();
        var vex = 5 * new Vector2(Random.Range(-20, 20), Random.Range(-2, 2));
        //print(vex);
        child.transform.parent = null;
        childRigidBody.AddForce(vex, ForceMode2D.Impulse);
        //childRigidBody.gameObject.transform.position = new Vector2(10f,10f);
    }
}
