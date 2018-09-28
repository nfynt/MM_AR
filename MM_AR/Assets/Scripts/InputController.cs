using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMAR;

public class InputController : Singleton<InputController> {

    public WhackGameController gameCtrl;

    private Camera mainCam;

    #region getter setter

    public Camera MainCamera
    {
        get
        {
            return mainCam;
        }
        set
        {
            mainCam = value;
        }
    }

    #endregion

    private void Awake()
    {
        mainCam = Camera.main;
        gameCtrl = WhackGameController.Instance;
    }

    public Vector3 GetRandomPositionInFOV()
    {
        Vector3 size = gameCtrl.transform.GetComponent<BoxCollider>().bounds.size;

        return gameCtrl.transform.position + new Vector3((Random.value - 0.5f) * size.x,
           (Random.value - 0.5f) * size.y,
           (Random.value - 0.5f) * size.z);
        
    }
    
}
