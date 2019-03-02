using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public int diff;
    public float clampAt; // 2.32 por 1080p

    /*
        Cloud Speed implicitly represents
        level, as difficulty changes cloud
        speed. All levels are played in
        the same scene, and top at 1.8f
        cloud speed.
     */
    public static float[] cloudSpeeds = {0.0f, 0.5f, 1.0f, 1.2f, 1.5f, 1.8f};
    // Start is called before the first frame update
    void Start()
    {
        diff = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
