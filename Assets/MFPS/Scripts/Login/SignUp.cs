using UnityEngine;
using MySql.Data.MySqlClient;
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

    private MySqlConnection conn;

    private string IDpattern = "^[a-zA-Z0-9]*$";
    private string PWpattern = @"^[a-zA-Z0-9!""#$%&'()*+,\-./:<>?@[\\\]^_`{|}~]*$";
    private string NICKpattern = "^[a-zA-Z0-9\\s]*$";

    [SerializeField] private GameObject X;

    private void Start()
    {
        conn = Sel.GetComponent<LogInSelect>()._conn;
    }

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
        // 정규식으로 검사
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
        // 정규식으로 검사
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
            Debug.Log(ex.Number);

            if (ex.Number == 1644)
            {
                ErrorText.gameObject.SetActive(true);
                ErrorText.text = "That ID already exists. Please choose a different one.";
                StopAllCoroutines();
                StartCoroutine(ErrorField());
            }
            else if (ex.Number == 1062)
            {
                ErrorText.gameObject.SetActive(true);
                ErrorText.text = "That NickName already exists. Please choose a different one.";
                StopAllCoroutines();
                StartCoroutine(ErrorField());
            }
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
                if (conn != null)
                {
                    conn.Close();
                }

                EnterOK.GetComponent<bl_LobbyUI>().LogInName(nickname);
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Login failed: " + ex.Message);
        }
    }

    public void OnClickX()
    {
        X.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
