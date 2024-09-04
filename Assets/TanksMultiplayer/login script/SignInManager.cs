using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro; // Include the TextMeshPro namespace
using UnityEngine.SceneManagement; // Include the SceneManagement namespace

public class SignInManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Button signInButton;
    public TextMeshProUGUI feedbackText; // TextMeshPro field for feedback

    private void Start()
    {
        signInButton.onClick.AddListener(OnSignInButtonClick);
    }

    private void OnSignInButtonClick()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(SignIn(username, password));
    }

    private IEnumerator SignIn(string username, string password)
    {
        // Create a JSON string with the username and password
        string jsonData = JsonUtility.ToJson(new SignInData(username, password));

        // Create a UnityWebRequest to send the POST request
        UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:9090/user/signin", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sign-in successful. Response: " + request.downloadHandler.text);

            try
            {
                // Parse the JSON response to get the user ID
                UserResponse response = JsonUtility.FromJson<UserResponse>(request.downloadHandler.text);
                string userId = response._id;

                // Store the user ID (e.g., in a session manager)
                SessionManager.Instance.SetUserID(userId);

                Debug.Log("User ID: " + userId);

                feedbackText.text = "Sign-in successful!"; // Feedback for successful sign-in
                feedbackText.color = Color.green;

                // Load the "Main Menu" scene
                SceneManager.LoadScene("MainMenu");
            }
            catch
            {
                feedbackText.text = "User was not found. Please check your information."; // Error feedback
                feedbackText.color = Color.red;
            }
        }
        else
        {
            Debug.LogError("Sign-in failed. Error: " + request.error);
            feedbackText.text = "Sign-in failed. Please try again.";
            feedbackText.color = Color.red;
        }
    }

    // Helper class to format the JSON request
    [System.Serializable]
    private class SignInData
    {
        public string username;
        public string password;

        public SignInData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    // Helper class to parse the JSON response
    [System.Serializable]
    private class UserResponse
    {
        public string _id;
        public string username;
        public string password;
        public string adresse;
        public string createdAt;
        public string updatedAt;
        public int victoires;
        public int defaites;
    }
}
