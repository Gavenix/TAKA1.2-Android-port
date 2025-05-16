using UnityEngine;

public class DashTrail : MonoBehaviour
{
        private TrailRenderer trail;
        private bool isDashing;

        void Start()
        {
            trail = GetComponent<TrailRenderer>();
            trail.enabled = false; // Выключаем трейл в начале
        }

        public void StartDash()
        {
            isDashing = true;
            trail.enabled = true;
        }

        public void EndDash()
        {
            isDashing = false;
            Invoke("DisableTrail", 0.1f); // Немного подождать перед отключением
        }

        private void DisableTrail()
        {
            if (!isDashing)
                trail.enabled = false;
        }
    }