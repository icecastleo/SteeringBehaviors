using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public Vector2 mapSize;

    public GameObject grass;
    public GameObject animal;
    public GameObject obstacle;
    public GameObject food;

    public int animalNumber;
    public int obstacleNumber;
    public int foodNumber;


    protected GameManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    // Use this for initialization
    void Start()
    {
        GameObject tiles = new GameObject("Tiles");
        GameObject obstacles = new GameObject("Obstacles");
        GameObject foods = new GameObject("Foods");
        GameObject boars = new GameObject("Boars");

        for (float i = -1; i <= mapSize.x; i++)
        {
            for(float j = -1; j <= mapSize.y; j++)
            {
                Instantiate(grass, new Vector3(i, j, 0f), Quaternion.identity, tiles.transform);

                if(i == -1 || j == -1 || i == mapSize.x || j == mapSize.y)
                {
                    Instantiate(obstacle, new Vector3(i, j, 0f), Quaternion.identity, obstacles.transform);
                }
            }
        }

        for (int i = 0; i < animalNumber; i++)
        {
            Instantiate(animal, new Vector3(Random.Range(0f, mapSize.x), Random.Range(0f, mapSize.y), 0f), Quaternion.identity, boars.transform);
        }

        for(int i = 0; i < obstacleNumber; i++)
        {
            Instantiate(obstacle, new Vector3(Random.Range(0f, mapSize.x), Random.Range(0f, mapSize.y), 0f), Quaternion.identity, obstacles.transform);
        }

        for (int i = 0; i < foodNumber; i++)
        {
            Instantiate(food, new Vector3(Random.Range(0f + 3f, mapSize.x -3f), Random.Range(0f +3f, mapSize.y - 3f), 0f), Quaternion.identity, foods.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
