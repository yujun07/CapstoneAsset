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

    // TMP_InputField�� ��Ŀ���� �������� �� ȣ��Ǵ� �Լ�
    public void OnSelect(BaseEventData eventData)
    {
        targetImage.sprite = SelSprite;
    }

    // TMP_InputField���� ��Ŀ���� ����� �� ȣ��Ǵ� �Լ�
    public void OnDeselect(BaseEventData eventData)
    {
        targetImage.sprite = UnSelSprite;
    }
}
