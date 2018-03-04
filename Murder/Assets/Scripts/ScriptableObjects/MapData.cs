using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Custom/MapData")]
public class MapData : ScriptableObject {
    public List<Vector2> obstacles;
}
