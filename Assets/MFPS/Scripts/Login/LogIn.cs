using MySql.Data.MySqlClient;
using System.Collections;
using TMPro;
using UnityEngine;

public class LogIn : MonoBehaviour
{
    private string server = "172.28.1.211";
    private string database = "UnityLoginDB";
    private string user = "root";
    private string password = "0000";   

    public TMP_InputField ID;
    public TMP_InputField PW;

    public TMP_Text ErrorMassage;

    public GameObject EnterName;

    private MySqlConnection conn;

    void Start()
    {
        string connStr = $"Server={server};Database={database};User={user};Password={password};";
        conn = new MySqlConnection(connStr);

        if (conn != null) conn.Open();
        else Debug.Log("SQL Error");
    }

    public void OnClickOk()
    {
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
}
