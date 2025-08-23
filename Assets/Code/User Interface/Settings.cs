using System;
using UnityEngine;

public class Settings : SingletonBasic<Settings>
{
    private static bool IsDataLoaded;

    //Controls
    public static IImpulseStrategy ImpulseType { get; private set; }

    //Audio
    public static float Sound { get; private set; } = 0.5f;
    public static float Music { get; private set; } = 0.5f;

    //Callback
    public static Action OnSaveValues { get; set; }

    protected override void Awake()
    {
        base.Awake();

        if (IsDataLoaded) return;
        SetImpulseType(PlayerPrefs.GetInt(nameof(IImpulseStrategy)));
        SetVolumeType(AudioMixerChannels.Sound, GetVolumeType(AudioMixerChannels.Sound));
        SetVolumeType(AudioMixerChannels.Music, GetVolumeType(AudioMixerChannels.Music));
        IsDataLoaded = true;
    }

    public int GetImpulseType() => PlayerPrefs.GetInt(nameof(IImpulseStrategy), 0);
    public void SetImpulseType(int index)
    {
        switch (index)
        {
            case 0: ImpulseType = new VoiceImpulse(); break;
            case 1: ImpulseType = new KeyboardImpulse(); break;
            default: print("not impulse type register"); return;
        }

        PlayerPrefs.SetInt(nameof(IImpulseStrategy), index);
    }

    public float GetVolumeType(AudioMixerChannels channel) => PlayerPrefs.GetFloat(channel.ToString(), 0.5f);
    public void SetVolumeType(AudioMixerChannels channel, float value)
    {
        switch (channel)
        {
            case AudioMixerChannels.Sound: Sound = value; break;
            case AudioMixerChannels.Music: Music = value; break;
        }

        PlayerPrefs.SetFloat(channel.ToString(), value);
    }

    public void SaveButton()
    {
        OnSaveValues?.Invoke();
    }
}