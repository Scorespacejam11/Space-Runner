using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManger : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle windowedtoggle;
    public GameObject PauseMenu;

    public GameObject MainPause;
    public GameObject SettingsPause;

    Animator setpauseanim,mainpauseanim;

    Animator PauseAnim;

    bool IsWindowed;
    bool IsPaused;

    bool IsScaling;


    public Slider slide;

    public GameObject LoadScreen;

    private void Awake()
    {
    }
    void Start()
    {
        IsPaused = false;
        IsScaling = true;
        setpauseanim = SettingsPause.GetComponent<Animator>();
        mainpauseanim = MainPause.GetComponent<Animator>();
        PauseAnim = PauseMenu.GetComponent<Animator>();
        slide.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        FindObjectOfType<SoundManger>().AdjustVolumeGlobal(slide.value);
        windowedtoggle.isOn = PlayerPrefs.GetString("Windowed", "false") == "true";
    }

    // Update is called once per frame
    void Update()
    {
        IsWindowed = PlayerPrefs.GetString("Windowed") == "true";

        if (IsWindowed && IsScaling)
        {
            print("Windowed");
            Screen.fullScreen = false;
            IsScaling = false;
        }
        else if (!IsWindowed && IsScaling)
        {
            print("Not Windowed");
            Screen.fullScreen = true;
            IsScaling = false;
        }

        if(!Player.IsDead) CheckIfPause();


        if (Input.GetKeyDown(KeyCode.Delete)) PlayerPrefs.DeleteAll();
        
    }

    public void OnValueChanged(float value)
    {
        FindObjectOfType<SoundManger>().AdjustVolumeGlobal(value);
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void OnStateChanged(bool state)
    {
        if (state) PlayerPrefs.SetString("Windowed", "true");
        else PlayerPrefs.SetString("Windowed", "false");
        IsScaling = true;
    }

    
    public void ChangeToSettings()
    {
        SettingsPause.SetActive(true);
        setpauseanim.Play("SettingsMainShow");
        mainpauseanim.Play("MainPauseHide");
    }

    void CheckIfPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                MainPause.transform.localScale = Vector3.one;
                PauseMenu.SetActive(true);
                MainPause.SetActive(true);
                SettingsPause.SetActive(false);
                Time.timeScale = 0;
            }
            else
            {
                PauseAnim.Play("HidePause");
                mainpauseanim.Play("New State");
                MainPause.transform.localScale = Vector3.zero;
                StartCoroutine(DisablePauseMenu());
                Time.timeScale = 1;
            }
        }
    }

    public void Restart()
    {
        LoadScreen.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    

    IEnumerator DisablePauseMenu()
    {
        yield return new WaitForSeconds(0.3f);
        MainPause.transform.localScale = Vector3.zero;
        PauseMenu.SetActive(false);
    }
}
