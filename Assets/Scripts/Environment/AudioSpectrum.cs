using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioSpectrum : MonoBehaviour
{
    private AudioSource m_src = null;

    /// <summary>
    /// Number of samples taken from an audio at a moment "t"
    /// </summary>
    [SerializeField] private float[] m_samples = new float[512];

    /// <summary> 
    /// Contains the value of 8 frequencies bandwidths
    /// m_bands[0] corresponds to the deepest notes
    /// m_bands[7] corresponds to the highest notes
    /// </summary>
    [SerializeField] private float[] m_bands = new float[8];
    [SerializeField] private int m_sampleNumber = 512;
    
    private List<GameObject> cubeSamples;
    private List<GameObject> cubeBands;
    // Start is called before the first frame update
    void Start()
    {
        cubeSamples = new List<GameObject>();
        m_samples = new float[m_sampleNumber];
        m_src = GetComponent<AudioSource>();        
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        ComputeBandwiths();
    }

    void GetSpectrumAudioSource()
    {
        m_src.GetSpectrumData(m_samples, 0, FFTWindow.Blackman);
    }

    void ComputeBandwiths()
    {
        int currentSample = 0;
        for(int i = 0; i < 8; i++)
        {
            int sampleCount = (int) Mathf.Pow(2,i+1);

            if(i == 7)
                sampleCount +=2;
            for(int j = 0; j < sampleCount; j++)
            {
                m_bands[i] += m_samples[currentSample] * (currentSample+1);
                currentSample++;
            }
            m_bands[i] /= sampleCount;
        }
    }

    public float GetBandWidthValue(int i)
    {
        if(i >= 8)
            throw new Exception("BandWidth out of range, are you sure it's below 8 ?");
        return m_bands[i];
    }
    public float GetSubBass()
    {
        return GetBandWidthValue(0);
    }
    public float GetBass()
    {
        return GetBandWidthValue(1);
    }

    void InitSampleCubes()
    {
        for(int i = 0; i < m_sampleNumber; i++)
        {
            GameObject c;
            c = GameObject.CreatePrimitive(PrimitiveType.Cube);
            c.transform.position = new Vector3(i,0,0);

            cubeSamples.Add(c);
        }
    }
    void UpdateSampleCubes()
    {
        for(int i = 0; i < m_sampleNumber ; i++)
            cubeSamples[i].transform.localScale = new Vector3(1, 1000.0f*m_samples[i], 1);
    }
    void InitBandCubes()
    {
        for(int i = 0; i < 8; i++)
        {
            GameObject c;
            c = GameObject.CreatePrimitive(PrimitiveType.Cube);
            c.transform.position = new Vector3(i,0,-30);
            cubeBands.Add(c);
        }
    }

    void UpdateBandCubes()
    {
        for(int i = 0; i < 8 ; i++)
            cubeBands[i].transform.localScale = new Vector3(1, 100.0f*m_bands[i], 1);
    }
}
