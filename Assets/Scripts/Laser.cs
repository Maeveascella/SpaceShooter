using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable
    [SerializeField]
    private float _speed = 8f;

    private bool _isEnemyLaser = false;
    [SerializeField]
    private bool _isHomingLaser = false;

    private GameObject[] _enemiesSeen;
    private Transform _enemyPos;
    private BoxCollider2D[] _enemyCollider;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
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


        if (_isHomingLaser == true)
        {
            _enemiesSeen = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < _enemiesSeen.Length; i++)
            {
                _enemyPos = _enemiesSeen[i].GetComponent<Transform>();
            }

            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            Vector3 dir = transform.position - _enemyPos.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }


        if (transform.position.y >= 7.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
        transform.gameObject.tag = "EnemyLaser";
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