using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapCarouselMasterScript : MonoBehaviour
{
	[SerializeField] private IntEvent emitter;
	[SerializeField] private MapCarouselView mapCarouselView;
	public SceneChoiceSo sceneChoiceSo;
	public CarouselMaster carouselMaster;
	public GameObject[] imageArray;
	public void masterMoveLeft() {
		mapCarouselView.moveLeft(emitter,sceneChoiceSo,carouselMaster.cData,imageArray);
	}

	public void masterMoveRight() {
		mapCarouselView.moveRight(emitter,sceneChoiceSo,carouselMaster.cData, imageArray);
	}

	public void masterMoveTo(int i) {
		//emitter.emit(i); THIS IS ALREADY BEING DONE ELSEWHERE
		sceneChoiceSo.choiceType = carouselMaster.cData[i].choiceType;
		
		mapCarouselView.moveTo(i);
		for (int j = 0; j < carouselMaster.cData[i].icons.Length; j++) {
			imageArray[j].SetActive(true);
			imageArray[j].GetComponent<Image>().sprite = carouselMaster.cData[i].icons[j];
			
		}

		for (int n = carouselMaster.cData[i].icons.Length; n <= imageArray.Length-1; n++) {
			imageArray[n].SetActive(false);
			
		}	
		
	}
	public void onStart(){
		masterMoveTo(1);
	}
}
