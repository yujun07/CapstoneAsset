using System;
using System.Collections.Generic;
using UnityEngine;


public class LoginManager : MonoBehaviour
{
    public static LoginManager Login_Inst = null;
    public bool isLoggedIn;
    public bool isError;
    public string Nick = null;

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

    private void OnApplicationQuit()
    {
        if (isLoggedIn == true)
        {
            var gameDB = GameDB.GetSingleton();
            gameDB.Logout(Nick);
        }
    }
}

public class CharacterRow
{
    [PrimaryKey]
    public int num {  get; set; }
    public string ID { get; set; }
    public string PW { get; set; }
    public string Nick { get; set; }
    public int LoggedIn { get; set; }
    public override string ToString()
    {
        return string.Format("CharacterRow: {0}, {1}, {2}, {3}, {4}", num, ID, PW, Nick, LoggedIn);
    }
}

public class GameDB
{
    private static GameDB SingletonInstance = null;

    public static GameDB GetSingleton()
    {
        if (SingletonInstance == null)
        {
            SingletonInstance = new GameDB();
        }
        return SingletonInstance;
    }

    public CharacterRow GetCharacter(string id, string pw)
    {
        SQLiteConnection conn = GetConnection();
        List<CharacterRow> result = null;

        try
        {
            SQLiteCommand command = conn.CreateCommand(string.Format("SELECT num, ID, PW, Nick, LoggedIn FROM LoginUser WHERE ID = '{0}' AND PW = '{1}';", id, pw));

            result = command.ExecuteQuery<CharacterRow>();
        }
        catch (Exception ee)
        {
            Debug.LogError(ee.Message);
            Debug.LogError(ee.StackTrace);

            LoginManager.Login_Inst.isError = true;

            conn.Close();
        }

        if (result != null && result.Count > 0)
        {
            SQLiteCommand updateCommand = conn.CreateCommand(string.Format("UPDATE LoginUser SET LoggedIn = 1 WHERE ID = '{0}';", id));
            updateCommand.ExecuteNonQuery();

            return result[0];
        }

        conn.Close();

        return null;
    }

    private SQLiteConnection GetConnection()
    {
        string dbPath = Application.streamingAssetsPath + "/CapstoneFPS.db";
        SQLiteConnection conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        return conn;
    }

    public CharacterRow Logout(string Nickname)
    {
        SQLiteConnection conn = GetConnection();

        SQLiteCommand updateCommand = conn.CreateCommand(string.Format("UPDATE LoginUser SET LoggedIn = 0 WHERE Nick = '{0}';", Nickname));
        updateCommand.ExecuteNonQuery();

        conn.Close();

        return null;
    }

    public int SignUp(string id, string pw, string nick)
    {
        SQLiteConnection conn = GetConnection();
        List<CharacterRow> result = null;

        SQLiteCommand command = conn.CreateCommand(string.Format("SELECT num, ID, PW, Nick, LoggedIn FROM LoginUser WHERE ID = '{0}';", id));
        result = command.ExecuteQuery<CharacterRow>();

        if (result.Count == 0)
        {
            List<CharacterRow> result2 = null;

            SQLiteCommand updateCommand = conn.CreateCommand(string.Format("SELECT num, ID, PW, Nick, LoggedIn FROM LoginUser WHERE Nick = '{0}';", nick));
            result2 = updateCommand.ExecuteQuery<CharacterRow>();

            if (result2.Count == 0)
            {
                SQLiteCommand insertCommand = conn.CreateCommand(string.Format("INSERT INTO LoginUser (ID, PW, Nick) VALUES ('{0}', '{1}', '{2}');", id, pw, nick));
                insertCommand.ExecuteNonQuery();

                conn.Close();

                return 2;
            }
            else
            {
                conn.Close();
                return 1;
            }
        }
        else
        {
            conn.Close();
            return 0;
        }
    }
}

