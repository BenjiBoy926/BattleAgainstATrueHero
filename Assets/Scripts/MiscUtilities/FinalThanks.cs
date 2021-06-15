using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalThanks : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Delay when the scene loads before it starts revealing text")]
    private float startDelay;
    [SerializeField]
    [Tooltip("Time between each text reveal")]
    private float revealTimes;
    [SerializeField]
    [Tooltip("List of text objects to reveal")]
    private List<GameObject> textObjects;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(revealTimes);

        yield return new WaitForSeconds(startDelay);
        for(int i = 0; i < textObjects.Count; i++)
        {
            if(i > 0) yield return wait;
            textObjects[i].SetActive(true);
        }
    }
}
