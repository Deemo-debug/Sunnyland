using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Animator anim;
    public Collider2D coll;
    
    public LayerMask ground;

    public float speed;
    public float jumpForce;
    public int cherry;

    public Text cherryNum;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        SwitchAnim();
    }

    //移动
    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");

        //角色移动
        rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
        anim.SetFloat("running", Mathf.Abs(faceDirection));

        //角色面向
        if(faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }

        //角色跳跃
        if(Input.GetButtonDown("Jump") && rb.velocity.y == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            anim.SetBool("jumping", true);
        }
    }

    //动作
    void SwitchAnim()
    {
        anim.SetBool("idle", false);
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    //收集
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            cherry += 1;
            cherryNum.text = cherry.ToString();
        }
    }

}
