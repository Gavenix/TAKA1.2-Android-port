using UnityEngine;

public class TargetHighlighter : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private Transform indicatorAnchor;

    private void Start()
    {
        if (indicator != null && indicatorAnchor != null)
        {
            indicator.transform.position = indicatorAnchor.position;
        }

        if (indicator != null)
        {
            indicator.SetActive(false); // �� ���������� �� ���������
        }
    }

    public void SetHighlighted(bool active)
    {
        if (indicator != null)
        {
            indicator.SetActive(active);
            Debug.Log("Highlight " + (active ? "ON" : "OFF"));
        }
    }
}