using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Behaviour Control
    private bool isJumping;
    private bool isDoubleJumping;


    // Start is called before the first frame update
    private Vector3 startDrag;
    private Vector3 endDrag;
    public float jumpForce;
    private bool mousePressed;
    private Vector3 jumpDirection;
    private float totalDistance;

    public Animator animatorManager;
    void Start()
    {
        jumpDirection = new Vector3();
        animatorManager = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Debug.Log("Mouse Down");
             mousePressed = true;
             Ray vRayStart = Camera.main.ScreenPointToRay(Input.mousePosition);
             startDrag = vRayStart.origin;
         }

         if (Input.GetMouseButtonUp(0)){
 
             if (mousePressed) {
                Debug.Log("Mouse Released!");
 
                Ray vRayEnd = Camera.main.ScreenPointToRay(Input.mousePosition);
 
                endDrag = vRayEnd.origin;
 
                jumpDirection = endDrag - startDrag;
                totalDistance = jumpDirection.magnitude;
                jumpDirection = jumpDirection/totalDistance;
 
                //GameObject fireInstance = Instantiate(prefab, mouseEndPosition, Quaternion.identity) as GameObject;
 
                Vector2 direction2D = new Vector2(-jumpDirection.x,-jumpDirection.y);

                Debug.Log(direction2D);
                Debug.Log(totalDistance);

                
                this.GetComponent<Rigidbody2D>().AddForce(direction2D * jumpForce);
 
                 mousePressed = false;
             }
 
         }

    }
}
