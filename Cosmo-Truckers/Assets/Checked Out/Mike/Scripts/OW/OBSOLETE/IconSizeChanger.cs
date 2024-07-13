using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconSizeChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float maxScale;
    [SerializeField] float scaleSpeed;
    [SerializeField] GameObject textBox;

    float startScale;

    private void Start()
    {
        startScale = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        textBox.SetActive(true);
        StartCoroutine(GrowIcon());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        textBox.SetActive(false);
        StartCoroutine(ShrinkIcon());
    }

    IEnumerator GrowIcon()
    {
        while(transform.localScale.x < maxScale)
        {
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * scaleSpeed, transform.localScale.y + Time.deltaTime * scaleSpeed, transform.localScale.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localScale = new Vector3(maxScale, maxScale, transform.localScale.z);
    }

    IEnumerator ShrinkIcon()
    {
        while (transform.localScale.x > startScale)
        {
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * scaleSpeed, transform.localScale.y - Time.deltaTime * scaleSpeed, transform.localScale.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localScale = new Vector3(startScale, startScale, transform.localScale.z);
    }
}
