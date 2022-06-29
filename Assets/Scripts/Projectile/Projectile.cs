using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D proj_Rigidbody;
    [SerializeField] float projectileSpeed = 3;
    public bool isEnemyProjectile;
    public AudioManager ParetnsAudioManager { private get; set; }

    private void Start()
    {
        proj_Rigidbody = GetComponent<Rigidbody2D>();
        ParetnsAudioManager.PlayShooting();
    }
    private void Update() => MoveProjectileForward();
    private void MoveProjectileForward() { proj_Rigidbody.velocity = transform.up * projectileSpeed;}
}
