using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    public int health = 3;

    public GameObject[] healthHUD;
    // Behaviour Control
    public bool isJumping;
    private bool isDoubleJumping;

    public GameController gameController;

    public GameObject currentCloud;
    private float currentOffset;



    // Start is called before the first frame update
    private Vector3 startDrag;
    private Vector3 endDrag;
    [Range(4000, 7000)]
    public float jumpForce;
    private bool mousePressed;
    private Vector3 jumpDirection;
    private float totalDistance;

    public Animator animatorManager;
    private AudioSource aSource;
    void Start()
    {
        jumpDirection = new Vector3();
        animatorManager = this.GetComponent<Animator>();
        isJumping = false;
        aSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        checkHealth();
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        if(currentCloud != null){
            fixKirbyPos2Cloud();
        }

        if(!isJumping){
            getDragDirection();
            if(rb.velocity.y < -100){
                gameController.restartGame();
            }
        }
        
    }

    private void checkHealth(){
        if(health <= 0){
            gameController.restartGame();
        }
    }
    private void fixKirbyPos2Cloud(){
        float newX = currentCloud.transform.position.x + currentOffset;
        Vector3 kPos = this.transform.position;
        this.transform.position = new Vector3(newX, kPos.y, kPos.z);
    }

    private void getDragDirection(){
        //Debug.Log("Player position");
        //Debug.Log(this.transform.position);
        if (Input.GetMouseButtonDown(0)){
            //Debug.Log("Mouse Down");
            mousePressed = true;
            Ray vRayStart = Camera.main.ScreenPointToRay(Input.mousePosition);
            startDrag = vRayStart.origin;
            animatorManager.SetBool("preparingToJump", true);
        }

        if (Input.GetMouseButtonUp(0)){
            if (mousePressed) {
                //Debug.Log("Mouse Released!");

                Ray vRayEnd = Camera.main.ScreenPointToRay(Input.mousePosition);

                endDrag = vRayEnd.origin;

                jumpDirection = endDrag - startDrag;
                totalDistance = jumpDirection.magnitude;
                jumpDirection = jumpDirection/totalDistance;

                Vector2 direction2D = new Vector2(-jumpDirection.x,-jumpDirection.y);

                
                //Debug.Log(direction2D);
                //Debug.Log("Total Distance: " + totalDistance + " vs " + jumpForce);

                this.GetComponent<Rigidbody2D>().AddForce(direction2D * MapForce(totalDistance));
                Cloud cloudObj = currentCloud.GetComponent<Cloud>() as Cloud;
                if(cloudObj!=null){
                    //Debug.Log("Added cloud velvect");
                    Vector2 auxvect = new Vector2(cloudObj.simSpeed.x, cloudObj.simSpeed.y);
                    this.GetComponent<Rigidbody2D>().velocity += auxvect;
                }
                

                mousePressed = false;
                aSource.Play();
                animatorManager.SetBool("preparingToJump", false);
                animatorManager.SetBool("isJumping", true);
                isJumping = true;
                currentCloud = null;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("Collision!");
        if(this.isJumping && col.transform.position.y > this.transform.position.y){
            isJumping = true;
            health--;
            healthHUD[health].SetActive(false);
            //Debug.Log("Crashed from below");
        }else{
            isJumping = false;
            //Debug.Log("kirby above CLOUD");
            this.currentCloud = col.gameObject;
            currentOffset = this.transform.position.x - currentCloud.transform.position.x;
            animatorManager.SetBool("preparingToJump", false);
            animatorManager.SetBool("isJumping", false);
        }
    }
    public float MapForce (float value){
        return (value - 0.0f) / (3.5f - 0.0f) * (this.jumpForce - 1500) + 1500;
    }
}
