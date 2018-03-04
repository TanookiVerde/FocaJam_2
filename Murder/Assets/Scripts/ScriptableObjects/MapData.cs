using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(menuName="Custom/MapData")]
public class MapData : ScriptableObject {
    [System.Serializable]
    public struct Obstacle {
        public Vector2 position;
        public Sprite sprite;
        public bool mirrored;
    }
    public List<Obstacle> obstacles;
}