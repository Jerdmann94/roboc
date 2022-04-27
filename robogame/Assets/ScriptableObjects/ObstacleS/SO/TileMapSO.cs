using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapSo : ScriptableObject
{
	public new String      name;
	public     int         health;

	public LabelBase[] labelBases;
	//public     int         attack;
	public     Sprite      shape;
}
