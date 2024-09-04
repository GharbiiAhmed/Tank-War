using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Networking;
using System.Collections;

namespace TanksMP
{
    public class UIGame : MonoBehaviourPunCallbacks
    {
        public UIJoystick[] controls; // Mobile joystick controls
        public Slider[] teamSize; // UI sliders for team sizes
        public Text[] teamScore; // UI texts for team scores
        public Text[] killCounter; // UI kill/death counter
        public GameObject aimIndicator; // Crosshair for aiming
        public Text deathText; // Death text
        public Text spawnDelayText; // Respawn delay text
        public Text gameOverText; // Game over text
        public GameObject gameOverMenu; // Game over UI menu

        void Start()
        {
#if !UNITY_EDITOR && (UNITY_STANDALONE || UNITY_WEBGL)
                ToggleControls(false);
#endif

#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBGL
            if (aimIndicator != null)
            {
                Transform indicator = Instantiate(aimIndicator).transform;
                indicator.SetParent(GameManager.GetInstance().localPlayer.shotPos);
                indicator.localPosition = new Vector3(0f, 0f, 3f);
            }
#endif

            AudioManager.PlayMusic(1);
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            OnTeamSizeChanged(PhotonNetwork.CurrentRoom.GetSize());
            OnTeamScoreChanged(PhotonNetwork.CurrentRoom.GetScore());
        }

        public void OnTeamSizeChanged(int[] size)
        {
            for (int i = 0; i < size.Length; i++)
                teamSize[i].value = size[i];
        }

        public void OnTeamScoreChanged(int[] score)
        {
            for (int i = 0; i < score.Length; i++)
            {
                if (score[i] > int.Parse(teamScore[i].text))
                    teamScore[i].GetComponent<Animator>().Play("Animation");
                teamScore[i].text = score[i].ToString();
            }
        }

        public void ToggleControls(bool state)
        {
            for (int i = 0; i < controls.Length; i++)
                controls[i].gameObject.SetActive(state);
        }

        public void SetDeathText(string playerName, Team team)
        {
#if UNITY_EDITOR || (!UNITY_STANDALONE && !UNITY_WEBGL)
            ToggleControls(false);
#endif
            deathText.text = "KILLED BY\n<color=#" + ColorUtility.ToHtmlStringRGB(team.material.color) + ">" + playerName + "</color>";
        }

        public void SetSpawnDelay(float time)
        {
            spawnDelayText.text = Mathf.Ceil(time).ToString();
        }

        public void DisableDeath()
        {
#if UNITY_EDITOR || (!UNITY_STANDALONE && !UNITY_WEBGL)
            ToggleControls(true);
#endif
            deathText.text = string.Empty;
            spawnDelayText.text = string.Empty;
        }

        public void DisplayGameOver(int winningTeamIndex)
        {
            bool isVictory = GameManager.GetInstance().localPlayer.GetView().GetTeam() == winningTeamIndex;

            // Send the game result to the backend
            SendGameResult(isVictory);

            // Display game over UI
            SetGameOverText(GameManager.GetInstance().teams[winningTeamIndex]);
            ShowGameOver();
        }

        public void SetGameOverText(Team team)
        {
#if UNITY_EDITOR || (!UNITY_STANDALONE && !UNITY_WEBGL)
            ToggleControls(false);
#endif
            gameOverText.text = "TEAM <color=#" + ColorUtility.ToHtmlStringRGB(team.material.color) + ">" + team.name + "</color> WINS!";
        }

        public void SendGameResult(bool isVictory)
        {
            string result = isVictory ? "win" : "lose";
            string userId = SessionManager.Instance.GetUserID(); // Get user ID from session management

            StartCoroutine(SendResultCoroutine(userId, result));
        }

        IEnumerator SendResultCoroutine(string userId, string result)
        {
            string url = "http://localhost:9090/user/" + userId + "/game-result";

            // Create JSON data
            string jsonData = "{\"result\":\"" + result + "\"}";

            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Game result updated successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error updating game result: " + request.error);
            }
        }

        public void ShowGameOver()
        {
            gameOverText.gameObject.SetActive(false);
            gameOverMenu.SetActive(true);
        }

        public void Restart()
        {
            GameObject gObj = new GameObject("RestartNow");
            gObj.AddComponent<UIRestartButton>();
            DontDestroyOnLoad(gObj);
            Disconnect();
        }

        public void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(NetworkManagerCustom.GetInstance().offlineSceneIndex);
        }
    }
}
