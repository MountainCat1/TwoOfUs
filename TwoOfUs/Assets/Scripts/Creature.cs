using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private float speed;
    
    public void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}
