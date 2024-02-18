using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] private float _floatingHeight = 1.2f;
    [SerializeField] private float _speed = 10f;
    private float _startHeight;

    private void Awake()
    {
        _startHeight = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3
            (transform.position.x, _startHeight + Mathf.Sin(_speed * Time.time) * _floatingHeight);
    }
}
