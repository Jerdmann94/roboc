using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPSetSortOrder : MonoBehaviour
{
    public TextMeshPro tmp;
    public float fadeDuration;
    public Color maximumOpacityColor, minimumOpacityColor;
    public AnimationCurve animationCurve;
    private void Start()
    {
        StartCoroutine(interpolateColor(maximumOpacityColor, minimumOpacityColor, fadeDuration));    
        tmp.sortingOrder = 2;
    }
     
    private IEnumerator interpolateColor(Color minColor, Color maxColor, float time) {
        float currTime = 0f;
        while (currTime < time) {
            currTime += Time.deltaTime;
            float lerpVal = Mathf.InverseLerp(0f, time, currTime);
            tmp.color = Color.Lerp(minColor, maxColor, animationCurve.Evaluate(lerpVal));
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
    

}
