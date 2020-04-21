using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyType enemyType;
        
    public float moveSpeed = 2f;
    public float speedMultiplier = 2.5f;
    public float visionDistance = 2f;
    public float wastedTime = 2f;
    public bool playerInSight;
    public float reachDistance = 0.6f;


    public Transform leftWaypoint;
    public Transform rightWaypoint;
    public float movingDir = 1f;
    
    
    private Rigidbody2D _rigidbody;
    private LayerMask _mask;
    private bool _canMove = true;
    private float _currentSpeed;
    private Vector3 _currentDestination;
    private Vector3 _leftPos;
    private Vector3 _rightPos;

    private Transform _player;
    private bool _hottieHasHim = false;

    public Computer enemysComputer;
    public bool enemysComputerRequiresPart;


    private void Start()
    {
        _leftPos = leftWaypoint.position;
        _rightPos = rightWaypoint.position;
        
        Destroy(leftWaypoint.gameObject);
        Destroy(rightWaypoint.gameObject);
        
        if (movingDir > 0)
            _currentDestination = _rightPos;
        else
            _currentDestination = _leftPos;
        
        _mask = LayerMask.GetMask("Player") ;
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentSpeed = moveSpeed;

        _player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        /*
         * if in vision -> stop and show warning sign, then start moving faster ++
         * when in reach, do your stuff ++
         */
        
        if (_canMove)
        {
            Move();
            
            if ((_currentDestination - transform.position).sqrMagnitude <= 0.3f && !playerInSight)
            {
                movingDir *= -1f;
                _currentDestination = movingDir > 0 ? _rightPos : _leftPos;
            }
        }
        
        float distance = movingDir * visionDistance;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distance, _mask);
        if (hit.collider != null && enemyType != EnemyType.Hottie)
        {
            //Debug.Log("hit: " + hit.collider.tag);
            if (!playerInSight)
            {
                // TODO: animation that in sight and alert && set new speed
                _currentSpeed *= speedMultiplier;
                playerInSight = true;
            }

            _currentDestination.x = hit.point.x;
            
            // check if in front
            if (Mathf.Abs(hit.point.x - transform.position.x) <= reachDistance)
            {
                // call event
                //Debug.Log("player in reach");
                // TODO: change to gamemanager call? or what?
                
                MenuManager.Instance.UpdateEnemySpeakBox(
                    enemyType == EnemyType.Delegator? TextManager.Instance.GetDelegatorText() 
                    : TextManager.Instance.GetBossText(),
                    enemyType);
                MenuManager.Instance.UpdatePlayerSpeakBox(
                    enemyType == EnemyType.Delegator ? TextManager.Instance.GetDelegatorProblem()
                    : TextManager.Instance.GetBossProblem());

                if (enemyType == EnemyType.Manager)
                {
                    GameManager.Instance.PlayerCaughtByManager();
                }

                if (enemyType == EnemyType.Delegator)
                {
                    if (enemysComputer != null)
                    {
                        enemysComputer.ComputerRepaired(false, Computer.Owner.Delegator, enemysComputerRequiresPart);
                        AudioManager.Instance.Play("broken");
                    }
                    GameManager.Instance.PlayerCaughtByDelegator(wastedTime);
                }
                
                
                Destroy(gameObject);
            }
        }
        else if (enemyType == EnemyType.Hottie)
        {
            if (Mathf.Abs(_player.transform.position.x - transform.position.x) <= visionDistance && 
                Mathf.Abs(_player.transform.position.y - transform.position.y) <= 1f)
            {
                if (!_hottieHasHim)
                {
                    _player.GetComponent<PlayerController>().movementSpeed *= 0.5f;
                    _hottieHasHim = true;
                    // call event
                    MenuManager.Instance.UpdateEnemySpeakBox(TextManager.Instance.GetHottieText(), enemyType);
                    MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetHottieProblem());
                }
            }
            else
            {
                if (_hottieHasHim)
                {
                    _player.GetComponent<PlayerController>().movementSpeed *= 2f;
                    _hottieHasHim = false;
                    // call event ? not sure yet
                    
                }
            }
        }
        else
        {
            if (playerInSight)
            {
                _currentSpeed /= speedMultiplier;
                ResetMoveDirection();
            }
            
            playerInSight = false;
        }
    }

    private void ResetMoveDirection()
    {
        // find which is further and in which direction
        float posX = transform.position.x;
        float tillLeft = Mathf.Abs(_leftPos.x - posX);
        float tillRight = Mathf.Abs(_rightPos.x - posX);

        if (tillLeft >= tillRight)
        {
            movingDir = -1f;
            _currentDestination = _leftPos;
        }
        else
        {
            movingDir = 1f;
            _currentDestination = _rightPos;
        }
    }

    private void Move()
    {
        Vector2 pos = _rigidbody.position;
        pos.x += movingDir * _currentSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(pos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (leftWaypoint != null)
        {
            Gizmos.DrawLine(transform.position, leftWaypoint.position);
            Gizmos.DrawWireSphere(leftWaypoint.position, 0.1f);
        }

        if (rightWaypoint != null)
        {
            Gizmos.DrawLine(transform.position, rightWaypoint.position);
            Gizmos.DrawWireSphere(rightWaypoint.position, 0.1f);
        }

        Gizmos.color = Color.white;
        if (enemyType == EnemyType.Delegator && enemysComputer != null)
        {
            Gizmos.DrawLine(transform.position, enemysComputer.transform.position);
        }
    }


    public enum EnemyType
    {
        Delegator,
        Hottie,
        Manager,
        Chatterbox
    }
}
