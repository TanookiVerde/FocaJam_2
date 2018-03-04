using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {

	const int size = 8;
	private int[,] map  = {{0,0,0,0,0,0,0,0},
						   {0,0,0,0,0,0,0,0},
						   {0,0,0,0,0,0,0,0},
						   {0,0,0,0,0,0,0,0},
						   {0,0,0,0,0,0,0,0},
						   {0,0,0,0,0,0,0,0},
						   {0,0,0,0,0,0,0,0},
						   {0,0,0,0,0,0,0,0}};
	[SerializeField] private Transform mapParent;
	[SerializeField] private List<GameObject> tiles;

	private void Start(){
		InitializeMap();
	}
	private void InitializeMap(){
		float tileSize = tiles[0].GetComponent<SpriteRenderer>().size.x;
		for(int y = 0; y < size; y++){
			for(int x = 0; x < size; x++){
				Vector3 position = new Vector3((float) x,(float) y, 0)*tiles[0].GetComponent<SpriteRenderer>().size.x;
				var go = Instantiate(tiles[0],position,Quaternion.identity);
				go.transform.parent = mapParent;
			}
		}
		mapParent.transform.position -= new Vector3(size*tileSize - tileSize*0.5f,size*tileSize - tileSize,0)*0.5f;
	}
}
