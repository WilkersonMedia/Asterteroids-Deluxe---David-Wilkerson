using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class SpawnController : MonoBehaviour
{
    [SerializeField] Asteroids asteroid;
    [SerializeField] UFO UFO;
    [SerializeField] UFO smallUFO;
    [SerializeField] DeathStar deathStar;

    private int currentLevel = 1;
    private float nextUFOSpawnTime = 15;
    private float nextStarSpawnTime = 25;
    private PointsTracker pointsTracker;
    private GameObject player;

    private void Start() => InstantiateData();

    private void InstantiateData()
    {
        player = GetComponent<ObjectTracker>().player;
        nextUFOSpawnTime = Time.time + nextUFOSpawnTime;
        nextStarSpawnTime = Time.time + nextStarSpawnTime;
        pointsTracker = gameObject.GetComponent<PointsTracker>();
        SpawnAsteroids();
    }
    private void Update()
    {
        if(player == null || !player.activeInHierarchy) return;
        
        //StartSpawning
        if(Time.time >= nextUFOSpawnTime) SpawnUFO();
        if(Time.time >= nextStarSpawnTime) SpawnDeathstar();
    }

    public void SpawnAsteroids()
    {
        for(var i = 0; i < currentLevel + 3; i++)
        {
            Asteroids newAsteroid = Instantiate(asteroid, GenerateSpawnLocation(false), transform.rotation, null);
            newAsteroid.GameManager = gameObject;
        }
        currentLevel++;
    }
    public void SpawnUFO()
    {
        nextUFOSpawnTime = Time.time + 15 + Random.Range(-5, 5);
        UFO newUFO = Instantiate(GetUFOType(), GenerateSpawnLocation(true), transform.rotation, null);
        newUFO.GameManager = gameObject;
    }
    public void SpawnDeathstar()
    {
        nextStarSpawnTime = Time.time + 15 + Random.Range(-5, 5);
        DeathStar newDeathstar = Instantiate(deathStar, GenerateSpawnLocation(false), transform.rotation, null);
        newDeathstar.GameManager = gameObject;
    }
    private Vector2 GenerateSpawnLocation(bool disableVertical)
    {
        float cardinalDirection;
        if (disableVertical) cardinalDirection = Random.Range(3, 4);
        else cardinalDirection = Random.Range(1, 4);
        return cardinalDirection switch
        {
            1 => new Vector2(Random.Range(-7f, 7f), 5f), //north
            2 => new Vector2(Random.Range(-7f, 7f), -5f), // south
            3 => new Vector2(7f, Random.Range(-4f, 4f)), // east
            4 => new Vector2(-7f, Random.Range(-4f, 4f)), // west
            _ => new Vector2(Random.Range(-7f, 7f), 5f), //defualt
        };
    }
    private UFO GetUFOType()
    {
        if (pointsTracker.GetPoints() < 10000) return UFO;
        switch (pointsTracker.GetPoints() % 20 == 0)
        {
            case true:
                return UFO;
            case false:
                return smallUFO;
        }
    }
}
