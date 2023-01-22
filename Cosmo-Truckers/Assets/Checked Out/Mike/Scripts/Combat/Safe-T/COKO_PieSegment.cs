using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class COKO_PieSegment : MonoBehaviour
{    
    [SerializeField] float fillAmount = 0.1f;
    [SerializeField] GameObject lookToObj;

    Image sliceImage;

    void Start()
    {
        sliceImage = GetComponent<Image>();
        sliceImage.fillAmount = fillAmount;
        //transform.LookAt(lookToObj.transform);
        //transform.rotation = new Quaternion(0, 0, transform.rotation.z, 1);
    }
}
