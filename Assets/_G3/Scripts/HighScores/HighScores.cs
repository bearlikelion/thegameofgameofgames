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
        public string score;
        public string seconds;
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
        loading.SetActive(false);

        foreach (Entry player in scores.dreamlo.leaderboard.entry) {
            GameObject childScore = Instantiate(scoreEntry, content.transform);
            // TODO: Populate score entry prefab
            childScore.transform.Find("Name").GetComponent<Text>().text = player.text;
            childScore.transform.Find("Correct").GetComponent<Text>().text = player.score;
            childScore.transform.Find("Speed").GetComponent<Text>().text = player.seconds;

            if (player.name == _gameManager.Guid) {
                childScore.GetComponent<Image>().color = new Color32(80, 160, 89, 200);
            }
        }
    }

    void PlayAgain() {
        if (_gameManager.playerName != "") {
            SceneManager.LoadScene("Questions");
        } else {
            SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator SendScores (string guid, int playerScore, int speed, string playerName) {
        string url = "https://dreamlo.com/lb/" + Secret.PrivateKey + "/add-json-score-desc-seconds-desc/" + guid + "/" + playerScore + "/" + speed + "/" + playerName;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            yield return webRequest.SendWebRequest();
            leaderboard = webRequest.downloadHandler.text;
            DisplayScores();
        }
    }

    IEnumerator LoadScores () {
        string url = "https://dreamlo.com/lb/" + Secret.PublicKey + "/json-score-desc-seconds-desc";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            yield return webRequest.SendWebRequest();
            leaderboard = webRequest.downloadHandler.text;
            DisplayScores();
        }
    }
}
