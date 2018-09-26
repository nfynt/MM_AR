using MMAR;
using UnityEngine;
using System.Collections;

using DLog = MMAR.LogManager;

[RequireComponent(typeof(Animator))]
public class MushroomController : MonoBehaviour {

    public delegate void EnemyDied();
    public event EnemyDied died;

    public float finalScale = 5f;

    private Animator animator;
    private bool isDead = false;
    private float startTime;
    private Vector3 growthRate;
    private float timeLeft = 25f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
        growthRate = Vector3.one * (Time.deltaTime/(finalScale-transform.localScale.x));
    }

    private void Update()
    {
        if (isDead)
            return;

        if (Time.time - startTime < timeLeft)
            transform.localScale += growthRate;

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
    }

    IEnumerator PlayDeadAnim()
    {
        animator.SetBool("Die", true);
        if (died != null)
            died.Invoke();
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
    }
}
