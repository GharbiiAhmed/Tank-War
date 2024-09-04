using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }

    private string userID;

    private void Awake()
    {
        // Implementing Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUserID(string id)
    {
        userID = id;
        Debug.Log("User ID set in SessionManager: " + userID);
    }

    public string GetUserID()
    {
        return userID;
    }
}
