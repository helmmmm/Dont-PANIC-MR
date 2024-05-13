using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool _isPlayer1;
    public bool _movable;
    private LevelGenerator _levelGenerator;
    Game_SM _gameSM => Game_SM.Instance;
    private Cell _currentCell;

    private float[] lastInputTime = new float[6];
    private float _minInputDelay = 0.1f;

    [SerializeField] LayerMask _layerMask;
    private ParticleSystem _deathParticles;
    private Renderer _renderer;
    private Light _playerLight;

    private void Start()
    {
        // Check if scene is menu scene 
    }

    private void OnEnable()
    {
        _levelGenerator = GameObject.Find("Cells").GetComponent<LevelGenerator>();
        _deathParticles = GetComponentInChildren<ParticleSystem>();
        _renderer = GetComponent<Renderer>();
        _playerLight = GetComponentInChildren<Light>();
        _movable = false;
        _gameSM.GameState_PreGame.OnEnter += DisableMovement;
        _gameSM.GameState_PreRound.OnEnter += DisableMovement;
        _gameSM.GameState_MidRound.OnEnter += EnableMovement;
        _gameSM.GameState_PostRound.OnEnter += DisableMovement;
        _gameSM.GameState_GameOver.OnEnter += DisableMovement;
        _gameSM.GameState_Pause.OnEnter += DisableMovement;
    }

    private void EnableMovement()
    {
        _movable = true;
    }

    private void DisableMovement()
    {
        _movable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cell"))
        {
            _currentCell = other.GetComponent<Cell>();
        }

        if (other.CompareTag("Laser"))
        {
            Debug.Log("hit!+\n");
            DestroyPlayer();
        }
    }

    private void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // if any button pressed
        if (Input.anyKeyDown)
        {
            if (Time.realtimeSinceStartup - lastInputTime[0] < _minInputDelay) return;
            if (!_movable) return;
            lastInputTime[0] = Time.realtimeSinceStartup;

            if (_isPlayer1)
            {
                if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector3.forward;
                if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector3.back;
                if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector3.left;
                if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector3.right;
                if (Input.GetKeyDown(KeyCode.V)) moveDirection = Vector3.down;
                if (Input.GetKeyDown(KeyCode.G)) moveDirection = Vector3.up;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) moveDirection = -Vector3.forward;
                if (Input.GetKeyDown(KeyCode.DownArrow)) moveDirection = -Vector3.back;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) moveDirection = -Vector3.left;
                if (Input.GetKeyDown(KeyCode.RightArrow)) moveDirection = -Vector3.right;
                if (Input.GetKeyDown(KeyCode.Keypad1)) moveDirection = Vector3.down;
                if (Input.GetKeyDown(KeyCode.Keypad4)) moveDirection = Vector3.up;
            }

            Vector3 destination = transform.position + moveDirection * _levelGenerator._offset;

            if (IsCellOpenAt(destination))
            {
                transform.position = destination;
            }
        }
    }

    private bool IsCellOpenAt (Vector3 destination)
    {
        Vector3 currentPos = transform.position;
        RaycastHit hit;

        if (Physics.Raycast(currentPos, destination - currentPos, out hit, 1f, _layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.CompareTag("Cell"))
            {
                return !hit.collider.GetComponent<Cell>().IsOccupied;
            }
        }

        return false;
    }

    private void OnDisable()
    {
        _gameSM.GameState_PreGame.OnEnter -= DisableMovement;
        _gameSM.GameState_PreRound.OnEnter -= DisableMovement;
        _gameSM.GameState_MidRound.OnEnter -= EnableMovement;
        _gameSM.GameState_PostRound.OnEnter -= DisableMovement;
        _gameSM.GameState_GameOver.OnEnter -= DisableMovement;
        _gameSM.GameState_Pause.OnEnter -= DisableMovement;
    }

    private void DestroyPlayer()
    {
        _renderer.enabled = false;
        _playerLight.enabled = false;
        _deathParticles.Play();
        GameManager.Instance.IsGameOver = true;
    }
}
