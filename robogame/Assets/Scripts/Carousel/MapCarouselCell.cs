using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FancyCarouselView.Runtime.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MapCarouselCell : CarouselCell<CarouselData, MapCarouselCell>
{
    [SerializeField] private Image _image = default;
    [SerializeField] private Text _text = default;

    [SerializeField] private Text id;


    protected override void Refresh(CarouselData data) {
        _image.sprite = data.background;
        _text.text = data.Text;
        id.text = data.id.ToString();
    }
}
