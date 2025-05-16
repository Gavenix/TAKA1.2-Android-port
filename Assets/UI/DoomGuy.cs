using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoomguyFace : MonoBehaviour
{
    public Image faceImage; // UI-������ ���������
    public Sprite normalFace; // ������� ����
    public Sprite hurtFace; // ���� ��� �����

    public void ShowHurtFace()
    {
        StopAllCoroutines(); // ������������� ������ ������, ���� ����
        StartCoroutine(ChangeFaceTemporarily());
    }

    IEnumerator ChangeFaceTemporarily()
    {
        faceImage.sprite = hurtFace; // ������ �� �������
        yield return new WaitForSeconds(1f); // ��� 1 �������
        faceImage.sprite = normalFace; // ���������� �������
    }
}
