using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public GameObject GameManager { private get; set; }

    [SerializeField] private float rotateSpeed = 1, thrustSpeed = 2;
    [SerializeField] private GameObject projectile, projectileSpawnLocation, shield;
    [SerializeField] private int maxProjectilesOnScreen = 4;
    [SerializeField] private GameObject explosionFX = null;
    [SerializeField] private GameObject thrustFX = null;
    [SerializeField] private float timeToKillWhenOffscreen = 6;

    private InputControls inputControls;
    private Rigidbody2D player_rigidbody;
    private GameObject[] bullets;
    private bool isInvincible = false;
    private float offScreenTimer = 0;
    private Renderer _renderer = null;
    private AudioManager audioManager;
    private LivesTracker livesTracker;

    private void Awake() { inputControls = new InputControls(); }
    private void Start() => InitializeData();
    private void OnEnable() => SubscribeToControlEvents();
    private void OnDisable() => UnsubscribeToControlEvents();
    private void Update() => MonitorIfOffScreen();

    private void FixedUpdate() => ListenForInput();
    private void OnTriggerEnter2D(Collider2D collision) => KillPlayer(collision);

    private void MonitorIfOffScreen()
    {
        if (!_renderer.isVisible) RunOffScreenTimer(true);
        else RunOffScreenTimer(false);
    }
    private void RespawnPlayer()
    {
        transform.position = new Vector2(0, 0);
        gameObject.SetActive(true);
        inputControls.Enable();
        shield.GetComponent<Shield>().RestartShieldLifetime();
        Invoke(nameof(ToggleInvincible), 3.0f);
    }
    private void ListenForInput()
    {
        float rotationInput = inputControls.ActionMap.Turn.ReadValue<float>();
        float thrustInput = inputControls.ActionMap.Thrust.ReadValue<float>();
        RotatePlayer(rotationInput);
        if (thrustInput >= 0.5f) player_rigidbody.AddForce(transform.up * thrustSpeed);
    }
    private void RotatePlayer(float rotationInput)
    {
        transform.Rotate(rotateSpeed * -rotationInput * Vector3.forward);
    }
    private void Shoot(InputAction.CallbackContext obj)
    {
        for (var i = 0; i < bullets.Length; i++)
        {
            if(bullets[i] == null)
            {
                bullets[i] = Instantiate(projectile, projectileSpawnLocation.transform.position, transform.rotation, null);
                bullets[i].GetComponent<Projectile>().ParetnsAudioManager = GameManager.GetComponent<AudioManager>();
                return;
            }
        }

    }
    private void ShieldIsToggled(InputAction.CallbackContext obj)
    {
        if (shield.GetComponent<Shield>().TimeRemaining > 0)
        {
            shield.SetActive(!shield.activeInHierarchy);
            ToggleInvincible();
        }
        else print("Shield Is Depleted");
    }
    public void ToggleInvincible() { isInvincible = !isInvincible; }
    private bool WasHitWithPlayerBullet(GameObject other)
    {
        if (other.GetComponent<Projectile>() == null) return false;
        return !other.GetComponent<Projectile>().isEnemyProjectile;
    }
    private void ToggleThrustFX(InputAction.CallbackContext obj)
    {
        thrustFX.SetActive(!thrustFX.activeInHierarchy);
        audioManager.ToggleThrust(thrustFX.activeInHierarchy);
    }
    private void KillPlayer(Collider2D collision)
    {
        if (WasHitWithPlayerBullet(collision.gameObject) || isInvincible) return;
        bool isDead = livesTracker.Lives < 0;
        shield.SetActive(false);
        thrustFX.SetActive(false);
        audioManager.PlayDead();
        livesTracker.AddLife(-1);
        inputControls.Disable();
        ToggleInvincible();
        gameObject.SetActive(false);
        Instantiate<GameObject>(explosionFX, transform.position, transform.rotation, null);
        if (isDead) GameManager.GetComponent<GameManager>().GameOver();
        else Invoke(nameof(RespawnPlayer), 3.0f);
    }
    private void EscPressed(InputAction.CallbackContext obj) => GameManager.GetComponent<GameManager>().ChangeScene("Esc");
    private void RunOffScreenTimer(bool offScreen)
    {
        switch (offScreen)
        {
            case true:
                offScreenTimer += Time.deltaTime;
                if (offScreenTimer > timeToKillWhenOffscreen) KillPlayer(null);
                break;
            case false:
                if (offScreenTimer == 0) return;
                offScreenTimer = 0;
                break;
        }
        Debug.Log("Player is offScreen: " + offScreen);

    }
    private void SubscribeToControlEvents()
    {
        inputControls.Enable();
        inputControls.ActionMap.Shield.performed += ShieldIsToggled;
        inputControls.ActionMap.Shield.canceled += ShieldIsToggled;
        inputControls.ActionMap.Shoot.performed += Shoot;
        inputControls.ActionMap.Thrust.performed += ToggleThrustFX;
        inputControls.ActionMap.Thrust.canceled += ToggleThrustFX;
        inputControls.ActionMap.BackButton.performed += EscPressed;
    }
    private void UnsubscribeToControlEvents()
    {
        inputControls.Disable();
        inputControls.ActionMap.Shield.performed -= ShieldIsToggled;
        inputControls.ActionMap.Shield.canceled -= ShieldIsToggled;
        inputControls.ActionMap.Shoot.performed -= Shoot;
        inputControls.ActionMap.Thrust.performed -= ToggleThrustFX;
        inputControls.ActionMap.Thrust.canceled -= ToggleThrustFX;
        inputControls.ActionMap.BackButton.performed -= EscPressed;
    }
    private void InitializeData()
    {
        player_rigidbody = GetComponent<Rigidbody2D>();
        bullets = new GameObject[maxProjectilesOnScreen];
        GameManager = FindObjectOfType<LivesTracker>().gameObject;
        _renderer = GetComponent<Renderer>();
        audioManager = GameManager.GetComponent<AudioManager>();
        livesTracker = GameManager.GetComponent<LivesTracker>();
        shield.GetComponent<Shield>().RestartShieldLifetime();
    }
}
