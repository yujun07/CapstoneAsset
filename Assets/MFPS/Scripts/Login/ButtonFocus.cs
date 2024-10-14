using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFocus : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image targetImage;
    public Sprite UnSelSprite;
    public Sprite SelSprite;

    public void OnSelect(BaseEventData eventData)
    {
        targetImage.sprite = SelSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetImage.sprite = UnSelSprite;
    }

    public void OnClickX()
    {
        targetImage.sprite = UnSelSprite;
    }
}
