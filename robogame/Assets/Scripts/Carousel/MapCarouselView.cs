using FancyCarouselView.Runtime.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MapCarouselView : CarouselView<CarouselData, MapCarouselCell> {
	
	
	public void moveLeft(IntEvent emitter, SceneChoiceSo sceneChoiceSo, CarouselDataSo[] carouselMasterCData, GameObject[] imageArray) {
		var scroller = base._scroller;
		var changer = ActiveCellIndex;
		if (changer == 0 ) {
			changer = DataCount;
		}
		if (scroller != null) scroller.JumpTo(changer - 1);
		emitter.emit(changer -1);
		sceneChoiceSo.choiceType = carouselMasterCData[changer-1].choiceType;
		changeIcons(carouselMasterCData,imageArray, changer-1);
	}
	public void moveRight(IntEvent emitter, SceneChoiceSo sceneChoiceSo, CarouselDataSo[] carouselMasterCData,GameObject[] imageArray) {
		var scroller = base._scroller;
		var changer = ActiveCellIndex;
		if (changer >= DataCount - 1 ) {
			changer = -1;
		}
		if (scroller != null) scroller.JumpTo(changer + 1);
		emitter.emit(changer+1);
		sceneChoiceSo.choiceType = carouselMasterCData[changer + 1].choiceType;
		changeIcons(carouselMasterCData,imageArray, changer+1);
	}

	public void moveTo(int i) {
		//Debug.Log(i);
		var scroller = base._scroller;
		if (scroller != null) scroller.JumpTo(i);
	}

	public void changeIcons(CarouselDataSo[] carouselMaster,GameObject[] imageArray,int i) {
		for (int j = 0; j < carouselMaster[i].icons.Length; j++) {
			imageArray[j].SetActive(true);
			imageArray[j].GetComponent<Image>().sprite = carouselMaster[i].icons[j];
			
		}

		for (int n = carouselMaster[i].icons.Length - 1; n <= imageArray.Length-1; n++) {
			
			imageArray[n].SetActive(false);
		}	
	}

	

	

}
