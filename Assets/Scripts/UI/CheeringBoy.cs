using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CheeringBoy : MonoBehaviour
{
    [SerializeField]
    private CheerPackage cheerPackage;

    [Header("Default Preferences Values")]
    [SerializeField, Tooltip("Mean time between each cheering")]
    private float cheerBoyQuietness = 4f;
    [SerializeField, Tooltip("Variance of the time between each cheering")]
    private float cheerBoyQuietnessVariance = 1f;
    [Space]
    [SerializeField, Tooltip("Text duration")]
    private float cheerBoyTextPersistence = 0.5f;
    [SerializeField, Tooltip("Text fade duration (added on top of the persistence)")]
    private float cheerBoyTextPersistenceFade = 0.5f;
    [SerializeField, Tooltip("Text size")]
    private float cheerBoyTextSize = 50f;

    [Header("References")]
    [SerializeField]
    private Image levelSlider;
    [SerializeField]
    private NotificationAlert notificationAlert;
    [SerializeField]
    private ObjectShake shakeAnimation;

    private bool cheering = false;

    public void StartCheering()
    {
        StartCoroutine(Cheer());
    }

    public void StopCheering()
    {
        StopAllCoroutines();
        cheering = false;
    }

    public void ToogleCheeringBoy(bool use)
    {
        PlayerPrefs.SetInt("CheeringBoy", use ? 1 : 0);
    }

    private IEnumerator Cheer()
    {
        if (cheering) yield break;

        while (true)
        {
            yield return new WaitForSeconds(cheerBoyQuietness + Random.Range(-cheerBoyQuietnessVariance, cheerBoyQuietnessVariance));

            MakeCheerNotification();
        }
    }

    private void MakeCheerNotification()
    {
        CheerSentence sentence = cheerPackage.GetRandomCheerSentence(levelSlider.fillAmount);

        notificationAlert.PlaceRandomly();
        notificationAlert.NewNotification(sentence.Sentence, sentence.Sprite, cheerBoyTextPersistence, cheerBoyTextPersistenceFade);
        notificationAlert.SetTextSize(cheerBoyTextSize);
        shakeAnimation.StartShake(sentence.ShakeFactor);
    }
}
