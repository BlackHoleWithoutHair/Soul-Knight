using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : AbstractSystem
{
    public AudioSource m_MusicAudioSource { get; private set; }
    public AudioSource m_SoundAudioSource { get; private set; }
    private AudioMixer m_MusicMixer;
    private AudioMixer m_SoundMixer;
    private GameObject gameObject;
    protected override void OnInit()
    {
        base.OnInit();
        if (!GameObject.Find("AudioSystem"))
        {
            gameObject = new GameObject("AudioSystem");

            m_MusicAudioSource = gameObject.AddComponent<AudioSource>();
            m_SoundAudioSource = gameObject.AddComponent<AudioSource>();
            m_MusicAudioSource.loop = true;
            m_MusicAudioSource.outputAudioMixerGroup = ProxyResourceFactory.Instance.Factory.GetResources<AudioMixerGroup>("BGM")[0];
            m_SoundAudioSource.outputAudioMixerGroup = ProxyResourceFactory.Instance.Factory.GetResources<AudioMixerGroup>("SFX")[0];
            m_MusicMixer = m_MusicAudioSource.outputAudioMixerGroup.audioMixer;
            m_SoundMixer = m_SoundAudioSource.outputAudioMixerGroup.audioMixer;
        }
        else
        {
            gameObject = GameObject.Find("AudioSystem");
            m_MusicAudioSource = gameObject.GetComponent<AudioSource>();
            m_SoundAudioSource = gameObject.GetComponent<AudioSource>();
            m_MusicAudioSource.loop = true;
            m_MusicAudioSource.outputAudioMixerGroup = ProxyResourceFactory.Instance.Factory.GetResources<AudioMixerGroup>("BGM")[0];
            m_SoundAudioSource.outputAudioMixerGroup = ProxyResourceFactory.Instance.Factory.GetResources<AudioMixerGroup>("SFX")[0];
            m_MusicMixer = m_MusicAudioSource.outputAudioMixerGroup.audioMixer;
            m_SoundMixer = m_SoundAudioSource.outputAudioMixerGroup.audioMixer;
        }
        switch (SceneModelCommand.Instance.GetActiveSceneName())
        {
            case SceneName.MainMenuScene:
                SetMusic("bgm_1Low");
                break;
            case SceneName.MiddleScene:
                SetMusic("bgm_room");
                break;
            case SceneName.BattleScene:
                SetMusic("bgm_1Low");
                break;
        }
    }
    public void SetMusic(string name)
    {
        m_MusicAudioSource.clip = ProxyResourceFactory.Instance.Factory.GetAudioClip(name);
    }
    public void SetSound(string name)
    {
        m_SoundAudioSource.PlayOneShot(ProxyResourceFactory.Instance.Factory.GetAudioClip(name));
    }
    public void SetMusicVolume(float val)
    {
        m_MusicMixer.SetFloat("VolumeBGM", Remap01ToDb(val));
        m_MusicAudioSource.Play();
    }
    public void SetSoundVolume(float val)
    {
        m_SoundMixer.SetFloat("VolumeSFX", Remap01ToDb(val));
    }
    private float Remap01ToDb(float val)
    {
        if (val <= 0) val = 0.01f;
        return Mathf.Log10(val) * 20;
    }
}
