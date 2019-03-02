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
    [Range(1, 10)]
    public int maxCloudGenerationHeight;

    private List<GameObject> currentClouds;
    private Vector3 mainCameraPos;

    Camera mainCam;

    // Utils
    public float clampAt;
    public GameObject playerObject;
    public Transform playerTransform;
    private System.Random random;

    private float playerHeight;
    private List<int> indexToDelete;
    private bool isPlayerJumping;

    void Start()
    {
        this.currentClouds = new List<GameObject>();
        this.indexToDelete = new List<int>();
        this.random = new System.Random();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        playerHeight = playerTransform.position.y;
        isPlayerJumping = playerObject.GetComponent<PlayerController>().isJumping;
        int y = 0;
        float x = 0.0f;
        // TODO: check for clouds below camera
        while(currentClouds.Count <= this.cloudQty){
            
            y = this.random.Next(1, maxCloudGenerationHeight);
            x = (float)this.random.Next(-3,3);
            x = x+= (float)this.random.NextDouble(); 
            x = clampToScreen(x);
            GameObject newCloud = Instantiate(cloudPF, new Vector3(x, playerHeight + y, -1.0f),transform.rotation);
            currentClouds.Add(newCloud);
            Debug.Log(currentClouds.Count);
        }

        // Check for cloud consistency
        Debug.Log(isPlayerJumping);
        if(!isPlayerJumping){
            removePastClouds();
            repositionCamera();
        }
        

        
    }

    private float clampToScreen(float value){
        return Mathf.Clamp(value, -clampAt, clampAt);
    }

    private void FillClouds(){

    }

    private void removePastClouds(){
        indexToDelete.Clear();
        //Debug.Log(currentClouds.Count);
        for(int i = 1; i<currentClouds.Count;i++){
            if(currentClouds[i].transform.position.y < playerHeight - 5){
                indexToDelete.Add(i);
            }
        }
        // Remove from list
        foreach(var i in indexToDelete){
            GameObject auxCloud = currentClouds[i];
            currentClouds.RemoveAt(i);
            Destroy(auxCloud);
        }
        //Debug.Log(currentClouds.Count);
    }

    private void repositionCamera(){
        float offset = playerHeight + 1.4f;
        Vector3 oldPos = mainCam.transform.position;
        Vector3 newPos = new Vector3(0.0f, offset, -10.0f);
        mainCam.transform.position = newPos;
    }
}
