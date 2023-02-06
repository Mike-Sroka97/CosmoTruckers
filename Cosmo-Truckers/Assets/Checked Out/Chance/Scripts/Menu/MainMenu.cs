using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

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
    [SerializeField] GameObject Fade;

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
            StartUp = false;
            StartCoroutine(TitleScreenChange());
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

    IEnumerator MenuChange(float direction)
    {
        Vector3 camPos = new Vector3(direction, 0, -10);

        while ((int)Camera.main.transform.position.x != (int)direction)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camPos, Time.deltaTime);
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


        Camera.main.orthographicSize = 5;
        Fade.SetActive(false);

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
}
