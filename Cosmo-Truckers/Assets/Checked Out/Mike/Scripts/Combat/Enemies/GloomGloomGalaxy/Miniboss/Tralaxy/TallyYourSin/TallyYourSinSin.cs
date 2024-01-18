using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TallyYourSinSin : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDuration;
    [SerializeField] float fireDelay;

    [Header("Art Variables")]
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Sprite[] attackSprites;
    [SerializeField] Color angryColor;
    [SerializeField] Rotator myRotator;
    [SerializeField] float angryRotateSpeed = 45f;

    float lerpTime = 0f; 
    Color lerpedColor; 
    bool lerpToColor = false; 
    Player player;

    public void Initialize()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        LerpColors();
    }

    public void FireMe()
    {
        transform.parent = null;
        transform.right = player.transform.position - transform.position;
        StartCoroutine(FireCo());
    }

    IEnumerator FireCo()
    {
        for (int i = 0; i < attackSprites.Length; i++)
        {
            spriteRenderers[i].sprite = attackSprites[i];
        }

        lerpToColor = true; 
        myRotator.RotateSpeed = angryRotateSpeed; 

        yield return new WaitForSeconds(fireDelay);

        lerpToColor = false; 
        float currentTime = 0;

        while(currentTime < moveDuration)
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    void LerpColors()
    {
        if (lerpToColor)
        {
            lerpTime += Time.deltaTime / fireDelay;

            lerpedColor = Color.Lerp(spriteRenderers[0].color, angryColor, lerpTime);

            for (int i = 0; i < attackSprites.Length; i++)
            {
                //Don't set outline sprite to new color, it will mess with hurt material
                if (i < (attackSprites.Length - 1))
                {
                    spriteRenderers[i].color = lerpedColor;
                }
            }
        }
    }
}
