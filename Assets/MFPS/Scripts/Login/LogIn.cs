using MySql.Data.MySqlClient;
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

    private MySqlConnection conn;

    [SerializeField] private GameObject Remem;

    [SerializeField] private GameObject ReLogError;

    private void Awake()
    {
        Sel.GetComponent<LogInSelect>().SQLConn();

        if (!LoginManager.Login_Inst.isError)
        {
            string ReID = null;
            string RePW = null;

            if (PlayerPrefs.GetString("ID", ReID) != "" && PlayerPrefs.GetString("PW", RePW) != "")
            {
                ID.text = PlayerPrefs.GetString("ID");
                PW.text = PlayerPrefs.GetString("PW");
                Sel.SetActive(false);

                conn = Sel.GetComponent<LogInSelect>()._conn;

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
        Sel.GetComponent<LogInSelect>().SQLConn();
        conn = Sel.GetComponent<LogInSelect>()._conn;

        Login();
    }

    public void Login()
    {
        MySqlCommand cmd = new MySqlCommand("LoginUser", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@p_username", ID.text);
        cmd.Parameters.AddWithValue("@p_password", PW.text);

        MySqlParameter nicknameParam = new MySqlParameter("@p_nickname", MySqlDbType.VarChar);
        nicknameParam.Direction = System.Data.ParameterDirection.Output;
        nicknameParam.Size = 16;
        cmd.Parameters.Add(nicknameParam);

        cmd.ExecuteNonQuery();
        string nickname = nicknameParam.Value.ToString();

        if (string.IsNullOrEmpty(nickname))
        {
            ErrorMassage.gameObject.SetActive(true);
            ErrorMassage.text = "The username or password is incorrect.";
            StopAllCoroutines();
            StartCoroutine(ErrorText());
        }
        else
        {
            if (conn != null)
            {
                conn.Close();
            }

            if (Remem.GetComponent<Remember>().Check())
            {
                PlayerPrefs.SetString("ID", ID.text);
                PlayerPrefs.SetString("PW", PW.text);
                PlayerPrefs.Save();
            }

            LoginManager.Login_Inst.isLoggedIn = true;
            conn.OpenAsync();
            
            using (var command = conn.CreateCommand())
            {
                command.CommandText = "UPDATE Äõ¸®¹® ÀÛ¼º";
                command.ExecuteNonQueryAsync();
            }
            EnterName.GetComponent<bl_LobbyUI>().LogInName(nickname);
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
