using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Login_Inst = null;

    public bool isLoggedIn;

    public bool isError;

    private string server = "172.28.11.208";
    private string database = "UnityLoginDB";
    private string user = "unity";
    private string password = "0000";

    public MySqlConnection _conn;

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

    public void SQLConn()
    {
        string connStr = $"Server={server};Database={database};User={user};Password={password};";
        _conn = new MySqlConnection(connStr);

        try
        {
            _conn.Open();
        }
        catch (MySqlException)
        {
            isError = true;
        }
        catch (Exception)
        {
            isError = true;
        }
    }

    public void LogoutUser()
    {
        SQLConn();

        if (_conn == null || _conn.State != System.Data.ConnectionState.Open)
        {
            Debug.LogError("No database connection available.");
            return;
        }

        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            Debug.LogError("No valid user to log out.");
            return;
        }

        try
        {
            MySqlCommand cmd = new MySqlCommand("LogoutUser", _conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_nickname", PhotonNetwork.NickName.ToString());

            cmd.ExecuteNonQuery();
            Debug.Log("User logged out successfully.");
        }
        catch (MySqlException ex)
        {
            Debug.Log("MySQL error while logging out: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("General error while logging out: " + ex.Message);
        }
    }

    // 애플리케이션 종료 시 호출
    private void OnApplicationQuit()
    {
        if (isLoggedIn)
        {
            LogoutUser();
        }

        if (_conn != null && _conn.State == System.Data.ConnectionState.Open)
        {
            _conn.Close();
        }
    }
}
