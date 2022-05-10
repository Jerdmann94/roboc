using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolTipManager : MonoBehaviour {
    public static ToolTipManager instance;
    public ToolTip toolTip;
    private void Awake() {
        instance = this;
        hideToolTip();
    }

    public static void showToolTip(string headerText, string bodyText) {
        instance.toolTip.showText(headerText,bodyText);
        instance.toolTip.gameObject.SetActive(true);
    }

    public static void hideToolTip() {
        instance.toolTip.gameObject.SetActive(false);
    }
    
}
