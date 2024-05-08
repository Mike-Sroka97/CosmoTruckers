using System.Collections;
using System.Collections.Generic;
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

    protected override IEnumerator CutsceneCommands()
    {
        //Show text
        StartCoroutine(cameraController.FadeText(true));

        while (cameraController.ExecutingCommand)
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

        StartCoroutine(cameraController.Zoom(true, 1.2f, 3.5f));

        SpriteRenderer shipFront = ship.transform.Find("ShipFront").GetComponent<SpriteRenderer>();

        while (shipFront.color.a != 0)
        {
            shipFront.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
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
