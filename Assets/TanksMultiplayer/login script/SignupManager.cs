using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class SignupManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public InputField adresseField;

    public Button signupButton;

    void Start()
    {
        signupButton.onClick.AddListener(Signup);
    }

    public void Signup()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        string adresse = adresseField.text;

        StartCoroutine(SendSignupRequest(username, password, adresse));
    }

    IEnumerator SendSignupRequest(string username, string password, string adresse)
    {
        SignupData data = new SignupData
        {
            username = username,
            password = password,
            adresse = adresse
        };

        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:9090/user/signup", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Signup successful: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Signup failed: " + request.error);
        }
    }

    [System.Serializable]
    public class SignupData
    {
        public string username;
        public string password;
        public string adresse;
    }
}
