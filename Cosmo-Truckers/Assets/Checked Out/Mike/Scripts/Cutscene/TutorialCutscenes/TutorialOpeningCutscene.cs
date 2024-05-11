using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialOpeningCutscene : CutsceneController
{
    [SerializeField] float textWaitTime;
    [SerializeField] Vector3 metrodonClusterPosition;
    [SerializeField] float cameraMoveSpeed;

    [SerializeField] Transform ship;
    [SerializeField] float shipMoveSpeed;
    [SerializeField] Transform cluster;
    [SerializeField] Transform bg;

    [SerializeField] float fadeSpeed;
    [SerializeField] BaseActor[] baseActors; 

    protected override IEnumerator CutsceneCommands()
    {
        //Show text
        StartCoroutine(cameraController.FadeText(true));

        while (cameraController.CommandsExecuting > 0)
            yield return null;

        //Fade text out and move camera simultaneously (can't use a ExecutingCommand check here so we use a camera pos check)
        yield return new WaitForSeconds(textWaitTime);

        StartCoroutine(cameraController.FadeText(false));
        StartCoroutine(cameraController.MoveTowardsPosition(metrodonClusterPosition, cameraMoveSpeed, true));

        while (cameraController.transform.position != metrodonClusterPosition)
            yield return null;

        //execute camera shake
        StartCoroutine(cameraController.Shake(5f, 40, .05f));

        //move ship and fucking obliterate the meteors
        yield return new WaitForSeconds(2);

        StartCoroutine(MoveShip());

        //fade front of ship out
        yield return new WaitForSeconds(3);

        // Set the player actors here, since this cutscene will always have the same player actors
        DialogManager.Instance.SetPlayerActors(baseActors.ToList());

        // Setup Dialog
        DialogSetup();

        // Zoom into ship
        StartCoroutine(cameraController.Zoom(true, 1.2f, 3.5f));

        SpriteRenderer shipFront = ship.transform.Find("ShipFront").GetComponent<SpriteRenderer>();

        while (shipFront.color.a > 0)
        {
            shipFront.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        //TODO COLE PLEASE GOD PLEEEEASE I AM BEGGING YOU ADD DIALOG FOR TUTORIAL PART 1 STEPS 9-13

        while (cameraController.CommandsExecuting > 0)
        {
            yield return null;
        }

        // Advance the scene
        StartCoroutine(AdvanceScene());

        while (DialogManager.Instance.DialogIsPlaying)
        {
            yield return null;
        }

        StartCoroutine(cameraController.FadeVignette(false));

        yield return null;

        while (cameraController.transform.Find("CameraVignette").GetComponent<SpriteRenderer>().color.a < 1)
            yield return null;

        End();
    }

    private void Update()
    {
        CheckPlayerInput(); 
    }

    private IEnumerator MoveShip()
    {
        while (ship.position.x < metrodonClusterPosition.x)
        {
            ship.position += new Vector3(shipMoveSpeed * Time.deltaTime, 0);
            yield return null;
        }

        if(cluster)
        {
            cluster.position -= new Vector3(shipMoveSpeed * Time.deltaTime, 0);
            if (cluster.position.x < -22.5f)
                Destroy(cluster.gameObject);
        }

        bg.position -= new Vector3(shipMoveSpeed * Time.deltaTime, 0);
        if (bg.position.x < -10f)
            bg.position = Vector3.zero;

        yield return null;

        StartCoroutine(MoveShip());
    }
}
