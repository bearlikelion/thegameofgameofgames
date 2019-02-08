using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HighScores : MonoBehaviour {

    [System.Serializable]
    public class Rootobject {
        public Dreamlo dreamlo;
    }

    [System.Serializable]
    public class Dreamlo {
        public Leaderboard leaderboard;
    }

    [System.Serializable]
    public class Leaderboard {
        public Entry[] entry;
    }

    [System.Serializable]
    public class Entry {
        public string name;
        public string score;
        public string seconds;
        public string text;
        public string date;
    }

    public GameObject user, score, highscores, textPrefab;
    string leaderboard;

    void Start () {
        GetScores();
    }

    public void SubmitScore () {
        int playerScore = 0;
        string playerName = user.GetComponent<InputField>().text;
        int.TryParse(score.GetComponent<InputField>().text, out playerScore);

        StartCoroutine(SendScores(playerName, playerScore));
        GetScores();
    }

    public void GetScores () {
        StartCoroutine(LoadScores());
    }

    void DisplayScores () {
        Rootobject scores = new Rootobject();
        scores = JsonUtility.FromJson<Rootobject>(leaderboard);

        foreach (Transform child in highscores.transform) {
            Destroy(child.gameObject);
        }

        foreach (Entry player in scores.dreamlo.leaderboard.entry) {
            GameObject childScore = Instantiate(textPrefab, highscores.transform);
            childScore.GetComponent<Text>().text = player.name + " - " + player.score + "(" + player.seconds + ")";
        }        
    }    

    IEnumerator SendScores (string playerName, int playerScore) {
        string url = "http://dreamlo.com/lb/" + Secret.PrivateKey + "/add/" + playerName + "/" + playerScore;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            yield return webRequest.SendWebRequest();
        }
    }
    
    IEnumerator LoadScores () {
        string url = "http://dreamlo.com/lb/" + Secret.PublicKey + "/json";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            yield return webRequest.SendWebRequest();
            leaderboard = webRequest.downloadHandler.text;
            DisplayScores();
        }
    }
}
