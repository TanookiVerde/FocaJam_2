using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {

	public const int totalSize = 10;
	public int size = totalSize - 2;
	public int[,] map  = {{1,1,1,1,1,1,1,1,1,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,0,0,0,0,0,0,0,0,1},
						   {1,1,1,1,1,1,1,1,1,1}};
	public List<MapData> mapData;
	[SerializeField] private Transform mapParent;
	[SerializeField] private List<GameObject> tiles;
	[HideInInspector] public float tileSize;
    [SerializeField] private Color tileColor1, tileColor2;

	public int currentMap;

	private void Start(){
		tileSize = tiles[0].GetComponent<SpriteRenderer>().size.x;
	}
	public void ReadMapData(int i){
		currentMap = i;
        //Le o scriptable obj
        foreach(MapData.Obstacle obstacle in mapData[currentMap].obstacles){
            int x = (int)obstacle.position.x, y = (int)obstacle.position.y;
            map[(int)x,(int)y] = 1;
            Vector2 position = new Vector2((float)x, (float)y) * tiles[0].GetComponent<SpriteRenderer>().size.x;
            var go = CreateTile((TileType)1, position);
            go.name = tiles[map[x,y]].name +" ("+x+","+y+")";
            SetOrdingLayerByPosition(go.GetComponent<SpriteRenderer>(), y, totalSize);
            go.GetComponent<SpriteRenderer>().sprite = obstacle.sprite;
            if (obstacle.mirrored) go.transform.Rotate(Vector2.up * 180);
        }
    }
    public void InitializeMap(){
        //A partir da matriz map o metodo instancia os tiles. Ele centraliza a posicao de mapParente e também faz um efeito de xadrez com as cores
        for(int y = 0; y < totalSize; y++){
            for(int x = 0; x < totalSize; x++){
                int tileIndex = map[x,y];
                Vector2 position = new Vector2((float) x,(float) y)*tiles[0].GetComponent<SpriteRenderer>().size.x;
                GameObject go = CreateTile(0, position);
                go.name = tiles[map[x,y]].name +" ("+x+","+y+")";
                /*if(map[x,y] == TileType.EMPTY.GetHashCode())*/ SetColorByPosition(go.GetComponent<SpriteRenderer>(),x+y);
                SetOrdingLayerByPosition(go.GetComponent<SpriteRenderer>(), y, totalSize);
                if (x == 0 || y == 0 || x == totalSize - 1 || y == totalSize - 1) {
                    //jesus cristo
                    GameObject wall = CreateTile((TileType)1, position);
                    SetOrdingLayerByPosition(wall.GetComponent<SpriteRenderer>(), y, totalSize);
                }
            }
        }
        mapParent.transform.position -= new Vector3(totalSize*tileSize - tileSize*0.5f,totalSize*tileSize - tileSize,0)*0.5f;
    }
	private GameObject CreateTile(TileType type, Vector2 position){
		//Cria um tile de tipo type na posicao position. Tambem o coloca como filho de mapParent, para depois centralizar.
		GameObject tile = tiles[type.GetHashCode()];
		var go = Instantiate(tile,position,Quaternion.identity);
		go.transform.parent = mapParent;
		return go; 
	}
	private void SetColorByPosition(SpriteRenderer sprite, int posSum){
        //Muda a cor baseado na soma das coordenadas. Se for par pinta de cinza. Impar fica branco.
        sprite.color = tileColor1;
		if(posSum % 2 == 0){
			sprite.color = tileColor2;
        }
	}
    private void SetOrdingLayerByPosition(SpriteRenderer sprite, int posY, int totalSize) {
        sprite.sortingOrder = totalSize - posY;
    }
}
public enum TileType{
	EMPTY, WALL
}
