using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabArr : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;

    private void Update()
    {
        if (Check() != -1)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Move(gameObjects[Check()], Check() - 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                Move(gameObjects[Check()], Check() + 1);
            }
        }
    }

    private int Check()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].GetComponent<TMP_InputField>() != null && gameObjects[i].GetComponent<TMP_InputField>().isFocused)
            {
                return i;
            }
            else if (gameObjects[i].GetComponent<Button>() && EventSystem.current.currentSelectedGameObject == gameObjects[i].gameObject)
            {
                return i;
            }
        }

        return -1;
    }

    private void Move(GameObject MoveObj, int i)
    {

        if (i == gameObjects.Length) i = 0;
        if (i == -1) i = gameObjects.Length - 1;

        if ((MoveObj.GetComponent<TMP_InputField>() != null && MoveObj.GetComponent<TMP_InputField>().isFocused) || (MoveObj.GetComponent<Button>() && EventSystem.current.currentSelectedGameObject == MoveObj.gameObject))
        {
            if (gameObjects[i].GetComponent<TMP_InputField>() != null)
            {
                gameObjects[i].GetComponent<TMP_InputField>().Select();
            }
            else if (gameObjects[i].GetComponent<Button>() != null)
            {
                gameObjects[i].GetComponent<Button>().Select();
            }
        }
    }
}
