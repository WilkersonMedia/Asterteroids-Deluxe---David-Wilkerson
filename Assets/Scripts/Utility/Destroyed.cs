using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyed : MonoBehaviour
{
    [SerializeField] private GameObject explosionFX;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float volume = 0.25f;
    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        Instantiate<GameObject>(explosionFX, transform.position, transform.rotation, null);
        FindObjectOfType<AudioManager>().PlayExplosion(volume);
    }
}
