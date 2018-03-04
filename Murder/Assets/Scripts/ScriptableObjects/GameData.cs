using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Custom/GameData")]
public class GameData : ScriptableObject {
	public RoundQuantity rounds;
	public int mapNumber;
}
public enum RoundQuantity{
	ONE,THREE,FIVE
}
