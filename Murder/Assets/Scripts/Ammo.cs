using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {
    public Vector2 position;

    public void Collect() {
        Destroy(gameObject);
    }
}
