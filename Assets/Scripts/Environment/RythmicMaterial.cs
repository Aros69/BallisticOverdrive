using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RythmicMaterial : MonoBehaviour
{
    enum Parameter { EmissionIntensity, Red, Blue, Green, RedEmission, BlueEmission, GreenEmission, TilingX, TilingY, OffsetX, OffsetY};

    [SerializeField] Parameter m_parameter = Parameter.EmissionIntensity;
    [SerializeField] private int m_frequencyBand = 0;
    [SerializeField] private float m_scaleFrequencyBand = 5.0f;

    private AudioSpectrum m_audioSpectrum = null;
    private Renderer m_renderer = null;
    private List<Material> m_materials = null;
    // private List<float> m_baseParameters = null; // Base parameters found on the material


    // public float maxFrequency, minFrequency = 0;
    void Start()
    {
        if (m_frequencyBand > 7)
            throw new Exception("Frequency band index is superior to 7. There are only 8 frequency bands [0-7]");
        // Look for the audio Spectrum
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioSpectrum");
        if (objs.Length == 0)
            throw new Exception("No AudioSpectrum object found, are you sure it's correctly tagged with \"AudioSpectrum\" ?");
        m_audioSpectrum = objs[0].GetComponent<AudioSpectrum>();
        m_materials = new List<Material>(GetComponent<Renderer>().materials);
    }

    void Update()
    {
        float f = m_audioSpectrum.GetBandWidthValue(m_frequencyBand) * m_scaleFrequencyBand;
        Debug.Log("Here" + f);
        switch (m_parameter)
        {
            case Parameter.EmissionIntensity:
                SetFloatForMaterials("Vector1_C7CE0749", f);
                break;
            case Parameter.Red:
                SetColorForMaterials("Color_8C5B6BEE", new Color(f, 1.0f, 1.0f, 1.0f));
                break;
            case Parameter.Blue:
                SetColorForMaterials("Color_8C5B6BEE", new Color(1.0f, f, 1.0f, 1.0f));
                break;
            case Parameter.Green:
                SetColorForMaterials("Color_8C5B6BEE", new Color(1.0f, 1.0f, f, 1.0f));
                break;
            case Parameter.RedEmission:
                SetColorForMaterials("Color_45AD81E0", new Color(f, 1.0f, 1.0f, 1.0f));
                break;
            case Parameter.BlueEmission:
                SetColorForMaterials("Color_45AD81E0", new Color(1.0f, f, 1.0f, 1.0f));
                break;
            case Parameter.GreenEmission:
                SetColorForMaterials("Color_45AD81E0", new Color(1.0f, 1.0f, f, 1.0f));
                break;
            case Parameter.TilingX:
                SetVector2DForMaterials("Vector2_5AB9E672", new Vector2(f, 1));
                break;
            case Parameter.TilingY:
                SetVector2DForMaterials("Vector2_5AB9E672", new Vector2(1, f));
                break;
            case Parameter.OffsetX:
                SetVector2DForMaterials("Vector2_B6029552", new Vector2(f, 1));
                break;
            case Parameter.OffsetY:
                SetVector2DForMaterials("Vector2_B6029552", new Vector2(1, f));
                break;
        }
    }

    /// <summary>
    /// Update the max and minimum frequency
    /// Used to compute the increment 
    /// </summary>
    /// <param name="f"></param>
    // void UpdateFrequencyStats(float f)
    // {
    //     if(f > maxFrequency)
    //         maxFrequency = f;
    // }

    void SetColorForMaterials(String propertyName, Color col)
    {
        foreach(Material mat in m_materials)
        {
            if(mat.HasProperty(propertyName))
                mat.SetColor(propertyName, col);
        }
    }
    void SetFloatForMaterials(String propertyName, float f)
    {
        foreach(Material mat in m_materials)
        {   
            if(mat.HasProperty(propertyName))
                mat.SetFloat(propertyName, f);
        }
    }
    void SetVector2DForMaterials(String propertyName, Vector2 v)
    {
        foreach(Material mat in m_materials)
        {   
            if(mat.HasProperty(propertyName))
                mat.SetVector(propertyName, v);
        }
    }
}
