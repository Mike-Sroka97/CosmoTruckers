using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using UnityEditor;
using TMPro;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    bool StartUp = true;
    [Header("Input")]
    [SerializeField] InputControl input;

    [Header("Pages")]
    [SerializeField] GameObject[] TileScreen;
    [SerializeField] GameObject[] MainMenuScreen;
    [Space(10)]
    [SerializeField] GameObject MainOptions;
    [SerializeField] GameObject OptionOptions;
    [SerializeField] GameObject ComfirmPage;
    int Confirm = -1;

    [Header("Zooming")]
    [SerializeField] Transform TruckLocation;
    [SerializeField] float ZoomSpeed;
    [SerializeField] float TranslateSpeed;
    [SerializeField] GameObject Fade;

    [Header("Character pass by")]
    [SerializeField] GameObject[] CharacterPrefabs;
    [SerializeField] Transform StartPosition;
    [SerializeField] Vector2 SpawnFrequency;
    [SerializeField] float WalkTime;
    [SerializeField] float WalkSpeed;
    bool Spawned = false;
    float SpawnTime;

    [Header("Options Menu")]
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Toggle WindowedToggle;
    [SerializeField] TMP_Dropdown ResolutionDropDown;

    delegate void YesFunction();
    delegate void NoFunction();


    public void HostGame()
    {
        StartCoroutine(HostFade());
    }

    public void JoinGame()
    {
        StartCoroutine(ClientFade());
    }

    public void QuitGame()
    {
        StartCoroutine(QuitingGame());
    }

    private void Update()
    {
        if(StartUp && Input.anyKeyDown)
        {
            StartUp = false;
            StartCoroutine(TitleScreenChange());
            SpawnTime = UnityEngine.Random.Range(SpawnFrequency.x, SpawnFrequency.y) + Time.time;
        }
        //In the menus
        else if(!StartUp && !Spawned && SpawnTime <= Time.time)
        {
            Spawned = true;
            SpawnTime = UnityEngine.Random.Range(SpawnFrequency.x, SpawnFrequency.y) + Time.time;

            StartCoroutine(SpawnCharacter());
        }
    }

    private void Awake() => StartUpSettings();

    private void StartUpSettings()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFX", .5f);
        MusicSlider.value = PlayerPrefs.GetFloat("Music", .5f);

        int width = PlayerPrefs.GetInt("ResolutionWidth", 1920);

        ResolutionDropDown.value =
            width == 1280 ? 0 :
            width == 1366 ? 1 :
            width == 1920 ? 2 : 2;
        SetResolution();
    }

    public void ShowOptionsMenu()
    {
        MainOptions.SetActive(false);
        StartCoroutine(MenuChange(-20.86f));
    }

    public void ShowMainMenu()
    {
        OptionOptions.SetActive(false);
        StartCoroutine(MenuChange(0.0f));
    }

    private void SetResolution()
    {
        bool fullScreen = PlayerPrefs.GetInt("WindowedMode", 0) == 0 ? true : false;
        WindowedToggle.SetIsOnWithoutNotify(!fullScreen);

        switch (ResolutionDropDown.value)
        {
            default:
            case 0:
                Screen.SetResolution(1280, 720, fullScreen);
                PlayerPrefs.SetInt("ResolutionWidth", 1280);
                PlayerPrefs.SetInt("ResolutionHeight", 720);
                break;
            case 1:
                Screen.SetResolution(1366, 768, fullScreen);
                PlayerPrefs.SetInt("ResolutionWidth", 1366);
                PlayerPrefs.SetInt("ResolutionHeight", 768);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, fullScreen);
                PlayerPrefs.SetInt("ResolutionWidth", 1920);
                PlayerPrefs.SetInt("ResolutionHeight", 1080);
                break;

        }
    }

    public void ChangeResolution()
    {
        bool fullScreen = Screen.fullScreen;

        switch (ResolutionDropDown.value)
        {
            default:
            case 0:
                Screen.SetResolution(1280, 720, fullScreen);
                break;
            case 1:
                Screen.SetResolution(1366, 768, fullScreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, fullScreen);
                break;

        }
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetInt("WindowedMode", WindowedToggle.isOn ? 1 : 0);

        SetResolution();

        PlayerPrefs.SetFloat("Music", MusicSlider.value);
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);

        AudioManager.Instance.CheckVolume();

        PlayerPrefs.Save();
    }

    public void DefaultOptions()
    {
        PlayerPrefs.DeleteAll();

        WindowedToggle.SetIsOnWithoutNotify(false);
        MusicSlider.value = .5f;
        SFXSlider.value = .5f;
        ResolutionDropDown.value = 2;
        SetResolution();

        AudioManager.Instance.CheckVolume();
    }

    public void TestSFXVol(float vol)
    {
        if(OptionOptions.activeInHierarchy)
            AudioManager.Instance.TestSFXVolume(vol);
    }
    public void TestMusicVol(float vol)
    {
        AudioManager.Instance.TestMusicVolume(vol);
    }

    IEnumerator MenuChange(float direction)
    {
        Vector3 camPos = new Vector3(direction, 0, -10);

        if(direction == 0 && CheckForChanges())
        {
            yield return ConfirmWait("Are you sure you want to leave without saving?",
                delegate 
                {
                    StartUpSettings();
                    Confirm = 1;
                },
                delegate
                {
                    Confirm = 0;
                });

            if(Confirm == 0)
            {
                OptionOptions.SetActive(true);
                Confirm = -1;
                yield break;
            }
            Confirm = -1;
        }

        while ((int)Camera.main.transform.position.x != (int)direction)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, camPos, Time.deltaTime * TranslateSpeed);
            yield return null;
        }

        Camera.main.transform.position = camPos;

        if (direction == 0)
            MainOptions.SetActive(true);
        else
            OptionOptions.SetActive(true);

    }

    IEnumerator ConfirmWait(string text, YesFunction yes, NoFunction no)
    {
        ComfirmPage.SetActive(true);

        ComfirmPage.GetComponent<ConfirmEvent>().ChangeDisplayText(text);
        ComfirmPage.GetComponent<ConfirmEvent>().AddListner(true, delegate { yes(); } );
        ComfirmPage.GetComponent<ConfirmEvent>().AddListner(false, delegate { no(); } );

        while (Confirm == -1)
            yield return null;

        ComfirmPage.GetComponent<ConfirmEvent>().RemoveListners();
        ComfirmPage.SetActive(false);
    }

    IEnumerator TitleScreenChange()
    {
        Fade.SetActive(true);

        //Zoom into truck
        while(Camera.main.orthographicSize >= 3.5f)
        {
            Vector3 camPos = Vector3.Lerp(Camera.main.transform.position, TruckLocation.position, Time.deltaTime);
            Camera.main.transform.position = new Vector3(camPos.x, camPos.y, -10);
            Camera.main.orthographicSize -= ZoomSpeed * Time.deltaTime;

            Fade.GetComponent<Image>().color = new Color(0, 0, 0, Fade.GetComponent<Image>().color.a + (ZoomSpeed * Time.deltaTime));
            yield return null;
        }

        //Reset Cam
        Camera.main.transform.position = new Vector3(0, 0, -10);

        //Toggle the screen player can see
        foreach (GameObject obj in TileScreen)
            obj.SetActive(false);

        foreach (GameObject obj in MainMenuScreen)
            obj.SetActive(true);

        //Fade out of black
        while (Camera.main.orthographicSize < 5)
        {
            Fade.GetComponent<Image>().color = new Color(0, 0, 0, Fade.GetComponent<Image>().color.a - (ZoomSpeed * Time.deltaTime));
            Camera.main.orthographicSize += ZoomSpeed * Time.deltaTime;
            yield return null;
        }

        MainOptions.SetActive(true);
        Camera.main.orthographicSize = 5;
        Fade.SetActive(false);

        //StartUp = false;
    }

    IEnumerator HostFade()
    {
        Fade.SetActive(true);

        while (Camera.main.orthographicSize >= 3.5f)
        {
            Vector3 camPos = Vector3.Lerp(Camera.main.transform.position, TruckLocation.position, Time.deltaTime);
            Camera.main.transform.position = new Vector3(camPos.x, camPos.y, -10);
            Camera.main.orthographicSize -= ZoomSpeed * Time.deltaTime;

            Fade.GetComponent<Image>().color = new Color(0, 0, 0, Fade.GetComponent<Image>().color.a + (ZoomSpeed * Time.deltaTime));
            yield return null;
        }
        FindObjectOfType<NetworkManager>().StartHost();
    }

    IEnumerator ClientFade()
    {
        Fade.SetActive(true);

        while (Camera.main.orthographicSize >= 3.5f)
        {
            Vector3 camPos = Vector3.Lerp(Camera.main.transform.position, TruckLocation.position, Time.deltaTime);
            Camera.main.transform.position = new Vector3(camPos.x, camPos.y, -10);
            Camera.main.orthographicSize -= ZoomSpeed * Time.deltaTime;

            Fade.GetComponent<Image>().color = new Color(0, 0, 0, Fade.GetComponent<Image>().color.a + (ZoomSpeed * Time.deltaTime));
            yield return null;
        }
        FindObjectOfType<NetworkManager>().StartClient();
    }

    IEnumerator SpawnCharacter()
    {
        GameObject character = Instantiate(CharacterPrefabs[UnityEngine.Random.Range(0, CharacterPrefabs.Length)]);
        character.transform.position = StartPosition.position;
        float endWalk = Time.time + WalkTime;

        while(endWalk >= Time.time)
        {
            character.transform.Translate(Vector3.left * Time.deltaTime * WalkSpeed);
            yield return null;
        }

        Destroy(character);
        Spawned = false;
    }

    IEnumerator QuitingGame()
    {
        yield return ConfirmWait("Exit to DeskTop?",
            delegate
            {
                Debug.LogError("Application Quiting");
                Debug.LogError("May break in editor");
                Application.Quit();
            },
            delegate
            {
                Confirm = 0;
            });

        Confirm = -1;
    }

    //TODO 
    //Will need to refactor this area, I hate the way it runs
    private bool CheckForChanges()
    {
        bool value = false;

        if (SFXSlider.value != PlayerPrefs.GetFloat("SFX", .5f))
            value = true;
        if(MusicSlider.value != PlayerPrefs.GetFloat("Music", .5f))
            value = true;

        print(PlayerPrefs.GetInt("WindowedMode"));

        //TODO find a better way
        if (PlayerPrefs.GetInt("WindowedMode", 0) == 0 && WindowedToggle.isOn ||
            PlayerPrefs.GetInt("WindowedMode", 0) == 1 && !WindowedToggle.isOn)
            value = true;
        if (Screen.currentResolution.height != PlayerPrefs.GetInt("ResolutionHeight", 1080))
            value = true;
        return value;
    }

    public void ToogleShown()
    {
        //Toggle the screen player can see
        foreach (GameObject obj in TileScreen)
            obj.SetActive(!obj.activeSelf);

        foreach (GameObject obj in MainMenuScreen)
            obj.SetActive(!obj.activeSelf);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MainMenu))]
public class MainMenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainMenu myScript = (MainMenu)target;
        if (GUILayout.Button("Toggle Shown"))
        {
            myScript.ToogleShown();
        }
    }
}
#endif
