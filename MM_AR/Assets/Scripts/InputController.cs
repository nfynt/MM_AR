using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMAR;

public class InputController : Singleton<InputController> {

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
    }

    public Vector3 GetRandomPositionInFOV()
    {
        //return mainCam.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 0f, Random.Range(0.1f, 0.9f)));
        Vector3 pos;
        Vector3 dir = mainCam.transform.forward;
        pos = new Vector3(Random.Range(dir.x + 0.5f, dir.x + 1f),
            0f,
            Random.Range(dir.z + 0.5f, dir.z + 1f));
        //return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        return pos;
    }
    
}
