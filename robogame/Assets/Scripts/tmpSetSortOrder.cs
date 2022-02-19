using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tmpSetSortOrder : MonoBehaviour
{
    public TextMeshPro tmp;
    public float fadeDuration;
    public Color maximumOpacityColor, minimumOpacityColor;
    public AnimationCurve animationCurve;
    private void Start()
    {
        StartCoroutine(InterpolateColor(maximumOpacityColor, minimumOpacityColor, fadeDuration));    
        tmp.sortingOrder = 2;
    }
     
    private IEnumerator InterpolateColor(Color minColor, Color maxColor, float _time) {
        float currTime = 0f;
        while (currTime < _time) {
            currTime += Time.deltaTime;
            float lerpVal = Mathf.InverseLerp(0f, _time, currTime);
            tmp.color = Color.Lerp(minColor, maxColor, animationCurve.Evaluate(lerpVal));
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
    

}
