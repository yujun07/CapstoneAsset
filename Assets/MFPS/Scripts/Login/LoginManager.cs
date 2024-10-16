using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Login_Inst = null;

    public bool isLoggedIn;

    public bool isError;

    private void Awake()
    {
        if (Login_Inst == null)
        {
            Login_Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
