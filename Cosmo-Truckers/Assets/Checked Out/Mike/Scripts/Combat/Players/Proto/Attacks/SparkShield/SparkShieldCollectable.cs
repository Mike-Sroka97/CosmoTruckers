using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkShieldCollectable : MonoBehaviour
{
    [SerializeField] int score;

    [SerializeField] bool isProton;
    [SerializeField] bool isNeutron;
    [SerializeField] bool isElectron;

    [SerializeField] float moveSpeedX;
    [SerializeField] float moveSpeedY;

    SparkShield miniGame;
    Rigidbody2D myBody;

    public void Initialize()
    {
        miniGame = FindObjectOfType<SparkShield>();
        myBody = GetComponent<Rigidbody2D>();

        int x = UnityEngine.Random.Range(0, 2);
        int y = UnityEngine.Random.Range(0, 2);
        float tempX = moveSpeedX;
        float tempY = moveSpeedY;

        if (x == 0)
            tempX = -moveSpeedX;
        if (y == 0)
            tempY = -moveSpeedY;

        myBody.velocity = new Vector2(tempX, tempY);
    }

    private void TypeSpecificTrigger(bool playerIframes)
    {
        if(isProton)
        {
            if (playerIframes)
                return;

            miniGame.Score += score;
            miniGame.CheckScoreAndAugmentSuccess(); 
        }
        else if(isNeutron)
        {
            miniGame.AugmentScore++;
            miniGame.CheckScoreAndAugmentSuccess();
        }
        else if(isElectron)
        {
            if (playerIframes)
                return;

            miniGame.Score -= score;
        }

        Destroy(gameObject);
    }

    public void VelocityUpdate(int quadrant)
    {
        switch (quadrant)
        {
            case 0:
                myBody.velocity = new Vector2(myBody.velocity.x, -moveSpeedY);
                break;
            case 1:
                myBody.velocity = new Vector2(moveSpeedX, myBody.velocity.y);
                break;
            case 2:
                myBody.velocity = new Vector2(myBody.velocity.x, moveSpeedY);
                break;
            case 3:
                myBody.velocity = new Vector2(-moveSpeedX, myBody.velocity.y);
                break;
            default:
                break;
        }
    }

    private void VelocityToggle()
    {
        myBody.velocity = new Vector2(-myBody.velocity.x, -myBody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            TypeSpecificTrigger(FindObjectOfType<Player>().iFrames);
        }
        else if(collision.gameObject.name == "PlayArea")
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 center = transform.position;
            Vector3 direction = collisionPoint - center;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
                angle += 45;
            if (angle > 360)
                angle = 360;

            int quadrant = (int)(angle / 90);
            Debug.Log("quadrant: " + quadrant + " name: " + name);
            VelocityToggle();
        }
    }
}
