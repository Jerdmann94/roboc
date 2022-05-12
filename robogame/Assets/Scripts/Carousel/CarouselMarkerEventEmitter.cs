using System.Collections;
using System.Collections.Generic;
using FancyCarouselView.Runtime.Scripts;
using UnityEngine;

public class CarouselMarkerEventEmitter : MonoBehaviour
{
	[SerializeField] private IntEvent emitter;
	[SerializeField] private DotCarouselProgressElement element;
	public void buttonPress() {
		emitter.emit(element.orderPosition);
	}
}
