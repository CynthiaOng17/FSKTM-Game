using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource AudioSource;
    public AudioClip PlaySound;
    public bool IsOneShot = true;
    public bool IsDontDestroyOnLoad = false;

    void Start()
    {
        if (IsDontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        AudioSource = GetComponent<AudioSource>();
        if (AudioSource == null) AudioSource = gameObject.AddComponent<AudioSource>();

        if (IsOneShot){AudioSource.PlayOneShot(PlaySound);}
        else
        {
            AudioSource.loop = true;
            AudioSource.clip = PlaySound;
            AudioSource.Play();
        }

    }

    void OnDisable()
    {
        if (AudioSource != null && AudioSource.isPlaying && !IsOneShot)
        {
            AudioSource.Stop();
        }
    }
}
