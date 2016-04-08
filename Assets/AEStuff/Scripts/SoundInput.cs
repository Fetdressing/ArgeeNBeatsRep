using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SoundInput : NetworkBehaviour
{

    public AudioSource audio;
    const int spectrumSize = 8192;
    const float binSize = 44100 / (spectrumSize * 2.0f);
    float[] spectrum = new float[spectrumSize];
    [SyncVar (hook = "OnFrequencyChange")]float frequency = 0;
    public float realFrequency;
    public int test = 0;

    // Use this for initialization
    void Start () {
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

        test = microhponeSamples;
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
                highestBand = i;
            }
        }


        frequency = (float)highestBand * binSize; 
        realFrequency = frequency;

        //int i = 1;
        //while (i < spectrum.Length - 1)
        //{
        //    Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
        //    Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
        //    Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
        //    Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
        //    i++;
        //}
    }

    float GetCurrentFrequency()
    {
        return frequency;
    }

    void OnFrequencyChange(float newFrequency)
    {
        frequency = newFrequency;
        realFrequency = newFrequency;
    }
}
