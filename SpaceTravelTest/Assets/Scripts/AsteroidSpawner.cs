using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

    // This will spawn a cubic amount of asteroids ( asteroidCount in each dimension )

    public int asteroidCount = 3;
    public Asteroid[] asteroid;
    public Vector3 maxGridSpacing;
    public Vector3 minGridSpacing;


    void Awake()
    {
        if (asteroid.Length < 1)
            Debug.Log("Add some asteroid!");

    }



    void Start()
    {
        PopulateAsteroidField();
    }



    void PopulateAsteroidField()
    {
        int i = 0;
        if (maxGridSpacing == Vector3.zero)
        {
            Debug.Log(maxGridSpacing + " too small!");
            return;
        }

        for(int x = 0 - asteroidCount / 2; x < asteroidCount; x++)
        {
            for(int z = 0 - asteroidCount / 2; z < asteroidCount; z++)
            {
                for(int y = 0 - asteroidCount / 2; y < asteroidCount; y++)
                {
                    int rnd = Random.Range(0, asteroid.Length); //get a random asteroid from the asteroid array
                    //position the asteroid on a random offset from the grid.
                    float xPos = Random.Range(minGridSpacing.x, maxGridSpacing.x) * x;
                    float yPos = Random.Range(minGridSpacing.y, maxGridSpacing.y) * y;
                    float zPos = Random.Range(minGridSpacing.z, maxGridSpacing.z) * z;
                    Vector3 pos = new Vector3(xPos, yPos, zPos);

                    //instantiate with generics - long way
                    Asteroid ast = Instantiate<Asteroid>(asteroid[rnd]);
                    ast.transform.position = pos;
                    ast.transform.parent = transform;
                }
            }
        }


    }
}
