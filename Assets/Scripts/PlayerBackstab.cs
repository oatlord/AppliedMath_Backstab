using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBackstab : MonoBehaviour
{
    [Header("References")]
    public Transform enemy;
    public TMP_Text promptText;

    [Header("Backstab Settings")]
    public float detectionRange = 2.5f;
    public float angleThreshold = 60f;
    public float messageResetTime = 2f;

    private bool canBackstab = false;
    private bool hasBackstabbed = false;
    private bool messageLocked = false;
    private PlayerControls playerControls;
    private EnemyPatrol enemyPatrol;
    private Coroutine messageRoutine;

    [Header("Optional References")]
    public Animator swordAnimator;

    void Start()
    {
        playerControls = GetComponent<PlayerControls>();
        if (enemy != null)
            enemyPatrol = enemy.GetComponent<EnemyPatrol>();

        if (promptText != null)
            promptText.text = "Get behind the enemy to backstab!";
    }

    void Update()
    {
        if (enemy == null) return;
        if (hasBackstabbed) return;

        Vector3 directionToPlayer = (transform.position - enemy.position).normalized;
        float angle = Vector3.Angle(enemy.forward, directionToPlayer);
        float distance = Vector3.Distance(transform.position, enemy.position);

        canBackstab = angle < angleThreshold && distance <= detectionRange;

        if (promptText != null && !messageLocked)
        {
            if (canBackstab)
                promptText.text = "Press V or Left Click to Backstab";
            else
                promptText.text = "Get behind the enemy to backstab!";
        }

        if ((Input.GetKey(KeyCode.V) || Input.GetMouseButtonDown(0)) && !hasBackstabbed)
        {
            if (canBackstab)
            {
                hasBackstabbed = true;
                Debug.Log("✅ Backstab successful!");
                ShowMessage("Backstab Successful!", true);
            }
            else
            {
                Debug.Log("❌ Not behind the enemy — cannot backstab!");
                ShowMessage("Attack Failed!", false);
            }
        }
    }

    void ShowMessage(string message, bool successful)
    {
        // stop any previous coroutine
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageLocked = true;

        // display one text only
        if (promptText != null)
            promptText.text = message;

        if (successful)
        {
            // freeze game movement and enemy patrol
            if (playerControls != null)
                playerControls.enabled = false;
            if (enemyPatrol != null)
                enemyPatrol.enabled = false;
            if (swordAnimator != null)
                swordAnimator.enabled = false;
        }
        else
        {
            // reset message after delay
            messageRoutine = StartCoroutine(ResetMessage());
        }
    }

    IEnumerator ResetMessage()
    {
        yield return new WaitForSeconds(messageResetTime);

        messageLocked = false;

        if (promptText != null)
            promptText.text = "Get behind the enemy to backstab!";
    }
}
