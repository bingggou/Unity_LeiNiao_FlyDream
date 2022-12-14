using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public bool autoSave=true;
    public string firstMusic = "";
    public string musicPath;
    public string soundPath;
    public static MusicManager instance;
    AudioSource musicAudioS;
    public int soundAudioSNum = 2;
    List<AudioSource> soundAudioS = new List<AudioSource>();
    public int soundLoopAudioSNum = 2;
    List<AudioSource> loopSoundAudioS = new List<AudioSource>();
    //AudioSource soundLoopAudioS;
    Slider slider4MusicVolumn;
    Slider slider4SoundVolumn;

    private const string IsFirst = nameof(IsFirst);
    private const string MusicVolumn = nameof(MusicVolumn);
    private const string SoundVolumn = nameof(SoundVolumn);


    // Start is called before the first frame update
    void Awake()
    {
        soundAudioSNum = Mathf.Clamp(soundAudioSNum, 1, 8);
        soundLoopAudioSNum = Mathf.Clamp(soundLoopAudioSNum, 1, 8);

        slider4MusicVolumn = TransformHelper.GetChild(transform, nameof(slider4MusicVolumn)).GetComponent<Slider>();
        slider4SoundVolumn = TransformHelper.GetChild(transform, nameof(slider4SoundVolumn)).GetComponent<Slider>();

        musicAudioS = gameObject.AddComponent<AudioSource>();
        musicAudioS.loop = true;
        musicAudioS.playOnAwake = false;

        for (int i = 0; i < soundAudioSNum; i++)
        {
            AudioSource tempAs = gameObject.AddComponent<AudioSource>();
            tempAs.loop = false;
            tempAs.playOnAwake = false;
            soundAudioS.Add(tempAs);
        }


        for (int i = 0; i < soundLoopAudioSNum; i++)
        {
            AudioSource tempAs = gameObject.AddComponent<AudioSource>();
            tempAs.loop = true;
            tempAs.playOnAwake = false;
            loopSoundAudioS.Add(tempAs);
        }

        IniSetVolumn();
        slider4MusicVolumn.onValueChanged.AddListener(SetMusicVolumn);
        slider4SoundVolumn.onValueChanged.AddListener(SetSoundVolumn);
        if (firstMusic != "")
        {
            ChangeAndPlayMusic(firstMusic);
        }
        instance = this;
    }

    void IniSetVolumn()
    {
        int isFirst = PlayerPrefs.GetInt(nameof(IsFirst));
        if (isFirst == 0)
        {

            SetMusicVolumn(0.5f);
            SetSoundVolumn(0.5f);
            SaveVolumn();
        }

        SetMusicVolumn(PlayerPrefs.GetFloat(nameof(MusicVolumn)));
        SetSoundVolumn(PlayerPrefs.GetFloat(nameof(SoundVolumn)));

        slider4MusicVolumn.value = musicAudioS.volume;
        slider4SoundVolumn.value = soundAudioS[0].volume;
    }


    public void SaveVolumn()
    {        
        PlayerPrefs.SetFloat(nameof(MusicVolumn), musicAudioS.volume);
        PlayerPrefs.SetFloat(nameof(SoundVolumn), soundAudioS[0].volume);
        PlayerPrefs.SetInt(nameof(IsFirst), 1);
    }


    void SetMusicVolumn(float _volumn)
    {
        musicAudioS.volume = _volumn;
    }
    void SetSoundVolumn(float _volumn)
    {
        foreach (var item in soundAudioS)
        {
            item.volume = _volumn;
        }
        foreach (var item in loopSoundAudioS)
        {
            item.volume = _volumn;
        }
    }


    public void ChangeAndPlayMusic(string musicName)
    {
        if (musicName == "")
        {
            Debug.Log("������Ϊ��");
            musicAudioS.Stop();
            musicAudioS.clip = null;
            return;
        }
        //����·����û��
        musicAudioS.clip = Resources.Load<AudioClip>(musicPath + "/" + musicName);

        if (musicAudioS.clip == null)
        {
            Debug.LogError("�����ڴ��ļ����֣�" + musicName);
        }
        else
        {

            musicAudioS.Play();

        }
    }


    public AudioClip GetLoopSoundClip()
    {       
        return GetLoopSoundClip(0);
    }
    public AudioClip GetLoopSoundClip(int index)
    {
        if (index < 0 || index >= loopSoundAudioS.Count)
        {
            Debug.LogError("�±����");
            return null;
        }
        return loopSoundAudioS[index].clip;
    }

    public AudioClip GetNowMusicClip()
    {
        return musicAudioS.clip;
    }





    public void ChangeAndPlaySound(string soundName, bool waitLastStop = false)
    {
        ChangeAndPlaySound(0, soundName, waitLastStop);
    }
    public void ChangeAndPlaySound(int index, string soundName, bool waitLastStop = false)
    {
        if (index < 0 || index >= soundAudioS.Count)
        {
            Debug.LogError("��������Ч������" + index);
            return;
        }

        if (waitLastStop == true)
        {
            if (soundAudioS[index].clip != null && soundAudioS[index].clip.name == soundName)
            {
                if (soundAudioS[index].isPlaying == true)
                {
                    return;

                }
            }
        }
        if (soundName == "")
        {
            Debug.Log("��Ч������" + index + ",��Ч��Ϊ��");
            soundAudioS[index].Stop();
            soundAudioS[index].clip = null;
            return;
        }

        soundAudioS[index].clip = Resources.Load<AudioClip>(soundPath + "/" + soundName);
        if (soundAudioS[index].clip == null)
        {
            Debug.LogError("��Ч������" + index + ",�����ڴ��ļ���Ч��" + soundName);
        }
        else
        {
            soundAudioS[index].Play();
        }



    }
    public void ChangeAndPlayLoopSound(string soundName)
    {
        ChangeAndPlayLoopSound(0, soundName);
    }

    public void ChangeAndPlayLoopSound(int index, string soundName,bool ingoreSameMusic=true)
    {
        if (index < 0 || index >= loopSoundAudioS.Count)
        {
            Debug.LogError("������ѭ����Ч������" + index);
            return;
        }


        if (ingoreSameMusic == true)
        {
            if (loopSoundAudioS[index].clip != null && loopSoundAudioS[index].clip.name == soundName)
            {
                if (loopSoundAudioS[index].isPlaying == false)
                {
                    loopSoundAudioS[index].Play();
                    return;
                }
            }
        }

        if (soundName == "")
        {
            Debug.Log("ѭ����Ч������" + index + ",ѭ����Ч��Ϊ��");
            loopSoundAudioS[index].Stop();
            loopSoundAudioS[index].clip = null;
            return;
        }

        loopSoundAudioS[index].clip = Resources.Load<AudioClip>(soundPath + "/" + soundName);
        if (loopSoundAudioS[index].clip == null)
        {
            Debug.LogError("�����ڴ��ļ���Ч��" + soundName);
        }
        else
        {
            loopSoundAudioS[index].Play();

        }
    }
}