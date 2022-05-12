using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] public string headerText;
	[SerializeField] public string bodyText;
	private float waitTimer = 0.5f;
	private bool showToolTipbool;
    public async void OnPointerEnter(PointerEventData eventData) {
	    
	   StopAllCoroutines();
	   StartCoroutine(callManager(headerText, bodyText));

    }

    public void OnPointerExit(PointerEventData eventData) {
	    StopAllCoroutines();
	    ToolTipManager.hideToolTip();
       
    }

    private IEnumerator callManager(string headerText, string bodyText) {
	    yield return new WaitForSeconds(waitTimer);
	    Debug.Log("showing tooltip");
	    ToolTipManager.showToolTip(headerText,bodyText);
    }
}
