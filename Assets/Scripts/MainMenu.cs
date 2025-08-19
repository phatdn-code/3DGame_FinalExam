using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform backgroundObject;
    [SerializeField] private float rotationSpeed = 10f;

    private void Update()
    {
        // Rotate the background object continuously
        backgroundObject.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // If running in the editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
