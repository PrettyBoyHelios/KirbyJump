using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public int diff;
    public float clampAt; // 2.32 por 1080p

    public Vector3 simSpeed;

    /*
        Cloud Speed implicitly represents
        level, as difficulty changes cloud
        speed. All levels are played in
        the same scene, and top at 1.8f
        cloud speed.
     */
    public static float[] cloudSpeeds = {0.3f, 0.5f, 0.7f, 1.0f, 1.5f, 1.6f, 1.8f, 2.0f, 2.5f, 2.8f};
    private bool initialRight;
    // Start is called before the first frame update
    void Start()
    {
        //diff = 0;
        if(this.transform.position.x < 0){
            // Is located at left
            initialRight = true;
        }else{
            initialRight = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        this.simSpeed = this.transform.position;
        Vector3 newPos = new Vector3();
        if(initialRight){
            newPos = this.transform.position + (Vector3.right * cloudSpeeds[diff] * Time.deltaTime);
            this.transform.position = newPos;
            if(this.transform.position.x > clampAt){
                initialRight = false;
            }
        }else{
            newPos = this.transform.position + (Vector3.left * cloudSpeeds[diff] * Time.deltaTime);
            this.transform.position = newPos;
            if(this.transform.position.x <= -clampAt){
                initialRight = true;
            }
        }

        this.simSpeed = (newPos/Time.deltaTime - this.simSpeed) * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("Collision!");
        /* if(col.gameObject.name == "Kirby"){
            Vector3 kirbyPos = col.gameObject.transform.position;
            if(col.gameObject.transform.position.y > this.transform.position.y){
                col.gameObject.transform.position = new Vector3(this.transform.position.x, kirbyPos.y, kirbyPos.z);
            }
        }*/
    }

    public void setDiff(int diff){
        this.diff = diff;
    }
}
