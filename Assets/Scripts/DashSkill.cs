using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DashSkill : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 5f;
    public float dashDistance = 2f;
    public float dashCooldown = 1f;

    [Header("Mana Settings")]
    public int dashManaCost = 20;

    [Header("Collision Settings")]
    public LayerMask obstacleLayers;

    private PlayerController playerController;
    private HealthManager healthManager;
    private TrailRenderer trailRenderer;
    private bool isDashing = false;
    private bool canDash = true;

    public event Action DashStart;
    public event Action DashEnd;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        healthManager = GetComponent<HealthManager>();
        trailRenderer = transform.Find("Trail").GetComponent<TrailRenderer>();// Получаем ссылку на трейл
        if (trailRenderer != null) trailRenderer.enabled = false; // Отключаем в начале
    }

    private void Update()
    {
        if (isDashing || !canDash) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && playerController.GetCurrentMana() >= dashManaCost)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        if (healthManager != null) healthManager.SetInvulnerable(true);

        playerController.UpdateMana(-dashManaCost);
        DashStart?.Invoke();

        if (trailRenderer != null) trailRenderer.enabled = true; // Включаем трейл

        Vector2 dashDirection = playerController.GetMovementDirection().normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, dashDistance, obstacleLayers);
        float actualDashDistance = hit.collider != null ? hit.distance : dashDistance;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + dashDirection * actualDashDistance;

        float elapsedTime = 0f;
        float dashDuration = actualDashDistance / dashSpeed;

        while (elapsedTime < dashDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        DashEnd?.Invoke();
        isDashing = false;

        if (healthManager != null) healthManager.SetInvulnerable(false);

        if (trailRenderer != null) Invoke("DisableTrail", 0.5f); // Немного подождать перед отключением

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void TryDash()
    {
        if (isDashing || !canDash) return;
        if (playerController.GetCurrentMana() < dashManaCost) return;
        StartCoroutine(Dash());
    }

    private void DisableTrail()
    {
        if (!isDashing && trailRenderer != null)
        {
            trailRenderer.enabled = false; // Выключаем трейл
        }
    }

    private IEnumerator FadeOutTrail()
    {
        float startTime = trailRenderer.time; // Запоминаем исходное время жизни трейла
        float elapsed = 0f;
        float fadeDuration = 0.5f; // Время затухания

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            trailRenderer.time = Mathf.Lerp(startTime, 0, elapsed / fadeDuration);
            yield return null;
        }

        trailRenderer.time = startTime; // Возвращаем время в норму
    }

    private void OnDrawGizmos()
    {
        if (playerController == null) return;
        Vector2 dashDirection = playerController.GetMovementDirection().normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, dashDirection * dashDistance);
    }
}