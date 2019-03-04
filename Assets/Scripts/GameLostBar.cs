using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLostBar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameController gameController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("Collision!");
        if(col.gameObject.name == "Kirby"){
            gameController.restartGame();
        }
    }
}
