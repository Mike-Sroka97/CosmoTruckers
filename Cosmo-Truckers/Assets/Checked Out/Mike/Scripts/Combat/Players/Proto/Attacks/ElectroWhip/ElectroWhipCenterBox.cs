using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroWhipCenterBox : MonoBehaviour
{
    [SerializeField] Material onMaterial;
    Material defaultMaterial; 

    ElectroWhipEnemy currentEnemy;
    SpriteRenderer mySpriteRenderer;
    bool active = false; 

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = mySpriteRenderer.material;
    }

    public void ActivateMe(ElectroWhipEnemy enemy)
    {
        if (currentEnemy != null)
        {
            currentEnemy.ResetThisEnemy(); 
        }

        currentEnemy = enemy;
        mySpriteRenderer.material = onMaterial;
        active = true; 
    }

    private void DeactivateMe()
    {
        if (currentEnemy != null)
        {
            currentEnemy.Recenter();
            currentEnemy = null;
        }

        mySpriteRenderer.material = defaultMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" && active)
        {
            active = false; 
            DeactivateMe();
        }
    }
}
