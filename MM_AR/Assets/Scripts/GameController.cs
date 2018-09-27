using MMAR;
using Vuforia;
using UnityEngine;

public class GameController : DefaultTrackableEventHandler {

    public enum GameState
    {
        HOME, GAME
    };

    public WhackGameController whackController;
   // public GameObject startCanvas;
    public GameObject homeScreen;
    public GameObject gameScreen;
    public TextMesh debugTxt;
    public GameState currState;

    private bool foundTrack = false;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        if (currState == GameState.GAME)
        {
            if (whackController.gameStarted)
                whackController.PauseResumeGame(false);
            else
                whackController.StartGame();

            foundTrack = true;
        }
        debugTxt.text = "plane found";
    }

    public void TrackingFound()
    {
        debugTxt.text += base.mTrackableBehaviour.CurrentStatus.ToString();

        if(!foundTrack)
        {
            OnTrackingFound();
        }
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        if (currState == GameState.GAME)
        {
            whackController.PauseResumeGame(true);
        }
        whackController.notificationTxt.text = base.mTrackableBehaviour.CurrentStatus.ToString();
        debugTxt.text = "plane lost";
    }

    protected override void Start()
    {
        base.Start();

        InputController.Instance.MainCamera = homeScreen.transform.GetComponentInChildren<Camera>();
        homeScreen.SetActive(true);
        gameScreen.SetActive(false);
        //WhackGameController.gStarted += WhackGameController_gStarted;
        WhackGameController.gEnded += WhackGameController_gEnded;
        currState = GameState.HOME;
    }

    public void EnterGameScene()
    {
        //whackController.StartGame();
        if (base.mTrackableBehaviour == null)
            whackController.EnableGameScene(false);
        else
            whackController.EnableGameScene(base.mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED ||
                base.mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED ||
                base.mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED);
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
