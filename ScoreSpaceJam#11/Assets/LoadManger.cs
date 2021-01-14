using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManger : MonoBehaviour
{
    // Start is called before the first frame update
    string[] hints =
    {
        "Did you know medkits heal you",
        "Did you know you can avoid hitting obstacles by switching lanes",
        "Did you know Obstacles damage you",
        "Did you know you can switch lanes",
    };

    public Text DidYouKnowTxt;

    public int LoadValue = 0;

    public Image LoadBar;

    public const int MaxLoadValue = 400;
    void Start()
    {
        DidYouKnowTxt.text = hints[Random.Range(0, hints.Length)];
        StartCoroutine(Loading());
      
    }

    // Update is called once per frame
    IEnumerator Loading()
    {
        AsyncOperation LoadGameOperation = SceneManager.LoadSceneAsync(1);
        LoadGameOperation.allowSceneActivation = false;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.005f);
            if (LoadValue <= MaxLoadValue)
            {
                LoadValue += 1;
                LoadBar.fillAmount = LoadValue / (float)MaxLoadValue;
            }
            if (LoadValue == MaxLoadValue)
            {
                LoadGameOperation.allowSceneActivation = true;
                SceneManager.UnloadScene(0);
            }
        }
    }
}
