using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonDontDestroy<AudioManager>
{
    [Header("Prefab & Pool")]
    [SerializeField] GameObject audioSourcePrefab;
    [SerializeField] int poolSize = 10;

    Queue<AudioSource> _audioPool = new Queue<AudioSource>();

    [SerializeField] AudioMixerGroup _sfxGroup;
    [SerializeField] AudioMixerGroup _uiGroup;
    [SerializeField] AudioSource _musicSource;

    [Header("Clip Tables")]
    [SerializeField] List<MusicClipData> _musicClips;
    [SerializeField] List<UIClipData> _uiClips;
    [SerializeField] List<SFXClipData> _sfxClips;

    protected override void OnAwake()
    {
        for (int i = 0; i < poolSize; i++)
            CreateAudioGameObject();
    }

    void CreateAudioGameObject()
    {
        GameObject obj = Instantiate(audioSourcePrefab, transform);
        obj.SetActive(false);
        AudioSource source = obj.GetComponent<AudioSource>();
        _audioPool.Enqueue(source);
    }

    public void PlayMusic(AudioMusicTable table)
    {
        foreach (var music in _musicClips)
        {
            if (music.type == table)
            {
                _musicSource.clip = music.clip;
                _musicSource.gameObject.SetActive(true);
                _musicSource.Play();
                return;
            }
        }
    }

    public void StopMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = null;
        _musicSource.gameObject.SetActive(false);
    }

    public void PlayUI(AudioUITable table)
    {
        foreach (var ui in _uiClips)
            if (ui.type == table)
                StartCoroutine(PlayOneShot(ui.clip, _uiGroup));
    }

    public void PlaySFX(AudioSFXTable table)
    {
        foreach (var sfx in _sfxClips)
            if (sfx.type == table)
                StartCoroutine(PlayOneShot(sfx.clip, _sfxGroup));
    }

    IEnumerator PlayOneShot(AudioClip clip, AudioMixerGroup mixerGroup)
    {
        if (_audioPool.Count == 0)
            CreateAudioGameObject();

        AudioSource source = _audioPool.Dequeue();
        source.outputAudioMixerGroup = mixerGroup;
        source.clip = clip;
        source.gameObject.SetActive(true);
        source.Play();

        yield return new WaitForSeconds(clip.length);

        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        _audioPool.Enqueue(source);
    }
}
