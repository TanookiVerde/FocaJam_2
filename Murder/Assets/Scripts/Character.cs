using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour {
    public Vector2 position;

    public void SetMovement(Vector2 pos) { 
        Vector3 dir = pos - position;
        float tileSize = GameObject.Find("MapCreator").GetComponent<MapCreator>().tileSize;
        position += pos - position;
        transform.DOMove(transform.position + dir*tileSize, .5f);
    }
    public Vector2 GetRandomVector2(){
        Vector2 randVec = Vector2.up;
        switch(Random.Range(0,4)){
            case 0:
                randVec = Vector2.up;
                break;
            case 1:
                randVec = Vector2.left;
                break;
            case 2:
                randVec = Vector2.right;
                break;
            case 3:
                randVec = Vector2.down;
                break;
        }
        return randVec;
    }
}
