using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CPU : Character {

    public override void SetMovement(MoveDirection direction) {
        MoveDirection dir = (MoveDirection)Random.Range(0, 4);

        Vector3 vecDir = Vector2.zero;
        switch (dir) {
            case MoveDirection.Up:
                vecDir = Vector2.up;
                break;
            case MoveDirection.Down:
                vecDir = Vector2.down;
                break;
            case MoveDirection.Left:
                vecDir = Vector2.left;
                break;
            case MoveDirection.Right:
                vecDir = Vector2.right;
                break;
        }

        transform.DOMove(transform.position + vecDir, .5f);
    }
}
