using UnityEngine;



[CreateAssetMenu(fileName ="new Enemy", menuName = "Obstacles/Basic")]
public class ObstacleSo : TileMapSo {
	public bool killable;
	public bool pushable;
	public int damageWhenPushed;
	public AbsDeathEffect deathEffect;

}