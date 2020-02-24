using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] m_elements;
    [SerializeField] private string m_mainMenuScene;

    [Header("Fade parameters")]
    [SerializeField] private float m_timeToWaitBetweenLogos = 5.0f;
    [SerializeField] private float m_timeToWaitForSceneChange = 2.0f;

    // Start is called before the first frame update
    void Start(){
        StartCoroutine(ActivateSequence());
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(m_mainMenuScene);
    }

    private IEnumerator ActivateSequence(){
        for(int i = 0; i < m_elements.Length; i++){
            m_elements[i].SetActive(true);
            yield return new WaitForSeconds(m_timeToWaitBetweenLogos);
        }

        yield return new WaitForSeconds(m_timeToWaitForSceneChange);
        SceneManager.LoadScene(m_mainMenuScene);
    }
}
