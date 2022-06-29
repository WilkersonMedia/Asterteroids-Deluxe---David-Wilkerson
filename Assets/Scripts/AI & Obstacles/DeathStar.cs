using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStar : MonoBehaviour
{
    [SerializeField] private float startingSpeed = 20;
    [SerializeField] private int pointsValue = 50;
    public GameObject GameManager { private get; set; }
    private PointsTracker pointsTracker;


    private void Start() => InitializeData();
    private void OnTriggerEnter2D(Collider2D collision) => CheckToSeeIfCollidedWithBullet(collision);

    private void InitializeData()
    {
        pointsTracker = GameManager.GetComponent<PointsTracker>();
        InitializeMovement();
        Invoke(nameof(BreakApart), 5);
    }
    private void CheckToSeeIfCollidedWithBullet(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            TakeDamage(collision.gameObject);
        }
    }
    private void TakeDamage(GameObject bullet)
    {
        if(!bullet.GetComponent<Projectile>().isEnemyProjectile) pointsTracker.AddPoints(pointsValue);
        Destroy(bullet);
        BreakApart();
    }
    private void BreakApart()
    {
        for (var i = 0; i <= 2; i++)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).parent = null;
        }
        Destroy(gameObject);
    }
    private void InitializeMovement()
    {
        Vector2 distance = new Vector3(0, 0, 0) - transform.position;
        distance = distance.normalized;
        this.GetComponent<Rigidbody2D>().AddForce(distance * startingSpeed);
    }
}
