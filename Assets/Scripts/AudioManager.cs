using UnityEngine;

public enum AudioType
{
    ButtonClick,
}

[System.Serializable]
public struct Audio
{
    public AudioClip audioClip;
    public AudioType audioType;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Audio[] audios;
    private AudioSource audioSource;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    public void PlaySoundEffect(AudioType type)
    {
        foreach(Audio aud in audios)
        {
            if (aud.audioType == type)
            {
                audioSource.PlayOneShot(aud.audioClip);
                break;
            }
        }
    }
}
