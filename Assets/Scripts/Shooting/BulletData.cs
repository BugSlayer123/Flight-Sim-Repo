using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Create Bullet", menuName = "Bullet/Create Bullet", order = 1)]
public class BulletData : ScriptableObject
{
    public float speed;
    public float damage;
    public GameObject prefab;
}
