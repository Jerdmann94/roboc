using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.String;

[ExecuteInEditMode()]
public class ToolTip : MonoBehaviour {

    public InputSystemUIInputModule inputModule;
    public TextMeshProUGUI header;

    public TextMeshProUGUI body;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void showText(string headerText, string bodyText) {
        int headerLength = header.text.Length;
        int bodyLength = body.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || bodyLength > characterWrapLimit);
        
        
        if (IsNullOrEmpty(headerText)) {
            header.gameObject.SetActive(false);
        }
        else {
            header.gameObject.SetActive(true);
            header.text = headerText;
        }

        body.text = bodyText;
    }
    
    void Update() {
        if (Application.isEditor) {
            int headerLength = header.text.Length;
            int bodyLength = body.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || bodyLength > characterWrapLimit);
        }
         
        
        Vector2 position = inputModule.point.action.ReadValue<Vector2>();

        var pivx = position.x / Screen.width;
        var pivy = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivx, pivy);
        transform.position = position;
    }
}
