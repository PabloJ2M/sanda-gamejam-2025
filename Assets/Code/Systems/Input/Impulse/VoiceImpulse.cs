using UnityEngine;

public class VoiceImpulse : IImpulseStrategy
{
    private AudioClip _micClip;
    private string _micDevice;
    private const int _samples = 128;

    public VoiceImpulse()
    {
        if (Microphone.devices.Length > 0)
        {
            _micDevice = Microphone.devices[0];
            _micClip = Microphone.Start(_micDevice, true, 10, AudioSettings.outputSampleRate);
        }
    }
    public float GetImpulse()
    {
        if (_micClip == null) return 0f;

        float[] samples = new float[_samples];
        int micPos = Microphone.GetPosition(_micDevice) - _samples + 1;
        if (micPos < 0) return 0f;

        _micClip.GetData(samples, micPos);
        float level = 0f;

        foreach (var s in samples) level += Mathf.Abs(s);
        return (level /= _samples) * 10f;
    }
}