using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private Player _player;
    private float _speed = 3.5f;

    private Transform _playerPos;

    private bool _playerMagnetActive = false;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveDown();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player.AddAmmo(Random.Range(4,9));
            Destroy(this.gameObject);
        }
    }

    public void PlayerMagnet()
    {
        _playerMagnetActive = true;
    }


    void MoveDown()
    {
        if (_playerMagnetActive == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerPos.position, 8f * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y <= -6.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
