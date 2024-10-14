using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextForcus : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TMP_InputField tmpInputField;
    public Image targetImage;
    public Sprite UnSelSprite;
    public Sprite SelSprite;

    // TMP_InputField에 포커스가 맞춰졌을 때 호출되는 함수
    public void OnSelect(BaseEventData eventData)
    {
        targetImage.sprite = SelSprite;
    }

    // TMP_InputField에서 포커스가 벗어났을 때 호출되는 함수
    public void OnDeselect(BaseEventData eventData)
    {
        targetImage.sprite = UnSelSprite;
    }
}
