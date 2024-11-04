using UnityEngine;

public class LogInSelect : MonoBehaviour
{
    [SerializeField] private GameObject LogIn;
    [SerializeField] private GameObject SignUp;
    [SerializeField] private GameObject guest;

    private void OnEnable()
    {
        if (LoginManager.Login_Inst != null && LoginManager.Login_Inst.isLoggedIn)
        {
            gameObject.SetActive(false);
        }
        else if (LoginManager.Login_Inst != null && LoginManager.Login_Inst.isError == true)
        {
            guest.SetActive(true);
        }
    }

    public void Setting()
    {
        LoginManager.Login_Inst.isLoggedIn = false;

        LogIn.SetActive(false);
        SignUp.SetActive(false);
    }

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

    //public void OnClickGuest()
    //{
    //    guest.SetActive(true);
    //    gameObject.SetActive(false);
    //}
}
