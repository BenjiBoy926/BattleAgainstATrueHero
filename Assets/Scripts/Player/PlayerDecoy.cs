using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using DG.Tweening;

public class PlayerDecoy : MonoBehaviour
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the player object this will act as the decoy for")]
    private string playerTag;
    [SerializeField]
    [Tooltip("Rect transform of the decoy")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Main graphic for the decoy")]
    private Image image;
    [SerializeField]
    [Tooltip("Image that overlays all other objects")]
    private Image overlay;
    [SerializeField]
    [Tooltip("Audio source that plays the audio")]
    private AudioSource audio;

    [Header("Death Effects")]

    [SerializeField]
    [Tooltip("Time between the crack and the splinter of the heart shape when the player dies")]
    private float splitDelay;
    [SerializeField]
    [Tooltip("Time after the player splinters away that the game over text appears")]
    private float gameOverDelay;
    [SerializeField]
    [Tooltip("Default appearance of the player")]
    private Sprite defaultSprite;
    [SerializeField]
    [Tooltip("Appearance of the player when the heart cracks")]
    private Sprite crackSprite;
    [SerializeField]
    [Tooltip("Object that represents the heart splinter")]
    private GameObject splinterObject;
    [SerializeField]
    [Tooltip("The number of splinters instantiated when the player dies")]
    private int splinterCount;
    [SerializeField]
    [Tooltip("Clip played when the heart cracks")]
    private AudioClip crackClip;
    [SerializeField]
    [Tooltip("Clip played when the heart splinters to pieces")]
    private AudioClip splinterClip;

    [Header("Unbreakable Effects")]

    [SerializeField]
    [Tooltip("Object that appears to signal that unbreakable mode is going to trigger")]
    private GameObject charaHeadParent;
    [SerializeField]
    [Tooltip("Text that displays unbreakable mode triggers during the trigger effect")]
    private TextMeshProUGUI unbreakableTriggerText;
    [SerializeField]
    [Tooltip("Delay before the heart starts shaking")]
    private float shakeDelay = 0.5f;
    [SerializeField]
    [Tooltip("Time for which the heart shakes before being put back together")]
    private float shakeDuration = 1f;
    [SerializeField]
    [Tooltip("Time it takes for the text size to punch in and out of it's larger position")]
    private float textPunchScaleTime = 0.25f;
    [SerializeField]
    [Tooltip("Time it takes after the unbreakable effect finishes to resume the music")]
    private float resumeDelay = 1f;

    // Transform of the player this is a decoy for
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;

        // Set the size of the decoy to have the same size as the player collider
        Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();
        Vector2 min = Camera.main.WorldToScreenPoint(playerCollider.bounds.min);
        Vector2 max = Camera.main.WorldToScreenPoint(playerCollider.bounds.max);
        rectTransform.sizeDelta = max - min;
    }
    // So that the player does not have to schedule the routine themselves
    public void DeathEffect()
    {
        // Enable the decoy
        SetActive(true);
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator SplitRoutine()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(splitDelay);

        // Wait the defined time before heart splits
        yield return wait;

        // After a second, make the heart crack
        image.sprite = crackSprite;
        // Play the heart crack clip
        audio.clip = crackClip;
        audio.Play();

        // Wait again before next thing happens
        yield return wait;
    }

    public IEnumerator DeathRoutine()
    {
        // Split the heart
        yield return SplitRoutine();

        // Disable the renderer so that it is not seen with the splinters
        image.enabled = false;
        // Play the heart splinter clip
        audio.clip = splinterClip;
        audio.Play();

        // Instantiate multiple splinters. The objects themselves take care of other things
        // like initial velocity and rotation
        for (int i = 0; i < splinterCount; i++)
        {
            GameObject instance = Instantiate(splinterObject, transform);
            instance.transform.position = transform.position;
        }
        // Wait for a delay until beginning the gameover
        yield return new WaitForSecondsRealtime(gameOverDelay);

        // Startup game over manager
        GameOver.BeginGameOver("BattleAgainstATrueHero");
    }

    public IEnumerator UnbreakableTriggerRoutine()
    {
        // Enable the decoy
        SetActive(true);
        // Split the heart
        yield return SplitRoutine();

        // Enable chara's head
        charaHeadParent.SetActive(true);
        // Display one less than the trigger counter to make increase by 1 dramatic
        unbreakableTriggerText.text = (PlayerHealth.unbreakableTriggerCounter - 1).ToString();
        // Wait before shaking the heart
        yield return new WaitForSecondsRealtime(shakeDelay);
        // Shake the heart around
        transform.DOShakePosition(shakeDuration, 10f, 30, fadeOut: false).SetUpdate(true);
        // Wait for the shake to finish
        yield return new WaitForSecondsRealtime(shakeDuration);

        // Put the heart back together
        audio.clip = crackClip;
        audio.Play();
        image.sprite = defaultSprite;

        // Set the number on the text, then punch the scale bigger
        unbreakableTriggerText.text = PlayerHealth.unbreakableTriggerCounter.ToString();
        unbreakableTriggerText.transform.DOPunchScale(unbreakableTriggerText.transform.localScale * 2f, textPunchScaleTime).SetUpdate(true);

        // Wait for text scaling to finish
        yield return new WaitForSecondsRealtime(textPunchScaleTime);
        // Wait a sec before resuming the game
        yield return new WaitForSecondsRealtime(resumeDelay);

        // Disable the chara effect
        charaHeadParent.SetActive(false);
        // Disable the decoy
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        overlay.enabled = active;
        overlay.color = Color.black;
        gameObject.SetActive(active);

        if(active)
        {
            // Go to player's position
            Vector3 position = Camera.main.WorldToScreenPoint(playerTransform.position);
            rectTransform.position = position;
        }
    }
}
