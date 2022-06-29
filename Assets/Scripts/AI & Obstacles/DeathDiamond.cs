using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDiamond : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private int pointsValue = 100;
    [SerializeField] private float rotateSpeed = 400;

    public GameObject GameManager { private get; set; }

    private GameObject target;
    private PointsTracker pointsTracker;
    private Rigidbody2D rb;

    private void Start() => InitializeData();
    private void FixedUpdate() {FacePlayer(); Thrust();}
    private void OnTriggerEnter2D(Collider2D collision) => CheckToSeeIfCollidedWithBullet(collision);

    private void CheckToSeeIfCollidedWithBullet(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            TakeDamage(collision.gameObject);
        }
    }
    private void TakeDamage(GameObject bullet)
    {
        if (!bullet.GetComponent<Projectile>().isEnemyProjectile) pointsTracker.AddPoints(pointsValue);
        Destroy(bullet);
        BreakApart();
    }
    private void BreakApart()
    {
        for (var i = 0; i <= 1; i++)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).parent = null;
        }
        Destroy(gameObject);
    }
    private void Thrust() {rb.velocity = transform.up * speed;}
    private void FacePlayer()
    {
        if (target == null) return;
        if (target.activeInHierarchy == false) Destroy(gameObject);
        Vector2 direction = target.transform.position - transform.position;
        direction = direction.normalized;
        float rotationAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotationAmount * rotateSpeed;
    }
    private void InitializeData()
    {
        GameManager = FindObjectOfType<ObjectTracker>().gameObject;
        target = GameManager.GetComponent<ObjectTracker>().player;
        pointsTracker = GameManager.GetComponent<PointsTracker>();
        rb = GetComponent<Rigidbody2D>();
    }
}
