using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDataBaseSO audioDataBase;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private Transform player;
    
    private AudioClip lastMusicPlayed;
    private string currentBgmGroupName;
    private Coroutine currentBgmCo;
    [SerializeField] private bool bgmShouldPlay;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(bgmSource.isPlaying == false && bgmShouldPlay)
        {
            if(string.IsNullOrEmpty(currentBgmGroupName) == false)
                NextBGM(currentBgmGroupName);
        }

        if(bgmSource.isPlaying && bgmShouldPlay == false)
            StopBGM();
    }

    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;

        if(musicGroup == currentBgmGroupName)
            return;

        NextBGM(musicGroup);
    }

    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentBgmGroupName = musicGroup;

        if(currentBgmCo != null)
            StopCoroutine(currentBgmCo);

        currentBgmCo = StartCoroutine(SwitchMusicCo(musicGroup));
    }

    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(FadeVolumeCo(bgmSource, 0, 1));

        if(currentBgmCo != null)
            StopCoroutine(currentBgmCo);
    }

    private IEnumerator SwitchMusicCo(string musicGroup)
    {
        AudioClipData  data = audioDataBase.Get(musicGroup);
        AudioClip nextMusic = data.GetRandomClip();

        if(data == null || data.clips.Count == 0)
        {
            yield break;
        }
        

        while(nextMusic == lastMusicPlayed)
            nextMusic = data.GetRandomClip();



        if(bgmSource.isPlaying)
            yield return FadeVolumeCo(bgmSource, 0, 1f);

        lastMusicPlayed = nextMusic;
        bgmSource.clip = nextMusic;
        bgmSource.Play();

        StartCoroutine(FadeVolumeCo(bgmSource, data.maxVolume, 1f));
    }

    private IEnumerator FadeVolumeCo(AudioSource source, float targetVolume, float duration)
    {
        float time = 0;
        float startVolume = source.volume;

        while(time <duration)
        {
            time += Time.deltaTime;
            
            source.volume = Mathf.Lerp(startVolume, targetVolume, time/duration);
            yield return null;
        }

        source.volume = targetVolume;
    }

    public void PlaySFX (string soundName, AudioSource sfxSource, float minDistanceToHear = 5)
    {
        if(player == null)
            player = Player.instance.transform;

        
        var data = audioDataBase.Get(soundName);
        if(data == null) return;

        var clip = data.GetRandomClip();
        if(clip == null) return;

        float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position,player.transform.position);
        float t = Mathf.Clamp01(1 - (distance/ minDistanceToHear));

        sfxSource.clip = clip;
        sfxSource.volume = Mathf.Lerp(0,maxVolume,t*t);
        sfxSource.pitch = Random.Range(.95f,1.1f);
        sfxSource.PlayOneShot(clip);

    }

    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDataBase.Get(soundName);

        if(data == null) return;

        var clip = data.GetRandomClip();
        if(clip == null)    return;

        Debug.Log("Player audio " + soundName);
        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(.95f, 1.1f);
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);

    }

}
