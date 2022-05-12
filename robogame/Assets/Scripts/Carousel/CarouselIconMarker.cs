using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;

public class CarouselIconMarker : MonoBehaviour {
    public GoRunTimeSet carouselMarkers;

    public void onCarouselChange(int i) {
        //Debug.Log(i + " moving icon");
        this.gameObject.transform.position = carouselMarkers.items[i].transform.position;
    }
}
