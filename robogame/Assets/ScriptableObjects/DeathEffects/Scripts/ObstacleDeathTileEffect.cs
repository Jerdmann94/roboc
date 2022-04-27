using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;



[CreateAssetMenu(fileName = "new Card", menuName = "DeathEffects/Spawn Tile Effect At Death Location")]

public class ObstacleDeathTileEffect : AbsDeathEffect {
	[SerializeField] private TileEffectSo tileEffectSo;
	[SerializeField] private GameObject tileEffectBase;
	internal override async Task execute(Vector3 pos) {
		var temp = Instantiate(tileEffectBase, pos, quaternion.identity);
		temp.GetComponent<TileEffectHandler>().setupData(tileEffectSo);
		await Task.Yield();
	}
}