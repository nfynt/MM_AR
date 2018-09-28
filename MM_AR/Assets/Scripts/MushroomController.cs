using MMAR;
using UnityEngine;
using System.Collections;

using DLog = MMAR.LogManager;

[RequireComponent(typeof(Animator))]
public class MushroomController : MonoBehaviour {

    public WhackGameController gCtrl;

    public float finalScale = 1f;

    public bool kill = false;

    private Animator animator;
    private bool isDead = false;
    private float startTime;
    private Vector3 growthRate;
    private float timeLeft = 25f;

    public bool IsAlive
    {
        get
        {
            return !isDead;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
        growthRate = Vector3.one * (finalScale-transform.localScale.x) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        if (Time.time - startTime < timeLeft)
            transform.localScale += growthRate*Time.deltaTime;

        if(kill)
        {
            kill = true;
            isDead = true;
            StartCoroutine(PlayDeadAnim());
            gCtrl.KilledMushroom();
            Debug.Log("Killed");
        }

    }

    public void TogglePauseState(bool pause = false)
    {
        if(pause)
        {
            timeLeft -= Time.time - startTime;
        }
        else
        {
            startTime = Time.time;
        }
    }

    private void OnMouseDown()
    {
        if (isDead)
            return;
        DLog.Log("Clicked");
        isDead = true;
        StartCoroutine(PlayDeadAnim());
        gCtrl.KilledMushroom();
        Debug.Log("Killed");
    }

    IEnumerator PlayDeadAnim()
    {
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
    }
}
