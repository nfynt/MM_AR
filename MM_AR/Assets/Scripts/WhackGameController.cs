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

    public int currScore;

    public GameObject planeFinder;
    public GameObject resetCanvas;

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
        resetCanvas.SetActive(false);
        //if (gameStarted)
          //  Invoke("StartGame", 1.4f);
    }

    public void StartGame()
    {
        if (gStarted != null)
            gStarted.Invoke();

        currScore = 0;
        notificationTxt.text = "Score: " + currScore.ToString();
        gameStarted = true;
        Invoke("SpawnTarget", 2f);

        planeFinder.SetActive(false);

        Debug.Log("Game started");
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
        if (currMush != null && currMush.GetComponent<MushroomController>().IsAlive)
            return;

        currMush = Instantiate(mushroomPrefab, wrldPos, wrldRot,mushroomContainer.transform) as GameObject;
        currMush.GetComponent<MushroomController>().gCtrl = this;
        Quaternion lookang = Quaternion.LookRotation(InputController.Instance.MainCamera.transform.position - currMush.transform.position);
        currMush.transform.rotation = Quaternion.Euler(0, lookang.eulerAngles.y, 0);
    }

    void SpawnTarget()
    {
        SpawnTarget(InputController.Instance.GetRandomPositionInFOV(), Quaternion.identity);
    }

    public void KilledMushroom()
    {
        currScore++;
        currMush = null;
        notificationTxt.text = "Score: " + currScore.ToString();
        if (dEnemy != null)
            dEnemy.Invoke();
#if !UNITY_EDITOR
        if(currScore>=10)
#else
        if (currScore >= 2)
#endif
        {
            EndGame(true);
            return;
        }

        Invoke("SpawnTarget", 2f);
    }

    public void ResetGame(bool gameFinished=true)
    {
        if (gReset != null)
            gReset.Invoke();

        if(gameFinished)
        {

            planeFinder.SetActive(true);
            EnableGameScene(true);
        }

        currScore = 0;

    }

    public void EndGame(bool win=false)
    {

        if(win)
        {
            DLog.Log("Won");
            notificationTxt.text = "You Won!";
        }
        else
        {
            DLog.Log("Lost");
        }

        resetCanvas.SetActive(true);
        gameStarted = false;

        //planeFinder.SetActive(true);

        //if (gEnded != null)
        //    gEnded.Invoke(true);
    }
}
