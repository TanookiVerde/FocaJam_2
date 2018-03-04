using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour {
    [SerializeField] private SpriteRenderer bulletIcon;
    [SerializeField] private GameObject bulletObject;

    public Vector2 position;
    public bool hasBullet;

    private void Start() {
        SetAmmo(false);
    }

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
    public void SetAmmo(bool value) {
        bulletIcon.enabled = value;
        hasBullet = value;
    }
    public void ShootBullet(Vector3 direction) {
        bulletObject.SetActive(true);
        bulletObject.transform.rotation = Quaternion.Euler(0, 0, -Vector2.Angle(direction, Vector2.up));
        bulletObject.transform.DOMove(bulletObject.transform.position + (direction * 20), 2);
        SetAmmo(false);
    }
    public void Die(){
        Destroy(this.gameObject,0.5f);
    }
}
