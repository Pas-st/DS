using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int quit = 0;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // ===== SCENE LOAD MIT SOUND =====

    public void Mainmenu(bool playSound = true)
    {
        StartCoroutine(LoadSceneWithSound(0, playSound));
    }

    public void ModeSelect(bool playSound = true)
    {
        StartCoroutine(LoadSceneWithSound(1, playSound));
    }

    public void PlayWAD()
    {
        StartCoroutine(LoadSceneWithSound(2));
    }

    public void Settings()
    {
        StartCoroutine(LoadSceneWithSound(3));
    }

    public void Credits()
    {
        StartCoroutine(LoadSceneWithSound(4));
    }

    public void Tutorial()
    {
        StartCoroutine(LoadSceneWithSound(5));
    }

    IEnumerator LoadSceneWithSound(int sceneIndex, bool playSound = true)
    {
        if (audioSource != null && clickSound != null && playSound)
        {
            // Sound abspielen
            audioSource.PlayOneShot(clickSound);
            // Warten bis Sound fertig
            yield return new WaitForSeconds(clickSound.length);
        }

        // Szene laden
        SceneManager.LoadScene(sceneIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quit == 0)
            {
                QuitGame();
            }
            else if (quit == 1)
            {
                Mainmenu(false);
            }
            else
            {
                ModeSelect(false);
            }
        }
    }
}