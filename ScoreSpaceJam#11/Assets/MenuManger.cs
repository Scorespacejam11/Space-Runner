using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManger : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Menu;
    public GameObject Credits;

    public GameObject clText;

    public InputField Ifield;

    public GameObject LoadingScreen;

    

    

    void Start()
    {
        Time.timeScale = 1f;
        Ifield.text = PlayerPrefs.GetString("Name");
    }

    public void Play()
    {
        if (CharLength() >= 3)
        {
            LoadingScreen.SetActive(true);
        }
        else
        {
            clText.SetActive(true);
            StartCoroutine(disablecl());
        }
    }
    public void ChangeToCredits()
    {
        Menu.SetActive(false);
        Credits.SetActive(true);
    }

    public void ChangeToMenu()
    {
        Menu.SetActive(true);
        Credits.SetActive(false);
    }

    public void OnValueChanged(string value)
    {
        PlayerPrefs.SetString("Name", value);
        if (CharLength() < 3)
        {
            clText.SetActive(true);
            StartCoroutine(disablecl());
        }
    }

    IEnumerator disablecl()
    {
        yield return new WaitForSeconds(2.5f);
        clText.SetActive(false);
    }


    public int CharLength()
    {
        int length = 0;
        foreach(char c in PlayerPrefs.GetString("Name"))
        {
            length += 1;
        }
        return length;
    }


    public void Exit()
    {
        Application.Quit();
    }

}
