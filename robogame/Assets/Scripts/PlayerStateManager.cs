using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScriptableObjects.Sets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PlayerStateManager : MonoBehaviour
{
    public TurnState playerState;
    public CombatManager combatManager;
    public MouseHandler mouseHandler;
    public GameObject    player;

    private PlayerScript playerScript;
    //PLAYER DATA
    public DeckSO     deckData;
    public GameObject cardUI;
    public GameObject canvas;
    public GameObject handPoint1;
    public GameObject handPoint2;
    
    private int        handSize = 5;
    static  Random     _random  = new Random();
    [SerializeField] private BoolValue playPhase;

    [SerializeField] private GORunTimeSet handUIArray;
    public CardSet handSet;
    public CardSet deckSet;
    public CardSet discardSet;
    
    public CardUIValue[] energyUIValues;
    
    //----------UNITY METHODS --------------//
    void Awake() {
        playerScript = player.GetComponent<PlayerScript>();
        deckSet.items = new List<CardAbs>();
        handSet.items = new List<CardAbs>();
        discardSet.items = new List<CardAbs>();
        handUIArray.items = new List<GameObject>();
        foreach (var cardSo in deckData.deck) {
            deckSet.add( cardSo);
        }
        deckSet.items = Shuffle<CardAbs>(deckSet.items);
        playPhase.Value = false;
        
        drawHand();
    }

   
    //-------PLAYER STATE METHODS ---------------//
    
    public void startState() {
        
        playerState.startState();
    }
    
    public void playerOnStateChange() {
       // Debug.Log("player state changing to " + playerState.CurrentRound.name);
        switch (playerState.CurrentRound.name) {
            case"GetEnergy":
                refillEnergy();
                mouseHandler.cc.checkWhichCardsCanBePlayed();
                playerState.nextState();
                break;
            case"Draw":
                drawCard();
                playerState.nextState();
                break;
            case"Play":
                startPlayPhaseOfTurn();
                break;
            case"Discard":
                reduceHandToMaxHandSize();
                break;
            case"EndTurn":
                mouseHandler.cc.checkWhichCardsCanBePlayed();
                combatManager.combatState.nextState();
                break;
            default:
                
                break;
        }
    }

    private void reduceHandToMaxHandSize() {
        if (handUIArray.items.Count < 5) {
            playerState.nextState();
            return;
        }

        for (int i = handUIArray.items.Count - 1; i >= 5; i--) {
            discardRandomCardFromHand();
        }
        resetHandPosition();
        playerState.nextState();

    }

    private void discardRandomCardFromHand() {
        int r = _random.Next(handUIArray.items.Count);
        Destroy(handUIArray.items[r]);
        handUIArray.items.RemoveAt(r);
        discardSet.items.Add(handSet.items[r]);
        handSet.items.RemoveAt(r);
    }

    private void startPlayPhaseOfTurn() {
        playPhase.Value = true;
    }


    //PLAYER METHODS - THEY NEED TO BE REMOVED FROM PLAYER OBJECT
    private void refillEnergy() {
        foreach (var card in energyUIValues)
        {
            switch (card.name) {
                case "MagicEnergy" :
                    card.Value = playerScript.stats.magicEnergy;
                    break;
                case "MoveEnergy" :
                    card.Value = playerScript.stats.moveEnergy;
                    break;
                case "PhysicalEnergy" :
                    card.Value = playerScript.stats.physicalEnergy;
                    break;
            }
        }
    }

    private void drawCard() {
        if (deckSet.items.Count == 0) {
            if (discardSet.items.Count > 0) {
                deckSet.items = Shuffle<CardAbs>(discardSet.items);
                discardSet.items = new List<CardAbs>();
            }
            else {
                return;
            }
        }
        
        createCardUI(handSet.items.Count);
        resetHandPosition();
    }
    public void drawHand() {
        for (int i = 0; i < handSize; i++) {
            createCardUI(i);
        }
        //resetHandPosition();
    }

    public void createCardUI(int i) {
        handSet.add((CardAbs) deckSet.items[0]);
        //hand.Add(deck[0]);

        deckSet.remove((CardAbs) deckSet.items[0]);
        //deck.RemoveAt(0);

        
        float xvalue = (handPoint1.transform.position.x - handPoint2.transform.position.x) / handSize;
        Vector3 temp = new Vector3(handPoint1.transform.position.x + Mathf.Abs((xvalue * i)) + Mathf.Abs(xvalue / 2),
            handPoint1.transform.position.y, 0);
        GameObject card = Instantiate(cardUI,temp, quaternion.identity, canvas.transform);
        card.GetComponent<CardDataScript>().setUpCard( handSet.items[i], card.transform.position);
        //Debug.Log(i);
        //card.transform.SetParent(canvas.transform);
        card.transform.localScale = Vector3.one;
        card.GetComponent<Button>().onClick.AddListener(() =>
            mouseHandler
                .playCard(card.GetComponent<CardDataScript>()
                    .card));
        handUIArray.items.Add(card);
        
    }

    public void resetHandPosition() {
        for (int j = 0; j < handUIArray.items.Count; j++) {
            float xvalue = (handPoint1.transform.position.x - handPoint2.transform.position.x) / handUIArray.items.Count;
            Vector3 temp = new Vector3(handPoint1.transform.position.x + Mathf.Abs((xvalue * j)) + Mathf.Abs(xvalue / 2),
                handPoint1.transform.position.y, 0);
            handUIArray.items[j].GetComponent<RectTransform>().anchoredPosition = (temp);
            handUIArray.items[j].GetComponent<CardDataScript>().startPosition = temp;
            handUIArray.items[j].GetComponent<CardDataScript>().setDestination(temp,0.1f);
            handSet.items[j].handPosition = j;

        }
        
        
    }


    static List<CardAbs> Shuffle<T>(List<CardAbs> array) {
        int n = array.Count;
        for (int i = 0; i < (n - 1); i++) {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + _random.Next(n - i);
            var t = array[r];
            array[r] = array[i];
            array[i] = t;
        }

        return array;
    }

    public void initializePlayerState() {
        player = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
