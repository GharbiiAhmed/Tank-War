using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ForgotPassword : MonoBehaviour
{
    public InputField emailInputField; // Reference to the input field for the email
    public Button confirmButton; // Reference to the confirm button

    void Start()
    {
        // Add a listener to the button click event
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    void OnConfirmButtonClick()
    {
        string email = emailInputField.text;

        // Check if the email field is not empty
        if (!string.IsNullOrEmpty(email))
        {
            StartCoroutine(PostForgotPassword(email));
        }
        else
        {
            Debug.Log("Email field is empty.");
        }
    }

    IEnumerator PostForgotPassword(string email)
    {
        // Create a form and add the email field to it
        WWWForm form = new WWWForm();
        form.AddField("adresse", email);

        // Create the request to the forgot password endpoint
        UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:9090/user/forgot-password", form);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Forgot password request sent successfully.");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }
}
