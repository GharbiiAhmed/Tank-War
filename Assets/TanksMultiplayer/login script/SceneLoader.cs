using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // These methods can be assigned to the buttons in the Inspector
    public void LoadIntroScene()
    {
        SceneManager.LoadScene("Intro");
    }
    public void LoadSignupScene()
    {
        SceneManager.LoadScene("Signup");
    }
    public void LoadSigninScene()
    {
        SceneManager.LoadScene("Signin");
    }
    public void LoadPasswordScene()
    {
        SceneManager.LoadScene("ForgotPassword");
    }
}
