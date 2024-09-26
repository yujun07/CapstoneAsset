using MySql.Data.MySqlClient;
using System;
using TMPro;
using UnityEngine;

public class LogInSelect : MonoBehaviour
{
    private string server = "172.28.1.211";
    private string database = "UnityLoginDB";
    private string user = "root";
    private string password = "0000";


    [SerializeField] private GameObject LogIn;
    [SerializeField] private GameObject SignUp;
    [SerializeField] private GameObject guest;

    public MySqlConnection _conn;

    [SerializeField] private GameObject ErrorPanel;
    [SerializeField] private TMP_Text ErrorText;

    void Awake()
    {
        string connStr = $"Server={server};Database={database};User={user};Password={password};";
        _conn = new MySqlConnection(connStr);

        try
        {
            _conn.Open();
        }
        catch (MySqlException ex)
        {
            ErrorPanel.SetActive(true);
            ErrorText.text = ex.Message;    
        }
        catch (Exception ex)
        {
            ErrorPanel.SetActive(true);
            ErrorText.text = ex.Message;
        }
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

    public void OnClickGuest()
    {
        guest.SetActive(true);
        gameObject.SetActive(false);
    }
}
