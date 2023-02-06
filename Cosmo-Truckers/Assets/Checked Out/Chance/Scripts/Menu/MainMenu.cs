using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    bool StartUp = true;
    [Header("Pages")]
    [SerializeField] GameObject[] TileScreen;
    [SerializeField] GameObject[] MainMenuScreen;

    [Header("Zooming")]
    [SerializeField] Transform TruckLocation;
    [SerializeField] float ZoomSpeed;
    [SerializeField] GameObject Fade;

    public void HostGame()
    {
        FindObjectOfType<NetworkManager>().StartHost();
    }

    public void JoinGame()
    {
        FindObjectOfType<NetworkManager>().StartClient();
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

}
