using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class ObjectTracker : MonoBehaviour
{
    public GameObject player = null;
    public int asteroidCount = 0;

    private void Start() => PlayerNullCheck();

    private void PlayerNullCheck()
    {
        if (player == null) Debug.Log("Player returned null");
    }
    public int GetAsteroidCount()
    {
        return asteroidCount;
    }
    public void AddAsteroid()
    {
        asteroidCount++;
    }
    public void SubtractAsteroid()
    {
        asteroidCount--;
    }
}
