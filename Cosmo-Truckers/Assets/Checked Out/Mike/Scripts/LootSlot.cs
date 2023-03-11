using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    [SerializeField] Image myImage;
    [SerializeField] TextMeshProUGUI myText;

    public void ToggleImage(bool toggle) { myImage.enabled = toggle; }
    public void SetImage(Sprite image) { myImage.sprite = image; }
    public void SetMyText(string text) { myText.text = text; }
}
