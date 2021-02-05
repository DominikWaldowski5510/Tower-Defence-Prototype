using UnityEngine;
using UnityEngine.SceneManagement;

//Handles pausing of the game in the game scene
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Transform pausePanel = null;                   //panel that gets displayed when the game pauses
    public static PauseMenu instance;                                       //refenrece to this class
    private bool isPaused;                                 //for checking if the game is paused used mainly for things that dont get affected by time.deltatime like invoke function
    [SerializeField] private Transform optionsPanel = null;
    public bool IsPaused { get => isPaused; }

    //sets up the instance
    private void Awake()
    {
        instance = this;
    }

    //sets up the default panels and pause state
    private void Start()
    {
        ReturnFromOptions();
        Resume();
    }

    //handles pauseing when the P button is selected
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.gameObject.activeInHierarchy == true)
            {
                //game is currently paused
                Resume();
            }
            else
            {
                //game is not paused
                pausePanel.gameObject.SetActive(true);
                isPaused = true;
            }
        }

    }

    //resumes the game
    public void Resume()
    {
        pausePanel.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
        isPaused = false;
    }

    //button that handles returning to main menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    //opens options panel
    public void OpenOptions()
    {
        optionsPanel.gameObject.SetActive(true);
    }

    //closes options and opens pause menu again
    public void ReturnFromOptions()
    {
        optionsPanel.gameObject.SetActive(false);
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
