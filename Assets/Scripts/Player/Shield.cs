using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float lifetime = 12;
    private SpriteRenderer spriteRenderer;
    public float TimeRemaining { get; private set; }
    private PlayerActions player;
    private AudioSource audioSource;

    private void Awake() => InstantiateData();

    void Update()
    {
        if (TimeRemaining <= 0) return;
        TimeRemaining -= Time.deltaTime;
        FadeShieldOverTime();
    }

    private void FadeShieldOverTime()
    {
        if (TimeRemaining <= 0)
        {
            ToggleShieldDepleted();
            return;
        }
        spriteRenderer.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0.2f), (lifetime - TimeRemaining) / lifetime);
    }
    private void ToggleShieldDepleted()
    {
        Debug.Log("Shield is Depleted");
        player.ToggleInvincible();
        audioSource.Stop();
        gameObject.SetActive(false);
    }
    public void RestartShieldLifetime()
    {
        TimeRemaining = lifetime;
    }
    private void InstantiateData()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        player = transform.GetComponentInParent<PlayerActions>();
        audioSource = GetComponent<AudioSource>();
    }
}
