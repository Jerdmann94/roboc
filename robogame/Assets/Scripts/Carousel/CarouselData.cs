using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselData {
    public int id { get; }
    public Sprite background { get; }
    public string Text { get; }

    public CarouselData(Sprite sbackground, string text, int identity) {
        background = sbackground;
        Text = text;
        id = identity;
    }
}
