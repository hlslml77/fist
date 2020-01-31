using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 声音管理类
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    /// <summary>
    /// 用来播放背景音乐（循环）
    /// </summary>
    [SerializeField]
    private AudioSource bgmAudioSource;
    /// <summary>
    /// 特效的音乐
    /// </summary>
    [SerializeField]
    private AudioSource effectAudioSource;

    /// <summary>
    /// 等待的队列
    /// </summary>
    private Queue<AudioClip> acEffectQue = new Queue<AudioClip>();

    void Start()
    {
        bgmAudioSource.loop = true;
        bgmAudioSource.playOnAwake = true;

        effectAudioSource.loop = false;
        effectAudioSource.playOnAwake = false;
       // UIManager.Instance.ShowUIPanel(UIDefinit.UIAccount);
    }

    #region 背景音乐
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBgMusic(AudioClip clip)
    {
        if(clip == null)
        {
            return;
        }
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }
    /// <summary>
    /// 停止背景音乐的播放
    /// </summary>
    public void StopBgMusic()
    {
        bgmAudioSource.clip = null;
        bgmAudioSource.Stop();
    }
    #endregion

    #region 特效音乐
    /// <summary>
    /// 播放特效音乐
    /// </summary>
    /// <param name="clip"></param>
    public void PlayEffectMusic(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        effectAudioSource.clip = clip;
        effectAudioSource.Play();
       // acEffectQue.Enqueue(clip);
    }
    /// <summary>
    /// 一次播放多个音乐，用队列
    /// </summary>
    //void Update()
    //{
    //    //如果 这个AS不在播放状态 并且等待播放的音效文件数大于0
    //    if(!effectAudioSource.isPlaying && acEffectQue.Count > 0)
    //    {
    //        //先出队头的一个文件
    //        AudioClip ac = acEffectQue.Dequeue();
    //        //开始播放
    //        effectAudioSource.clip = ac;
    //        effectAudioSource.Play();
    //    }
    //}

    /// <summary>
    /// 停止特效音乐的播放
    /// </summary>
    public void StopEffectMusic()
    {
        effectAudioSource.clip = null;
        effectAudioSource.Stop();
    }
    #endregion
}

