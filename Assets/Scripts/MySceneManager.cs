using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{


    public static void ExitApplication()
    {
        Application.Quit();
    }
 
    public static void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
