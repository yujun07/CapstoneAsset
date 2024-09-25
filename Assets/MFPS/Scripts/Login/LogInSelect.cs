using UnityEngine;

public class LogInSelect : MonoBehaviour
{
    public GameObject LogIn;
    public GameObject SignUp;
    public GameObject guest;

    public void OnClickLogIn()
    {
        LogIn.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickSignUp()
    {
        SignUp.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickGuest()
    {
        guest.SetActive(true);
        gameObject.SetActive(false);
    }
}
