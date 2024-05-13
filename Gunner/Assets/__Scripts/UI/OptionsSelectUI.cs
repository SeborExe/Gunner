using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsSelectUI : MonoBehaviour
{
    public void LoadInstructions()
    {
        SceneManager.UnloadSceneAsync("OptionsSelect");
        SceneManager.LoadScene("InstructionsScene", LoadSceneMode.Additive);
    }

    public void LoadGuid()
    {
        SceneManager.UnloadSceneAsync("OptionsSelect");
        SceneManager.LoadScene("WorldGuideScene", LoadSceneMode.Additive);
    }

    public void ReturnToOptionsSelectFromInstructions()
    {
        SceneManager.UnloadSceneAsync("InstructionsScene");
        SceneManager.LoadScene("OptionsSelect", LoadSceneMode.Additive);
    }

    public void ReturnToOptionsSelectFromGuid()
    {
        SceneManager.UnloadSceneAsync("WorldGuideScene");
        SceneManager.LoadScene("OptionsSelect", LoadSceneMode.Additive);
    }
}
