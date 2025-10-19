using UnityEngine;

public class PapMovePlayer : MonoBehaviour
{
    [SerializeField] PaPConveyor myConveyor;

    Rigidbody2D playerBody;
    SafeTINA player;

    private void Start()
    {
        player = FindObjectOfType<SafeTINA>();
        playerBody = player.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.D) && !player.GetIsJumping())
            {
                playerBody.velocity = new Vector2(player.GetMoveSpeed() * 1.5f, playerBody.velocity.y);
            }
            else if(Input.GetKey(KeyCode.A) && !player.GetIsJumping())
            {
                playerBody.velocity = new Vector2(-player.GetMoveSpeed() / 2, playerBody.velocity.y);
            }
            else 
            {
                playerBody.velocity = new Vector2(myConveyor.GetMoveSpeed(), playerBody.velocity.y);
            }
        }
    }
}
