using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip m_jump = null;
    public AudioClip m_ouch = null;
    private AudioSource m_audioSrc;

    void Start()
    {
        m_audioSrc = GetComponent<AudioSource>();
    }
    public void PlayJumpSound()
    {
        m_audioSrc.PlayOneShot(m_jump);
    }
    public void PlayOuchSound()
    {
        m_audioSrc.PlayOneShot(m_ouch);
    }
}
