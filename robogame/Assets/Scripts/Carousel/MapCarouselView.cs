using FancyCarouselView.Runtime.Scripts;

public class MapCarouselView : CarouselView<CarouselData, MapCarouselCell>
{
	public void doStuff() {
		base._scroller.JumpTo(1);
	}
}
