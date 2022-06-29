using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    [SerializeField] private GameObject Asteroid;
    [SerializeField] private float startingSpeed = 50;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private int PointsValueLarge = 20;
    [SerializeField] private int PointsValueMedium = 50;
    [SerializeField] private int PointsValueSmall = 100;

    public GameObject GameManager {private get; set; }

    private ObjectTracker objectTracker;
    private PointsTracker pointsTracker;
    private enum Size {Large, Medium, Small};
    private Size asteroidSize = Size.Large;
    private Size nextSize;
    private GameObject newAsteroid;
    private float timeOutOfBounds = 0;
    private float sizeDenominator = 1;
    private bool startRotate = false;

    private void Start()
    {
        InitializeData();
        AddToObjectTracker();
        SwitchSize();
        InitializeMovement();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null) TakeDamage(collision.gameObject);
        else return;
        if (!collision.gameObject.GetComponent<Projectile>().isEnemyProjectile) CalculatePoints();
    }
    private void Update() { Rotate(); DestroyIfOutOfBoundsTooLong(); }

    private void InitializeData()
    {
        objectTracker = GameManager.GetComponent<ObjectTracker>();
        pointsTracker = GameManager.GetComponent<PointsTracker>();
    }
    private void DestroyIfOutOfBoundsTooLong()
    {
        if (gameObject.GetComponent<Renderer>().isVisible) timeOutOfBounds = 0;
        else timeOutOfBounds += Time.deltaTime;
        if (timeOutOfBounds > 5) { RemoveFromObjectTracker(); Destroy(gameObject); }
    }
    private void TakeDamage(GameObject bullet)
    {
        if (asteroidSize != Size.Small) BreakApart();
        RemoveFromObjectTracker();
        Destroy(bullet);
        Destroy(gameObject);
    }
    private void BreakApart()
    {
        for (var i = 0; i < 2; i++)
        {
            newAsteroid = Instantiate(Asteroid, this.transform.position, this.transform.rotation, null);
            newAsteroid.GetComponent<Asteroids>().GameManager = GameManager;
            newAsteroid.GetComponent<Asteroids>().asteroidSize = nextSize;
        }
    }
    private void InitializeMovement()
    {
        if (asteroidSize == Size.Large)
        {
            Vector2 distance = new Vector3(Random.Range(-3,3), Random.Range(-3,3), 0) - transform.position;
            distance = distance.normalized;
            this.GetComponent<Rigidbody2D>().AddForce(distance * startingSpeed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)) * startingSpeed);
        }
        startRotate = true;

    }
    private void SwitchSize()
    {
        switch (asteroidSize)
        {
            case Size.Small:
                sizeDenominator = 2;
                nextSize = default;
                break;
            case Size.Medium:
                sizeDenominator = 1.5f;
                nextSize = Size.Small;
                break;
            case Size.Large:
                nextSize = Size.Medium;
                break;
            default:
                nextSize = Size.Medium;
                break;
        }
        transform.localScale = transform.localScale / sizeDenominator;
    }
    private void AddToObjectTracker()
    {
        if (objectTracker == null)
        {
            Debug.Log("Couldn't find Object Tracker");
            return;
        }
        objectTracker.AddAsteroid();
    }
    private void RemoveFromObjectTracker()
    {
        objectTracker.asteroidCount--;
        if (objectTracker.GetAsteroidCount() > 0) return;
        GameManager.GetComponent<SpawnController>().SpawnAsteroids();
    }
    private void Rotate()
    {
        if (!startRotate) return;
        transform.Rotate( 0, 0, Time.deltaTime * rotationSpeed);
    }
    private void CalculatePoints()
    {
        switch (asteroidSize)
        {
            case Size.Small:
                pointsTracker.AddPoints(PointsValueSmall);
                break;
            case Size.Medium:
                pointsTracker.AddPoints(PointsValueMedium);
                break;
            case Size.Large:
                pointsTracker.AddPoints(PointsValueLarge);
                break;
            default:
                pointsTracker.AddPoints(PointsValueLarge);
                break;
        }
    }
}
