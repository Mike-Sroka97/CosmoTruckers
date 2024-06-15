using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainHubButton : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    [SerializeField] float speed;
    [SerializeField] float xMax;
    float xMin;

    private void Start()
    {
        xMin = transform.localPosition.x;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(MoveMe(true));
    }
    public void OnDeselect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(MoveMe(false));
    }

    IEnumerator MoveMe(bool right)
    {
        if(right)
        {
            while(transform.localPosition.x < xMax)
            {
                transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.localPosition = new Vector3(xMax, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            while (transform.localPosition.x > xMin)
            {
                transform.localPosition += new Vector3(-speed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.localPosition = new Vector3(xMin, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
