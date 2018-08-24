using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
	public int levelIndex;
	public CapturedOctoData[] CapturedOctos;
	public KeyData[] keys;
	public int amounToDefeat;
}
[Serializable]
public class CapturedOctoData
{
	public float AnchorX;
	public float AnchorY;
    public float PosX;
    public float PosY;
	public int KeysNeeded;

	public Vector2 Anchor
	{
		get
		{
			return new Vector2(AnchorX, AnchorY);
		}
	}

	public Vector2 Position
	{
		get
		{
			return new Vector2(PosX, PosY);
		}
	}
}

public class KeyData
{
	public float AnchorX;
	public float AnchorY;
	public float PosX;
	public float PosY;

	public Vector2 Anchor
	{
		get
		{
			return new Vector2(AnchorX, AnchorY);
		}
	}

	public Vector2 Position
	{
		get
		{
			return new Vector2(PosX, PosY);
		}
	}
}

public class LevelContainer
{
	public List<LevelData> levels;
}