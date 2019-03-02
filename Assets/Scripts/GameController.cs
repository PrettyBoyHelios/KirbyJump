using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    // Prefabs
    public GameObject cloudPF;
    public GameObject normalSky;

    // Game Info
    public int cloudQty;
    private List<GameObject> currentClouds;
    private Vector3 mainCameraPos;

    // Utils
    public float clampAt;
    public GameObject playerObject;
    private Transform playerTransform;
    private System.Random random;

    void Start()
    {
        this.currentClouds = new List<GameObject>();
        this.random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        int y = 0;
        float x = 0.0f;
        // TODO: check for clouds below camera
        while(currentClouds.Count <= this.cloudQty){
            y = this.random.Next(1, 5);
            x = (float)this.random.Next(-3,3);
            x = x+= (float)this.random.NextDouble(); 
            x = clampToScreen(x);
            GameObject newCloud = Instantiate(cloudPF, new Vector3(x, y),transform.rotation);
            currentClouds.Add(newCloud);
            Debug.Log(currentClouds.Count);
        }

        // Check for cloud consistency
        for(int i = 1; i<currentClouds.Count;i++){
            
        }

        
    }

    private float clampToScreen(float value){
        return Mathf.Clamp(value, -clampAt, clampAt);
    }
}
