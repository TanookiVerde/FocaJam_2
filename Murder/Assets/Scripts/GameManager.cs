using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Preferences")]
    const int playerQuantity = 2;
    [SerializeField] private int charactersQuantity;

    [SerializeField] private GameObject cpuPrefab;
    [SerializeField] private MapCreator mapCreator;
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    private Dictionary<Vector2,GameObject> nextPositions = new Dictionary<Vector2, GameObject>();

    bool ended;

	void Start () {
        InitializeCharacters();
        InitializeCharacters();
        InitializeCharacters();
        InitializeCharacters();
        StartCoroutine(GameLoop());
    }
    private IEnumerator GameLoop(){
        while(!ended){
            nextPositions.Clear();
            AddObstaclesInDictionary();
            RandomizeCharactersMovement();
            yield return WaitForPlayerInput(0);
            yield return WaitForPlayerInput(1);
            yield return NewPositionsHandler();
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator WaitForPlayerInput(int playerId){
        Vector2 dir = Vector2.zero;
        while (!Input.GetKeyDown(KeyCode.Return)){
            Vector2 temp_dir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            if(temp_dir != Vector2.zero) dir = temp_dir;
            yield return null;
        }
        Vector2 newPos = characters[playerId].GetComponent<Character>().position + dir;
        AddOnDictionary(newPos,characters[playerId]);
        yield return new WaitWhile(() => Input.GetKeyDown(KeyCode.Return));
    }
    private IEnumerator NewPositionsHandler(){
        foreach(Vector2 v in nextPositions.Keys){
            if(nextPositions[v].GetComponent<Character>() != null){
                nextPositions[v].GetComponent<Character>().SetMovement(v);
            }
        }
        yield return new WaitForEndOfFrame();
    }
    private void RandomizeCharactersMovement(){
        for(int i = playerQuantity; i < characters.Count; i++){
            Vector2 pos = characters[i].GetComponent<Character>().GetRandomVector2() + characters[i].GetComponent<Character>().position;
            AddOnDictionary(pos,characters[i]);
        }
    }
    private void AddOnDictionary(Vector2 vector, GameObject gameObject){
        if(nextPositions.ContainsKey(vector)){
            if(nextPositions[vector].GetComponent<Character>() != null) nextPositions.Remove(vector);
        }else{
            nextPositions.Add(vector,gameObject);
        }
    }
    private void AddObstaclesInDictionary(){
        for(int y = 0; y < mapCreator.size+2; y++){
			for(int x = 0; x < mapCreator.size+2; x++){
				if(mapCreator.map[x,y] == 1){
                    Vector2 v = new Vector2(x,y);
                    nextPositions.Add(v,this.gameObject);
                }
			}
		}
    }
    private void InitializeCharacters(){
        int x = Random.Range(2,8); 
        int y = Random.Range(2,8);
        float tileSize = mapCreator.tileSize;
        float size = mapCreator.size+2;

        Vector3 position = new Vector3((float)x, (float)y, 0) * tileSize;
        position -= new Vector3(size*tileSize - tileSize*0.5f,size*tileSize - tileSize,0)*0.5f;
        var character = Instantiate(cpuPrefab, position, Quaternion.identity);
        character.GetComponent<Character>().position = new Vector2(x,y);
        characters.Add(character);
    }
}
public enum MoveDirection {
    Up, Down, Right, Left
}

