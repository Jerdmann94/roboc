using System.Linq;
using UnityEngine;



	public class CarouselMaster : MonoBehaviour
	{
		[SerializeField] private MapCarouselView _carouselView = default;
		[SerializeField, Range(1, 8)] private int _bannerCount = 3;
		[SerializeField] private CarouselDataSo[] cData;

		private void Start()
		{
			// var items = Enumerable.Range(0, _bannerCount)
			// 	.Select(i =>
			// 	{
			// 		var spriteResourceKey = $"tex_demo_banner_{i:D2}";
			// 		var text = $"Demo Banner {i:D2}";
			// 		return new CarouselData(spriteResourceKey, text);
			// 	})
			// 	.ToArray();
			int counter = 0;
			var items = cData.Select(i => {
				var spriteResourceKey = i.background;
				var text = i.name;
				int id = counter++;
				return new CarouselData(spriteResourceKey, text,counter);
			}).ToArray();
			_carouselView.Setup(items);
		}
	}
