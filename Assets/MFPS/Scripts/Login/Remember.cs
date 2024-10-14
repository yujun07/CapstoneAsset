using TMPro;
using UnityEngine;

public class Remember : MonoBehaviour
{
    private bool isChecked = false;

    [SerializeField] private TMP_Text text;

    public void OnClickCheck()
    {
        if (isChecked)
        {
            isChecked = false;
            text.text = "";
        }
        else
        {
            isChecked = true;
            text.text = "V";
        }
    }

    public bool Check()
    {
        return isChecked;
    }
}
