using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceFX;
    [SerializeField] private AudioSource audioLoopFX;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip shooting;
    [SerializeField] private AudioClip thrust;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip oneUp;
    [SerializeField] Slider volumeSlider;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolumeLevels();
        }
        else LoadVolumeLevels();
        
    }
    public void PlayExplosion(float volume)
    {
        audioSourceFX.PlayOneShot(explosion, volume);
    }
    public void PlayShooting()
    {
        audioSourceFX.PlayOneShot(shooting, 0.25f);
    }
    public void ToggleThrust(bool shouldPlay)
    {
        if (shouldPlay) audioLoopFX.PlayOneShot(thrust, 0.25f);
        else if (audioLoopFX.isPlaying && !shouldPlay) audioLoopFX.Stop();
    }
    public void PlayDead()
    {
        audioSourceFX.PlayOneShot(death, 1f);
    }
    public void Play1UP()
    {
        audioSourceFX.PlayOneShot(oneUp, 1f);
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolumeLevels();
    }
    private void LoadVolumeLevels()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void SaveVolumeLevels()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
