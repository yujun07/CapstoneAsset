using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class SignUp : MonoBehaviour
{
    private string server = "172.28.1.211";
    private string database = "UnityLoginDB";
    private string user = "root";
    private string password = "0000";

    public TMP_Text ID;
    public TMP_Text PW;
    public TMP_Text NickName;
    private string CleanID;
    private string CleanPW;
    private string CleanNick;

    public TMP_Text ErrorText;

    public GameObject EnterOK;

    private MySqlConnection conn;

    private string IDpattern = "^[a-zA-Z0-9]*$";
    private string PWpattern = @"^[a-zA-Z0-9!""#$%&'()*+,\-./:<>?@[\\\]^_`{|}~]*$";

    private void Start()
    {
        string connStr = $"Server={server};Database={database};User={user};Password={password};";
        conn = new MySqlConnection(connStr);

        if (conn != null) conn.Open();
        else Debug.Log("SQL Error");
    }

    public void OnClickOk()
    {
        Debug.Log(ID.text);
        Debug.Log(PW.text);

        CleanInput();

        if (ID.text.Length == 0 || PW.text.Length == 0)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please enter both ID and password";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (ID.text.Length < 4 || ID.text.Length > 20)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please set your ID to be between 4 and 20 characters.";   
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (PW.text.Length < 8 || PW.text.Length > 32)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please set your password to be between 8 and 32 characters.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (NickName.text.Length < 2)
        {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "Please set your nickname to at least 2 characters.";
            StopAllCoroutines();
            StartCoroutine(ErrorField());
        }
        else if (NickName.text.Length > 16)
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

    public bool IDTest()
    {
        // 정규식으로 검사
        if (Regex.IsMatch(CleanID, IDpattern))
        {
            Debug.Log("Valid input");
            return true;
        }
        else
        {
            Debug.Log("Invalid input");
            return false;
        }
    }

    public bool PWTest()
    {
        // 정규식으로 검사
        if (Regex.IsMatch(CleanPW, PWpattern))
        {
            Debug.Log("Valid input");
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
        try
        {
            MySqlCommand cmd = new MySqlCommand("RegisterUser", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_username", CleanID);
            cmd.Parameters.AddWithValue("@p_password", CleanPW);
            cmd.Parameters.AddWithValue("@p_nickname", CleanNick);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Login();
            }
            else
            {
                Debug.Log("Registration failed. No rows were affected.");
            }
        }
        catch (MySqlException ex)
        {
            Debug.Log("Registration failed: " + ex.Message);
        }
    }


    public void Login()
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand("LoginUser", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_username", CleanID);
            cmd.Parameters.AddWithValue("@p_password", CleanPW);

            // Output parameter
            MySqlParameter nicknameParam = new MySqlParameter("@p_nickname", MySqlDbType.VarChar);
            nicknameParam.Direction = System.Data.ParameterDirection.Output;
            nicknameParam.Size = 50;
            cmd.Parameters.Add(nicknameParam);

            cmd.ExecuteNonQuery();
            string nickname = nicknameParam.Value.ToString();

            if (string.IsNullOrEmpty(nickname))
            {
                Debug.Log("Login failed.");
            }
            else
            {
                EnterOK.GetComponent<bl_LobbyUI>().LogInName(nickname);

                Debug.Log("Login successful! Nickname: " + nickname);
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Login failed: " + ex.Message);
        }
    }
}
