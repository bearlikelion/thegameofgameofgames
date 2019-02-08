using System.IO;
using System.Data.SQLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

    public GameObject user, score, highscores, textPrefab;
    SQLiteConnection _dbConnection; 
    private string dbPath = "./HighScores.sqlite";

    void Start () {      
        if (File.Exists(@dbPath)) {
            Debug.Log("Using saved DB");            
        } else {
            SQLiteConnection.CreateFile(dbPath);
            Debug.Log("Creating new DB");
        }

        _dbConnection = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;");
        _dbConnection.Open();

        string sql = "CREATE TABLE IF NOT EXISTS highscores (name VARCHAR(20), score INT)";
        SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
        int _response = command.ExecuteNonQuery();
        Debug.Log("Rows affected: " + _response);
        LoadScores();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SubmitScore() {
        int playerScore = 0;
        string playerName = user.GetComponent<InputField>().text;
        int.TryParse(score.GetComponent<InputField>().text, out playerScore);

        string sql = "INSERT into highscores (name, score) values ('" + playerName + "', " + playerScore + ")";
        SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
        int inserted = command.ExecuteNonQuery();
        Debug.Log(inserted + " row(s) inserted");
        LoadScores();
    }

    public void LoadScores() {
        // TODO: Cleanup list
        foreach (Transform child in highscores.transform) {
            Destroy(child.gameObject);
        }

        string sql = "select * from highscores order by score desc";
        SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read()) {
            GameObject childScore = Instantiate(textPrefab, highscores.transform);            
            childScore.GetComponent<Text>().text = "Name: " + reader["name"] + " Score: " + reader["score"];
        }
    }
}
