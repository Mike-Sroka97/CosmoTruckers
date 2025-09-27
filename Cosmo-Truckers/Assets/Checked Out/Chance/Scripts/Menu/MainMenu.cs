using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool StartUp = true;
    [Header("Input")]
    [SerializeField] InputControl input;

    [Header("Zooming")]
    [SerializeField] Transform TruckLocation;
    [SerializeField] float ZoomSpeed;
    [SerializeField] float TranslateSpeed;
    [SerializeField] GameObject Fade;

    [SerializeField] string hub;
    [SerializeField] string tutorial;

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

        //Load tutorial if tutorial is not complete
        if (SaveManager.LoadTutorialStatus().TutorialFinished)
            SceneManager.LoadScene(hub);
        else
            SceneManager.LoadScene(tutorial);
    }
}
