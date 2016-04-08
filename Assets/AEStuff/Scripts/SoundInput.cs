using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SoundInput : NetworkBehaviour
{

    public AudioSource audio;
    const int spectrumSize = 8192;
    const float binSize = 44100 / (spectrumSize * 2.0f);
    float[] spectrum = new float[spectrumSize];
    float frequency = 0;
    public float realFrequency;
    float previousFrequency = 0;

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            return;
        }
        audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start("Built-in Microphone", true, 5, 44100);
        //audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        int microhponeSamples = Microphone.GetPosition("Built-in Microphone");

        // We delay before we start the fft 30 ms
        if (!audio.isPlaying && microhponeSamples / 44100 > 0.030f)
        {
            audio.timeSamples = (int)(microhponeSamples - (0.030f * 44100));
            audio.Play();
        }


        audio.GetSpectrumData(spectrum, 0, FFTWindow.Triangle);

        float max = 0;
        int highestBand = 0;

        for (int i = 0; i < spectrumSize; i++)
        {
            if (spectrum[i] > 0.001f && spectrum[i] > max)
            {
                max = spectrum[i];
                highestBand = i;
            }
        }


        frequency = Mathf.Min((((float)highestBand * binSize)/1000.0f), 1.0f); 
        //frequency = ((float)highestBand * binSize);
        realFrequency = frequency;
        if (previousFrequency != frequency)
        {
            CmdUpdateFrequency(frequency);
        }
        previousFrequency = frequency;
    }

    float GetCurrentFrequency()
    {
        return frequency;
    }

    [Command]
    void CmdUpdateFrequency(float newFrequency)
    {
        if (!isLocalPlayer)
        {
            Debug.Log("Freq change");
        }

        frequency = newFrequency;
        realFrequency = newFrequency;
    }
}
