using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class CardDataScript : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{
    public CardAbs card;

    public Text nameText;

    public Text cardCost;

    public Image image;

    public ToolTipTrigger tipTrigger;
    
    private Vector3 _target;
    public Vector3 startPosition;
    private float   _timeToReachTarget = 0.1f;
    private float   _t;

    
    private void Awake() {
        
    }

    public void setUpCard(CardAbs cardInfo,Vector3 initPosition) {

        this.card = cardInfo;
        nameText.text = cardInfo.name;
        image = cardInfo.image;
        tipTrigger.headerText = cardInfo.name;
        tipTrigger.bodyText = cardInfo.bodyText;
        if (cardInfo.cost.cost.Length > 1)
        {
            Debug.Log("need to set up multiple cardInfo costs still");
        }
        else
        {
            cardCost.text = cardInfo.cost.cost[0].ToString();
            
        }
        startPosition = initPosition;
        _target = startPosition;
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        setDestination(new Vector3(transform.position.x, transform.position.y  + 20f, 0),0.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        setDestination(startPosition,0.2f);
    }

    void Update() 
    {
        _t += Time.deltaTime/_timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, _target, _t);
    }
    public void setDestination(Vector3 destination, float time)
    {
        _t = 0;
        _timeToReachTarget = time;
        _target = destination; 
    }
    
    
   
    
    
}
