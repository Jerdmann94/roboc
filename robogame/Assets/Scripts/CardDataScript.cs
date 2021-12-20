using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class CardDataScript : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{
    public CardSO card;

    public Text nameText;

    public Text cardCost;

    public Text cardDescription;

    public Text cardFlavor;

    private Vector3 target;
    public Vector3 startPosition;
    private float   timeToReachTarget = 0.1f;
    private float   t;

    
    private void Awake() {
        
    }

    public void setUpCard(CardSO card,Vector3 initPosition) {
        this.card = card;
        nameText.text = card.name;
        if (card.cost.cost.Length > 1)
        {
            Debug.Log("need to set up multiple card costs still");
        }
        else
        {
            cardCost.text = card.cost.cost[0].ToString();
        }
        
        cardDescription.text = card.cardDescription;
        cardFlavor.text = card.cardFlavor;
        startPosition = initPosition;
        target = startPosition;
        
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
        t += Time.deltaTime/timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
    }
    public void setDestination(Vector3 destination, float time)
    {
        t = 0;
        timeToReachTarget = time;
        target = destination; 
    }
    
    
   
    
    
}
