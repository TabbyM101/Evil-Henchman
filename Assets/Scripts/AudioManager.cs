using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Current;
        
    private AudioSource audioSource;

    public enum SongChoice
    {
        MainMenuMusic,
        GameMusic,
        RoomAmbience,
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Current = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayMusic(SongChoice.MainMenuMusic); // Assume we always start at main menu
    }

    private AudioClip ClipWithName(string audioName)
    {
        return Resources.Load($"Audio/{audioName}") as AudioClip;
    }

    // Changes the background music to the chosen song
    public void PlayMusic(SongChoice song)
    {
        switch (song)
        {
            case SongChoice.MainMenuMusic:
                audioSource.clip = ClipWithName("MainMenuMusic");
                break;
            case SongChoice.GameMusic:
                audioSource.clip = ClipWithName("GameMusic");
                break;
            case SongChoice.RoomAmbience:
                audioSource.clip = ClipWithName("RoomAmbience");
                break;
        }
        
        audioSource.Play();
    }

    // Play a clip from the AudioManager
    public void PlayClip(string audioClip)
    {
        AudioSource.PlayClipAtPoint(ClipWithName(audioClip), gameObject.transform.position);
    }
}