using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GameManagerScript : MonoBehaviour
{
    private int partsPerRound = 10; //number of parts to spawn
    public float maxX, maxY, minX, minY;    //boundaries of game world
    public GameObject partsPrefab, enemyPrefab; //prefabs for the collectables and enemy

    // Start is called before the first frame update
    void Start()
    {
        //spawn in objects on game start
        for (int i = 0; i < partsPerRound; i++)
        {
            //instantiate between boundaries, avoid space station and player
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
