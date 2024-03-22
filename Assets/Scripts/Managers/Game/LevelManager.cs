using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("--- Level Manager Settings")]
    public string mainScreen;
    public string actualGame;
    [Space]
    public float transitionTime;
    public GameObject fadeScreen;
    public Animator fadeAnimator;

    private void Awake()
    {
        fadeScreen.SetActive(true);  
    }

    private void Start()
    {
        PlayFadeIn();
    }

    public void ChangeScene(string sceneName)
    {
        StopCoroutine(SceneChange(sceneName));
        StartCoroutine(SceneChange(sceneName));
    }

    public void ChangeScene(int sceneIndex)
    {
        StopCoroutine(SceneChange(sceneIndex));
        StartCoroutine(SceneChange(sceneIndex));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator SceneChange(string sceneName)
    {
        PlayFadeOut();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private IEnumerator SceneChange(int sceneIndex)
    {
        PlayFadeOut();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    public void PlayFadeIn()
    {
        fadeAnimator.Play("FadeIn");
    }

    public void PlayFadeOut()
    {
        fadeAnimator.Play("FadeOut");
    }
}
