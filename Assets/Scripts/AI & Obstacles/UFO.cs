using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private float timeToReachTarget = 10, timeBetweenShots = 100;
    [SerializeField] private Projectile projectile;
    [SerializeField] private int numOfAsteroidsToShootFirst = 3;
    [SerializeField] private int numOfTotalShotsToFire = 4;
    [SerializeField] private int pointsValueLarge = 200;
    [SerializeField] private int pointsValueSmall = 1000;
    [SerializeField] private bool isSmall;


    public GameObject GameManager { private get; set; }

    private float startTime;
    private Vector2 startingLocation, destination;
    private ObjectTracker objectTracker = null;
    private PointsTracker pointsTracker = null;
    private GameObject target = null;
    private float nextFire = 0;

    private void Start() => InitializeData();
    private void Update()
    {
        MoveAcrossScreen();
        if (TimeToShoot())
        {
            SetTarget();
            Shoot();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision) => CheckToSeeIfCollidedWithPlayerBullet(collision);

    private void CheckToSeeIfCollidedWithPlayerBullet(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            if (collision.gameObject.GetComponent<Projectile>().isEnemyProjectile) return;
            TakeDamage(collision.gameObject);
        }
    }
    private void InitializeData()
    {
        pointsTracker = GameManager.GetComponent<PointsTracker>();
        objectTracker = GameManager.GetComponent<ObjectTracker>();
        startingLocation = transform.position;
        destination = new Vector2(-transform.position.x, transform.position.y);
        startTime = Time.time;
    }
    private void MoveAcrossScreen()
    {
        float timeSinceStart = Time.time - startTime;
        float progressPecentage = timeSinceStart/timeToReachTarget;
        transform.position = Vector2.Lerp(startingLocation, destination, progressPecentage);
        if (progressPecentage >= 1) Destroy(gameObject);
    }
    private bool TimeToShoot()
    {
        return Time.time >= nextFire && numOfTotalShotsToFire > 0;
    }
    private void SetTarget()
    {
        switch (isSmall)
        {
            case true:
                target = objectTracker.player;
                break;
            case false:
                if (numOfAsteroidsToShootFirst > 0)
                {
                    if (FindAsteroid() == null)
                    {
                        target = objectTracker.player;
                        return;
                    }
                    numOfAsteroidsToShootFirst--;
                }
                else
                {
                    target = objectTracker.player;
                }
                numOfTotalShotsToFire--;
                break;
        }
    }
    private void Shoot()
    {
        if (target == null) return;
        Projectile newBullet = Instantiate(projectile, transform.position, transform.rotation, null);
        newBullet.isEnemyProjectile = true;
        newBullet.transform.up = target.transform.position - transform.position;
        newBullet.ParetnsAudioManager = GameManager.GetComponent<AudioManager>();
        nextFire = Time.time + timeBetweenShots;
    }
    private GameObject FindAsteroid()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 6);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Asteroids>() != null || hitCollider.gameObject.GetComponent<DeathStar>() != null)
            {
                target = hitCollider.gameObject;
                break;
            }
        }
        return target;
    }
    private void TakeDamage(GameObject bullet)
    {
        pointsTracker.AddPoints(CalculatePoints());
        Destroy(bullet);
        Destroy(this.gameObject);
    }
    private int CalculatePoints()
    {
        if (isSmall) return pointsValueSmall;
        else return pointsValueLarge;
    }
}
