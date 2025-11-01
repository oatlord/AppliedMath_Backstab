using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float waitTime = 2f;

    private bool isWaiting = false;

    void Start()
    {
        targetPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.1f)
            {
                StartCoroutine(PauseBeforeMoving());
            }
        }
    }

    IEnumerator PauseBeforeMoving()
    {
        if (isWaiting) yield break;
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);
        increaseTargetInt();
        isWaiting = false;
    }

    void increaseTargetInt()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }
}
