using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable
    [SerializeField]
    private float _speed = 8f;

    private bool _isEnemyLaser = false;
    [SerializeField]
    private bool _isHomingLaser = false;

    private Transform _enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        _enemyPos = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false && _isHomingLaser == false)
        {
            MoveUp();
        }
        else if (_isHomingLaser == true)
        {
            HomingMove();
        }
        else
        {
            MoveDown();
        }

    }

    void MoveDown()
    {

        //translate laser down 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //destroy laser at 7.5 y
        if (transform.position.y <= -7.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveUp()
    {
        //translate laser up 
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        //destroy laser at 7.5 y
        if (transform.position.y >= 7.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void HomingMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemyPos.position, _speed * Time.deltaTime);
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void AssignHomingLaser()
    {
        _isHomingLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }

        if (other.tag == "Powerup" && _isEnemyLaser == true)
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}