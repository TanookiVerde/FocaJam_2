using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public MapCreator mapCreator;
    List<GameObject> characters;

	void Start () {
        characters = mapCreator.characters;
        StartCoroutine(WaitForInput());
    }

    private IEnumerator WaitForInput() {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        foreach(GameObject ch in characters) {
            ch.GetComponent<Character>().SetMovement(0);
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(WaitForInput());
    }
}
