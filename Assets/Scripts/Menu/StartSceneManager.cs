using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup[] m_elements;
    [SerializeField] private string m_mainMenuScene;

    [Header("Fade parameters")]
    [SerializeField] private float m_fadeTime = 0.5f;
    [SerializeField] private float m_pauseTime = 1.0f;

    private int m_currentElement = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach(CanvasGroup cg in m_elements){
            cg.alpha = 0.0f;
        }

        StartCoroutine(FadeInElement());
    }

    private IEnumerator FadeInElement(){
        while(m_elements[m_currentElement].alpha < 1.0f){
            m_elements[m_currentElement].alpha += Time.deltaTime * m_fadeTime;
            yield return null;
        }
        m_elements[m_currentElement].alpha = 1.0f;
        StartCoroutine(FadeOutElement());
    }

    private IEnumerator FadeOutElement(){
        while(m_elements[m_currentElement].alpha > 0.0f){
            m_elements[m_currentElement].alpha -= Time.deltaTime * m_fadeTime;
            yield return null;
        }
        if(++m_currentElement >= m_elements.Length){
            SceneManager.LoadScene(m_mainMenuScene);
        } else {
            StartCoroutine(FadeInElement());
        }
    }
}
