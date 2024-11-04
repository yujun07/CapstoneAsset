using System.Collections;
using TMPro;
using UnityEngine;

public class LogIn : MonoBehaviour
{
    [SerializeField] private GameObject Sel;

    [SerializeField] private TMP_InputField ID;
    [SerializeField] private TMP_InputField PW;

    [SerializeField] private TMP_Text ErrorMassage;

    [SerializeField] private GameObject EnterName;

    [SerializeField] private GameObject Remem;

    [SerializeField] private GameObject ReLogError;

    private void Awake()
    {
        if (!LoginManager.Login_Inst.isError)
        {
            string ReID = null;
            string RePW = null;

            if (PlayerPrefs.GetString("ID", ReID) != "" && PlayerPrefs.GetString("PW", RePW) != "")
            {
                ID.text = PlayerPrefs.GetString("ID");
                PW.text = PlayerPrefs.GetString("PW");
                Sel.SetActive(false);

                Login();

                if (!LoginManager.Login_Inst.isLoggedIn)
                {
                    ReLogError.SetActive(true);
                    PlayerPrefs.SetString("ID", "");
                    PlayerPrefs.SetString("PW", "");
                    PlayerPrefs.Save();
                }
            }
            else
            {
                LoginManager.Login_Inst.isLoggedIn = false;
                Sel.SetActive(true);
            }
        }
    }

    public void OnClickOk()
    {
        Login();
    }

    private void Login()
    {
        var gameDB = GameDB.GetSingleton();
        var character = gameDB.GetCharacter(ID.text, PW.text);

        if (character != null)
        {
            if (character.LoggedIn == 1)
            {
                ErrorMassage.gameObject.SetActive(true);
                ErrorMassage.text = "An account is already logged in.";
                StopAllCoroutines();
                StartCoroutine(ErrorText());

                return;
            }

            Debug.Log("로그인 성공");

            if (Remem.GetComponent<Remember>().Check())
            {
                PlayerPrefs.SetString("ID", ID.text);
                PlayerPrefs.SetString("PW", PW.text);
                PlayerPrefs.Save();
            }
            
            LoginManager.Login_Inst.isLoggedIn = true;
            LoginManager.Login_Inst.Nick = character.Nick;

            EnterName.GetComponent<bl_LobbyUI>().LogInName(character.Nick);
        }
        else
        {
            ErrorMassage.gameObject.SetActive(true);
            ErrorMassage.text = "The ID or password is incorrect.";
            StopAllCoroutines();
            StartCoroutine(ErrorText());
        }
    }

    IEnumerator ErrorText()
    {
        float A = 0f;
        while (A < 3)
        {
            A += Time.deltaTime;
            yield return null;
        }

        ErrorMassage.gameObject.SetActive(false);
    }

    public void OnClickX()
    {
        Sel.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }
}