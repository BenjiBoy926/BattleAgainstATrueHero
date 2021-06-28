using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ExpandOverTime : MonoBehaviour
{
    // TYPEDEFS
    public enum FinishBehavior
    {
        Nothing, Disable, Destroy
    }

    [SerializeField]
    [Tooltip("Starting size of the object")]
    private Vector3 startingSize = Vector3.zero;
    [SerializeField]
    [Tooltip("Ending size of the object")]
    private Vector3 endingSize = Vector3.one * 2f;
    [SerializeField]
    [Tooltip("Time it takes to finish the effect")]
    private float effectTime = 0.5f;
    [SerializeField]
    [Tooltip("If true, the effect plays when the object is activated")]
    private bool playOnAwake;
    [SerializeField]
    [Tooltip("If true, destroy this object when the effect finishes")]
    private FinishBehavior finishBehavior;

    private void Start()
    {
        if (playOnAwake) Play();
    }

    public void Play()
    {
        gameObject.SetActive(true);
        transform.localScale = startingSize;
        transform.DOScale(endingSize, effectTime).OnComplete(Finish);
    }

    private void Finish()
    {
        if (finishBehavior == FinishBehavior.Destroy) Destroy(gameObject);
        else if (finishBehavior == FinishBehavior.Disable) gameObject.SetActive(false);
    }
}
