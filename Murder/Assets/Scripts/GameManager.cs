using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Preferences")]
    const int playerQuantity = 2;
    [SerializeField] private int charactersQuantity;

    [SerializeField] private GameObject characterPrefab, ammoPrefab;
    [SerializeField] private MapCreator mapCreator;
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private List<GameObject> ammos = new List<GameObject>();
    private Dictionary<Vector2,GameObject> nextPositions = new Dictionary<Vector2, GameObject>();
    [SerializeField] private bool[] shallShoot;
    private Vector2[] shootDirection;

    [SerializeField] private Text gameState;

    bool ended;

	void Start () {
        shallShoot = new bool[playerQuantity];
        shootDirection = new Vector2[playerQuantity];

        InitializeCharacters();
        InitializeCharacters();
        InitializeCharacters();

        SpawnAmmo();
        SpawnAmmo();

        StartCoroutine(GameLoop());
    }
    private void Update() {
        if (Input.GetKeyDown("1")) {
            characters[0].GetComponent<SpriteRenderer>().color = Color.blue;
            characters[1].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private IEnumerator GameLoop(){
        while(!ended){
            nextPositions.Clear();
            AddObstaclesInDictionary();
            RandomizeCharactersMovement();
            yield return WaitForPlayerInput(0);
            yield return WaitForPlayerInput(1);
            if (isMurderTime()) {
                ShootBullets();
                ended = true;
            } else {
                yield return NewPositionsHandler();
                yield return new WaitForEndOfFrame();
            }
        }
    }
    private IEnumerator WaitForPlayerInput(int playerId){
        Vector2 dir = Vector2.zero;
        Character character = characters[playerId].GetComponent<Character>();
        gameState.text = "Get Player " + playerId + " input";

        while (!Input.GetKeyDown(KeyCode.Return)){
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (character.hasBullet) {
                    gameState.text = "Player " + playerId + ": select direction";
                    shallShoot[playerId] = true;
                    yield return GetShootDirection(playerId);
                } else {
                    //error feedback
                }
            }

            Vector2 temp_dir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            if (temp_dir != Vector2.zero) {
                gameState.text = "Player " + playerId + ": Press Start";
                dir = temp_dir;
            }
            yield return null;
        }
        Vector2 newPos = character.position + dir;
        AddOnDictionary(newPos,characters[playerId]);
        yield return new WaitWhile(() => Input.GetKeyDown(KeyCode.Return));
    }
    private IEnumerator GetShootDirection(int id) {
        Vector2 temp_dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        while(temp_dir == Vector2.zero) {
            temp_dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(temp_dir.x != 0 && temp_dir.y != 0) { temp_dir.x = 0; }
            yield return null;
        }
        shootDirection[id] = temp_dir;
    }
    private bool isMurderTime() {
        foreach(bool b in shallShoot) {
            if (b) return true;
        } return false;
    }
    private IEnumerator NewPositionsHandler(){
        gameState.text = "Moving";
        foreach (Vector2 v in nextPositions.Keys){
            if(nextPositions[v].GetComponent<Character>() != null){
                nextPositions[v].GetComponent<Character>().SetMovement(v);
            }
        }
        CheckAmmoCollect();
        yield return new WaitForEndOfFrame();
    }
    private void CheckAmmoCollect() {
        bool ammoColleted = false;
        GameObject collected = new GameObject();
        foreach(GameObject a in ammos) {
            Ammo ammo = a.GetComponent<Ammo>();
            foreach(GameObject b in characters) {
                Character character = b.GetComponent<Character>();
                if(ammo.position == character.position) {
                    character.SetAmmo(true);
                    collected = a;
                    ammoColleted = true;
                }
            }
        }
        if (ammoColleted) {
            ammos.Remove(collected);
            collected.GetComponent<Ammo>().Collect();
        }
    }
    private void ShootBullets() {
        for(int i = 0; i < shallShoot.Length; i++) {
            if (shallShoot[i]) {
                CheckBulletHit(characters[i].GetComponent<Character>().position, shootDirection[i]);
                characters[i].GetComponent<Character>().ShootBullet(shootDirection[i]);
            }
        }
    }
    private void CheckBulletHit(Vector2 startPos, Vector2 direction) {
        bool hit = false;
        Vector2 bulletPos = startPos;
        for(int i = 0; i < mapCreator.size && !hit; i++) {
            bulletPos += direction;
            foreach(GameObject go in characters) {
                Debug.Log("bulletPos: " + bulletPos);
                if (bulletPos == go.GetComponent<Character>().position) {
                    Destroy(go);
                    hit = true;
                    break;
                }
            }
        }
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
        int x = Random.Range(2, 8);
        int y = Random.Range(2, 8);
        float tileSize = mapCreator.tileSize;
        float size = mapCreator.size + 2;
        Vector3 position = new Vector3((float)x, (float)y, 0) * tileSize;
        position -= new Vector3(size * tileSize - tileSize * 0.5f, size * tileSize - tileSize, 0) * 0.5f;

        var character = Instantiate(characterPrefab, position, Quaternion.identity);
        character.GetComponent<Character>().position = new Vector2(x, y);
        characters.Add(character);
    }

    private void SpawnAmmo() {
        int x = Random.Range(2, 8);
        int y = Random.Range(2, 8);
        Debug.Log("x: " + x + ",y: " + y);
        float tileSize = mapCreator.tileSize;
        float size = mapCreator.size + 2;
        Vector3 position = new Vector3((float)x, (float)y, 0) * tileSize;
        position -= new Vector3(size * tileSize - tileSize * 0.5f, size * tileSize - tileSize, 0) * 0.5f;

        var ammo = Instantiate(ammoPrefab, position, Quaternion.identity);
        ammo.GetComponent<Ammo>().position = new Vector2(x, y);
        ammos.Add(ammo);
    }
}
public enum MoveDirection {
    Up, Down, Right, Left
}

