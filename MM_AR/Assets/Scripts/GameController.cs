using MMAR;
using Vuforia;
using UnityEngine;

public class GameController : MonoBehaviour {

    public enum GameState
    {
        HOME, GAME
    };

    public WhackGameController whackController;
   // public GameObject startCanvas;
    public GameObject homeScreen;
    public GameObject gameScreen;
    //public TextMesh debugTxt;
    public GameState currState;

    private bool foundTrack = false;

    private void OnEnable()
    {
        OnTrackingFound();
    }

    private void OnDisable()
    {
        OnTrackingLost();
    }

    public void OnTrackingFound()
    {
        if (currState == GameState.GAME)
        {
            if (whackController.gameStarted)
                whackController.PauseResumeGame(false);
            else
                whackController.StartGame();

            foundTrack = true;
        }
        //debugTxt.text = "plane found";
    }

    public void OnTrackingLost()
    {
        
            whackController.PauseResumeGame(true);
        
        whackController.notificationTxt.text = "Lost tracking";
        //debugTxt.text = "plane lost";
    }

    void Start()
    {
        InputController.Instance.MainCamera = homeScreen.transform.GetComponentInChildren<Camera>();
        homeScreen.SetActive(true);
        gameScreen.SetActive(false);
        //WhackGameController.gStarted += WhackGameController_gStarted;
        WhackGameController.gEnded += WhackGameController_gEnded;
        WhackGameController.gReset += WhackGameController_gReset;
        currState = GameState.HOME;
    }

    private void WhackGameController_gReset()
    {
        
    }

    public void EnterGameScene(bool start=true)
    {
        //whackController.StartGame();

        whackController.EnableGameScene(start);

        homeScreen.SetActive(false);
        gameScreen.SetActive(true);
        InputController.Instance.MainCamera = gameScreen.transform.GetComponentInChildren<Camera>();
        currState = GameState.GAME;
    }

    private void WhackGameController_gStarted()
    {
        homeScreen.SetActive(false);
        gameScreen.SetActive(true);
        InputController.Instance.MainCamera = gameScreen.transform.GetComponentInChildren<Camera>();
    }

    private void WhackGameController_gEnded(bool win)
    {
        homeScreen.SetActive(true);
        gameScreen.SetActive(false);
        InputController.Instance.MainCamera = homeScreen.transform.GetComponentInChildren<Camera>();
        currState = GameState.HOME;
    }
}
