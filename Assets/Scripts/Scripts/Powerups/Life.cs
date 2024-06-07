using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    private Player _player;
    private Transform _playerPos;
    [SerializeField]
    private bool _playerMagnetActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
        StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player.AddLife();
            Destroy(this.gameObject);
        }
    }

    public void PlayerMagnet()
    {
        _playerMagnetActive = true;
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        Destroy(this.gameObject);
    }

    void Movement()
    {
        if (_playerMagnetActive == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerPos.position, 8f * Time.deltaTime);
        }
        else
        {
        }

        if (transform.position.y <= -6.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
