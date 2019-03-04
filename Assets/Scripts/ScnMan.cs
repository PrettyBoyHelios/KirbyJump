using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScnMan : MonoBehaviour
{
    // Start is called before the first frame update
    public int nextSceneIndex;

    public void loadNext(){
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void loadMainScreen(){
        SceneManager.LoadScene(0);
    }
    
}
