using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RythmicMaterial : MonoBehaviour
{
    enum Parameter { EmissionIntensity, Red, Blue, Green, RedEmission, BlueEmission, GreenEmission, TilingX, TilingY, OffsetX, OffsetY};

    [SerializeField] Parameter m_parameter = Parameter.EmissionIntensity;
    [SerializeField] private int m_frequencyBand = 0;
    [SerializeField] private float m_scaleFrequencyBand = 100.0f;

    private AudioSpectrum m_audioSpectrum = null;
    private Renderer m_renderer = null;

    void Start()
    {
        if (m_frequencyBand > 7)
            throw new Exception("Frequency band index is superior to 7. There are only 8 frequency bands [0-7]");
        // Look for the audio Spectrum
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioSpectrum");
        if (objs.Length == 0)
            throw new Exception("No AudioSpectrum object found, are you sure it's correctly tagged with \"AudioSpectrum\" ?");
        m_audioSpectrum = objs[0].GetComponent<AudioSpectrum>();
        m_renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float f = m_audioSpectrum.GetBandWidthValue(m_frequencyBand) * m_scaleFrequencyBand;
        switch (m_parameter)
        {
            case Parameter.EmissionIntensity:
                m_renderer.material.SetFloat("Vector1_C7CE0749", f);
                break;
            case Parameter.Red:
                m_renderer.material.SetColor("Color_8C5B6BEE", new Color(f, 1.0f, 1.0f, 1.0f));
                break;
            case Parameter.Blue:
                m_renderer.material.SetColor("Color_8C5B6BEE", new Color(1.0f, f, 1.0f, 1.0f));
                break;
            case Parameter.Green:
                m_renderer.material.SetColor("Color_8C5B6BEE", new Color(1.0f, 1.0f, f, 1.0f));
                break;
            case Parameter.RedEmission:
                m_renderer.material.SetColor("Color_45AD81E0", new Color(f, 1.0f, 1.0f, 1.0f));
                break;
            case Parameter.BlueEmission:
                m_renderer.material.SetColor("Color_45AD81E0", new Color(1.0f, f, 1.0f, 1.0f));
                break;
            case Parameter.GreenEmission:
                m_renderer.material.SetColor("Color_45AD81E0", new Color(1.0f, 1.0f, f, 1.0f));
                break;
            case Parameter.TilingX:
                m_renderer.material.SetVector("Vector2_5AB9E672", new Vector2(f, 1));
                break;
            case Parameter.TilingY:
                m_renderer.material.SetVector("Vector2_5AB9E672", new Vector2(1, f));
                break;
            case Parameter.OffsetX:
                m_renderer.material.SetVector("Vector2_B6029552", new Vector2(f, 1));
                break;
            case Parameter.OffsetY:
                m_renderer.material.SetVector("Vector2_B6029552", new Vector2(1, f));
                break;
        }

    }
}
