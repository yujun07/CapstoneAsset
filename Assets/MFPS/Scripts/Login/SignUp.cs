using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class SignUp : MonoBehaviour
{
    [SerializeField] private GameObject Sel;

    [SerializeField] private TMP_Text ID;
    [SerializeField] private TMP_Text PW;
    [SerializeField] private TMP_Text NickName;

    private string CleanID;
    private string CleanPW;
    private string CleanNick;

    [SerializeField] private TMP_Text ErrorText;

    [SerializeField] private GameObject EnterOK;

    private string IDpattern = "^[a-zA-Z0-9]*$";
    private string PWpattern = @"^[a-zA-Z0-9!""#$%&'()*+,\-./:<>?@[\\\]^_`{|}~]*$";
    private string NICKpattern = "^[a-zA-Z0-9\\s]*$";

    public void OnClickOk()
    {
        CleanInput();

        if (CleanID.Length == 0 || CleanPW.Length == 0)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please enter both ID and password";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (CleanID.Length < 4 || CleanID.Length > 20)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please set your ID to be between 4 and 20 characters.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (CleanPW.Length < 8 || CleanPW.Length > 32)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please set your password to be between 8 and 32 characters.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (CleanNick.Length < 2)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please set your nickname to at least 2 characters.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (CleanNick.Length > 16)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please set your nickname to no more than 16 characters.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (!IDTest())
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "ID must include letters and numbers.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (!PWTest())
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Password must include letters, numbers, and special characters.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (!NICKTest())
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "NickName must include letters, numbers, and spaces.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else
        {
            Register();
        }
    }

    private void CleanInput()
    {
        CleanID = Regex.Replace(ID.text, @"[\u200B-\u200D\uFEFF]", "");
        CleanPW = Regex.Replace(PW.text, @"[\u200B-\u200D\uFEFF]", "");
        CleanNick = Regex.Replace(NickName.text, @"[\u200B-\u200D\uFEFF]", "");
    }

    private bool IDTest()
    {
        if (Regex.IsMatch(CleanID, IDpattern))
        {
            return true;
        }
        else
        {
            Debug.Log("Invalid input");
            return false;
        }
    }

    private bool PWTest()
    {
        if (Regex.IsMatch(CleanPW, PWpattern))
        {
            return true;
        }
        else
        {
            Debug.Log("Invalid input");
            return false;
        }
    }

    private bool NICKTest()
    {
        if (Regex.IsMatch(CleanNick, NICKpattern))
        {
            return true;
        }
        else
        {
            Debug.Log("Invalid input");
            return false;
        }
    }

    IEnumerator ErrorField()
    {
        float A = 0f;
        while (A < 3)
        {
            A += Time.deltaTime;
            yield return null;
        }

        ErrorText.gameObject.SetActive(false);
    }

    public void Register()
    {
        var gameDB = GameDB.GetSingleton();
        int a = gameDB.SignUp(CleanID, CleanPW, CleanNick);

        if (a == 2)
        {
            Login();
        }
        else if (a == 1)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Nickname already exists.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (a == 0)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "ID already exists.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
    }


    public void Login()
    {
        var gameDB = GameDB.GetSingleton();
        var character = gameDB.GetCharacter(CleanID, CleanPW);

        if (character != null)
        {
            LoginManager.Login_Inst.isLoggedIn = true;
            LoginManager.Login_Inst.Nick = character.Nick;

            EnterOK.GetComponent<bl_LobbyUI>().LogInName(character.Nick);
        }
        else
        {
            Debug.Log("LoginError");
        }
    }

    public void OnClickX()
    {
        Sel.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
