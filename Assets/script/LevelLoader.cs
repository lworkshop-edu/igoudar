using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;
    public int scene = 1; 
    private void PlayClickSfx()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    public void LoadNextLevel()
    {
        PlayClickSfx();
        StartCoroutine(LoadLevel(scene)); 
    }

    IEnumerator LoadLevel(int levelIndex) 
    {
        if (transition != null) 
            transition.SetTrigger("start"); 
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }


    public void LoadSceneByIndex(int sceneIndex)
    {
        PlayClickSfx();
        StartCoroutine(LoadLevel(sceneIndex)); 
    }
}
