using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip m_menuMusic;
    [SerializeField] private AudioClip m_gameMusic;
    private AudioSource m_audioSource;
    private bool isInMenu = true;
    private string sceneName;


    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        m_audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void useGameMusic()
    {
        m_audioSource.clip = m_gameMusic;
        m_audioSource.Play();
    }

    private void useMenuMusic()
    {
        m_audioSource.clip = m_menuMusic;
        m_audioSource.Play();
    }

    public AudioClip getCurrentBackgroundMusic()
    {
        return m_audioSource.clip;
    }

    public void Update()
    {
        if(sceneName.Contains("Game") != SceneManager.GetActiveScene().name.Contains("Game"))
        {
            sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.Contains("Game"))
            {
                useGameMusic();
            }
            else
            {
                useMenuMusic();
            }
        }
    }
}
