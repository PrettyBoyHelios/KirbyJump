using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Prefabs
    public int highScore;
    public GameObject cloudPF;
    public GameObject normalSky;

    [SerializeField]
    public GameObject[] background;
    private bool isFirstBg;


    // Game Info
    [SerializeField]
    public Text scoreField;
    public int cloudQty;
    [Range(1, 10)]
    public int maxCloudGenerationHeight;

    private int levelInfo;
    [Range(1, 10)]
    public int levelRate;

    private List<GameObject> currentClouds;
    private Vector3 mainCameraPos;

    Camera mainCam;
    public Transform resetBlock;

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

        Instantiate(background[0], new Vector3(0.0f, 0.4f, 0.0f), Quaternion.identity);
        Instantiate(background[1], new Vector3(0.0f, 0.4f + 8.0f, 0.0f), Quaternion.identity);
        isFirstBg = true;
        scoreField.text = "0 Score";
        highScore = 0;
        levelInfo = 0;
        levelRate = 10;
    }

    // Update is called once per frame
    void Update()
    {
        updateLevelInfo();
        playerHeight = playerTransform.position.y;
        //Debug.Log("Height: " + playerHeight);
        isPlayerJumping = playerObject.GetComponent<PlayerController>().isJumping;

        fixBackground();
        
        

        // Check for cloud consistency
        //Debug.Log(isPlayerJumping);
        if(!isPlayerJumping){
            removePastClouds();
            fillClouds();
            checkHighscore();
            
            repositionResetBlock();
        }
        repositionCamera();

        if(playerHeight < (float) highScore - 20f){
            Debug.Log("Restart by exit condition");
            restartGame();
        }

        scoreField.text = "Score: " + highScore.ToString() + "\nLevel: " + (this.levelInfo + 1);
    }

    private float clampToScreen(float value){
        return Mathf.Clamp(value, -clampAt, clampAt);
    }
    private void checkHighscore(){
        if(highScore < (int)playerHeight){
            highScore = (int)playerHeight;
        }
    }

    private void fillClouds(){
        int[] cloudsByIndex = new int[this.maxCloudGenerationHeight];
        
        int y = 0;
        float x = 0.0f;
        // TODO: check for clouds below camera
        while(currentClouds.Count < this.cloudQty){
            y = this.random.Next(1, maxCloudGenerationHeight);
            cloudsByIndex[y%(this.maxCloudGenerationHeight)]++;
            x = (float)this.random.Next(-3,3);
            int randomOp = random.Next(101);
            if(randomOp%2==0){
                x = x+= (float)this.random.NextDouble(); 
            }else{
                x = x-= (float)this.random.NextDouble(); 
            }
            
            x = clampToScreen(x);
            GameObject newCloud = Instantiate(cloudPF, new Vector3(x, playerHeight + y, -1.0f),transform.rotation);
            currentClouds.Add(newCloud);
            int diff = this.random.Next(0, this.levelInfo + 2);
            Cloud cloudObj = newCloud.GetComponent<Cloud>() as Cloud;
            cloudObj.setDiff(diff);
        }
        
    }


    private void fixBackground(){
        //Debug.Log("Fixing BG!");
        if(!isPlayerJumping){
            //Debug.Log("Checking!");
            if(playerHeight > background[0].transform.position.y + 6.0f){
                if(isFirstBg){
                    // Keeps reference in order to safely
                    // destroy GameObject
                    GameObject aux = background[0];
                    Vector3 auxVect = background[1].transform.position;
                    background[0] = Instantiate(background[1], auxVect, Quaternion.identity);
                    Destroy(aux);
                    Vector3 bgOffset = new Vector3(auxVect.x, auxVect.y + 6.0f, auxVect.z);
                    background[1].transform.position = bgOffset;
                    isFirstBg = false;
                }else{
                    Vector3 auxVect = background[1].transform.position;
                    background[0].transform.position = auxVect;
                    Vector3 bgOffset = new Vector3(auxVect.x, auxVect.y + 6.0f, auxVect.z);
                    background[1].transform.position = bgOffset;
                }
            }
        }
    }

    private void removePastClouds(){
        //indexToDelete.Clear();
        List<GameObject> delClouds = new List<GameObject>();
        //Debug.Log(currentClouds.Count);
        for(int i = 1; i<currentClouds.Count;i++){
            if(currentClouds[i].transform.position.y < playerHeight - 1.0f){
                indexToDelete.Add(i);
                delClouds.Add(currentClouds[i]);
            }
        }
        while(delClouds.Count != 0){
            GameObject aux = delClouds[0];
            currentClouds.Remove(aux);
            delClouds.Remove(aux);
            Destroy(aux);
        }
        // Remove from list
        // foreach(var i in indexToDelete){
        //     GameObject auxCloud = currentClouds[i];
        //     currentClouds.RemoveAt(i);
        //     Destroy(auxCloud);
        // }
        //Debug.Log(currentClouds.Count);
    }

    private void repositionCamera(){
        float offset = playerHeight + 1.4f;
        offset = Mathf.Clamp(offset, resetBlock.position.y +10.6f, playerHeight +1.4f);
        //Vector3 oldPos = mainCam.transform.position;
        Vector3 newPos = new Vector3(0.0f, offset, -10.0f);
        mainCam.transform.position = newPos;
        //resetBlock.position = new Vector3(0.0f, mainCam.transform.position.y - 12.0f, -2.0f);
    }

    private void repositionResetBlock(){
        float offset = playerHeight -13.4f;
        //Vector3 oldPos = mainCam.transform.position;
        Vector3 newPos = new Vector3(0.0f, offset, -2.0f);
        resetBlock.position = newPos;
    }

    private void updateLevelInfo(){
        if(levelRate != 0){
            this.levelInfo = this.highScore/levelRate;
            this.levelInfo = Mathf.Clamp(this.levelInfo, 0, 9);
        }
        
    }
    public void restartGame(){
        SceneManager.LoadScene(2);
    }
}
