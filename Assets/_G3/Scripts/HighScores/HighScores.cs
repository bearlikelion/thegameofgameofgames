using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
        public int score;
        public int seconds;
        public string text;
        public string date;
    }

    public GameObject scoreEntry, content, loading;

    private string leaderboard = "";
    private GameManager _gameManager;

    void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager.playerName != "") {
            Debug.Log("Submit Scores");
            StartCoroutine(SendScores(_gameManager.Guid, _gameManager.correct, _gameManager.speed, _gameManager.playerName));
        } else {
            StartCoroutine(LoadScores());
        }
    }

    void DisplayScores () {
        Rootobject scores = new Rootobject();
        scores = JsonUtility.FromJson<Rootobject>(leaderboard);

        if (scores.dreamlo.leaderboard.entry != null) {
            loading.SetActive(false);

            List<Entry> entries = new List<Entry>();

            foreach (Entry player in scores.dreamlo.leaderboard.entry) {
                entries.Add(player);
            }

            entries = entries.OrderByDescending(x => x.score).ThenByDescending(x => x.seconds).ToList();

            Debug.Log(entries.Count);

            foreach (Entry player in entries) {
                GameObject childScore = Instantiate(scoreEntry, content.transform);
                childScore.transform.Find("Name").GetComponent<Text>().text = player.text;
                childScore.transform.Find("Correct").GetComponent<Text>().text = player.score.ToString();
                childScore.transform.Find("Speed").GetComponent<Text>().text = player.seconds.ToString();

                if (player.text == _gameManager.Guid) {
                    childScore.GetComponent<Image>().color = new Color32(80, 160, 89, 200);
                }
            }
        } else {
            loading.GetComponent<Text>().text = "No Highscores";
        }
    }

    public void PlayAgain() {
        _gameManager.correct = 0;
        _gameManager.strikes = 0;

        if (_gameManager.playerName != "") {
            SceneManager.LoadScene("Questions");
        } else {
            SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator SendScores (string guid, int playerScore, int speed, string playerName) {
        string url = "https://dreamlo.com/lb/" + Secret.PrivateKey + "/add-json/" + guid + "/" + playerScore + "/" + speed + "/" + playerName;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            yield return webRequest.SendWebRequest();
            leaderboard = webRequest.downloadHandler.text;
            DisplayScores();
        }
    }

    IEnumerator LoadScores () {
        string url = "https://dreamlo.com/lb/" + Secret.PublicKey + "/json";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            yield return webRequest.SendWebRequest();
            leaderboard = webRequest.downloadHandler.text;
            Debug.Log("Got scores");
            DisplayScores();
        }
    }
}
