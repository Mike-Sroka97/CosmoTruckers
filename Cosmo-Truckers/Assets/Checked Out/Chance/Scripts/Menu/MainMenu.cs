using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    bool StartUp = true;
    [Header("Pages")]
    [SerializeField] GameObject[] TileScreen;
    [SerializeField] GameObject[] MainMenuScreen;
    [Space(10)]
    [SerializeField] GameObject MainOptions;
    [SerializeField] GameObject OptionOptions;

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
        Debug.LogError("Application Quiting");
        Application.Quit();
    }

    private void Update()
    {
        if(StartUp && Input.anyKeyDown)
        {
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

    public void ToogleShown()
    {
        //Toggle the screen player can see
        foreach (GameObject obj in TileScreen)
            obj.SetActive(!obj.activeSelf);

        foreach (GameObject obj in MainMenuScreen)
            obj.SetActive(!obj.activeSelf);
    }

    IEnumerator MenuChange(float direction)
    {
        Vector3 camPos = new Vector3(direction, 0, -10);

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

        StartUp = false;
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
