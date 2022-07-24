using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject baguaUI;
    public float rotateTime;

    public void StartGame()
    {
        Debug.Log("Start Game");

        SceneManager.LoadScene("Demo");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");

        //StartCoroutine(UiRotate(46f));

        Application.Quit();
    }

    IEnumerator UiRotate(float degree)
    {
        var t = baguaUI.transform;

        var time = rotateTime;
        var d = degree / rotateTime;
        while (time >= 0)
        {
            t.Rotate(0, 0, d * Time.deltaTime);

            time -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        //t.rotation = Quaternion.Lerp(t.rotation, Quaternion.Euler(0, 0, degree), rotateTime);


    }
}
