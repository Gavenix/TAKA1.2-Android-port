using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoomguyFace : MonoBehaviour
{
    public Image faceImage; // UI-иконка персонажа
    public Sprite normalFace; // Обычное лицо
    public Sprite hurtFace; // Лицо при уроне

    public void ShowHurtFace()
    {
        StopAllCoroutines(); // Останавливаем другие вызовы, если есть
        StartCoroutine(ChangeFaceTemporarily());
    }

    IEnumerator ChangeFaceTemporarily()
    {
        faceImage.sprite = hurtFace; // Меняем на больное
        yield return new WaitForSeconds(1f); // Ждём 1 секунду
        faceImage.sprite = normalFace; // Возвращаем обратно
    }
}
