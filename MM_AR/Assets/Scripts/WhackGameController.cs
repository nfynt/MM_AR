using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMAR;

using DLog = MMAR.LogManager;

public class WhackGameController : Singleton<WhackGameController> {

    public delegate void GameStarted();
    public delegate void GamePaused();
    public delegate void GameEnded(bool win);
    public delegate void GameReset();
    public delegate void DestroyedEnemy();
    public static event GameStarted gStarted;
    public static event GameStarted gPaused;
    public static event GameEnded gEnded;
    public static event GameReset gReset;
    public static event DestroyedEnemy dEnemy;

    public GameObject mushroomPrefab;
    public GameObject mushroomContainer;
    public TextMesh notificationTxt;

    private int currScore;
    private GameObject currMush;
    private bool pausedState;
    public bool gameStarted = false;
    //private InputController inputCtrl;

    #region getter setter

    public int CurrentScore
    {
        get
        {
            return currScore;
        }
        set
        {
            currScore = value;
        }
    }

    #endregion

    public void EnableGameScene(bool scanningDone)
    {
        notificationTxt.text = "Scanning...";
        gameStarted = scanningDone;

        if (gameStarted)
            Invoke("StartGame", 1.4f);
    }

    public void StartGame()
    {
        if (gStarted != null)
            gStarted.Invoke();

        currScore = 0;
        notificationTxt.text = "Score: " + currScore.ToString();
        gameStarted = true;
        Invoke("SpawnTarget", 2f);
    }

    private void Update()
    {
        if (Input.touchCount > 1)
            StartGame();
    }

    public void PauseResumeGame(bool pause=false)
    {
        if (gPaused != null && pause)
            gPaused.Invoke();
        
        if (!(pause && pausedState))    //NAND
        {
            if (currMush != null)
            {
                currMush.GetComponent<MushroomController>().TogglePauseState(pause);
                currMush.SetActive(!pause);
                pausedState = pause;
            }
        }
    }

    void SpawnTarget(Vector3 wrldPos, Quaternion wrldRot)
    {
        currMush = Instantiate(mushroomPrefab, wrldPos, wrldRot,mushroomContainer.transform) as GameObject;
        currMush.GetComponent<MushroomController>().died += KilledMushroom;
        currMush.transform.LookAt(InputController.Instance.MainCamera.transform, Vector3.up);
    }

    void SpawnTarget()
    {
        SpawnTarget(InputController.Instance.GetRandomPositionInFOV(), Quaternion.identity);
    }

    private void KilledMushroom()
    {
        currScore++;
        notificationTxt.text = "Score: " + currScore.ToString();
        if (dEnemy != null)
            dEnemy.Invoke();

        if(currScore>=10)
        {
            EndGame(true);
            return;
        }

        Invoke("SpawnTarget", 2f);
    }

    public void ResetGame()
    {
        if (gReset != null)
            gReset.Invoke();
    }

    public void EndGame(bool win=false)
    {
        if (gEnded != null)
            gEnded.Invoke(win);

        if(win)
        {
            DLog.Log("Won");
        }
        else
        {
            DLog.Log("Lost");
        }
    }
}
