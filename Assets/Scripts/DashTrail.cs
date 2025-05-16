using UnityEngine;

public class DashTrail : MonoBehaviour
{
        private TrailRenderer trail;
        private bool isDashing;

        void Start()
        {
            trail = GetComponent<TrailRenderer>();
            trail.enabled = false; // ��������� ����� � ������
        }

        public void StartDash()
        {
            isDashing = true;
            trail.enabled = true;
        }

        public void EndDash()
        {
            isDashing = false;
            Invoke("DisableTrail", 0.1f); // ������� ��������� ����� �����������
        }

        private void DisableTrail()
        {
            if (!isDashing)
                trail.enabled = false;
        }
    }