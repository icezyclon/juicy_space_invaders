using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    // Set this speed via the Unity Inspector Window
    public float speed = 1f;
    public GameObject bulletPrefab;
    public TMP_Text scoreText;
    public GameObject wonLostPanel;
    public TMP_Text wonLostText;
    public Animator animator;
    public List<int> streakScore = new List<int>() { 10, 100, 200, 500, 1000, 2000 };
    [SerializeField] public float StreakTime = 3f;
    [SerializeField] public List<AudioClip> annoucements;
    [SerializeField] public AudioClip winning;
    [SerializeField] public AudioClip losing;
    [SerializeField] public int AnnouceStreak = 6;

    private int _enemyCount = 0;
    private AudioSource _audioSource;
    private ScreenShake _screenShake;
    private void Awake()
    {
        _screenShake = Camera.main.gameObject.GetComponent<ScreenShake>();
        _audioSource = GetComponent<AudioSource>();
        var enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        var gridSize = enemySpawner.GridSize;
        _enemyCount = gridSize.x * gridSize.y; // enemy count equals maximum score
    }

    private int _score = 0;
    private int _streak = 0;
    private float _streakTime = 0.0f;
    private int _random = 0;

    // Update is called once per frame
    void Update()
    {
        if (_streakTime >= 0.0f)
        {
            _streakTime -= Time.deltaTime;
        }
        
        HandleMovement();
        HandleShooting();

        if (_audioSource.isPlaying)
        {
            _screenShake.shakeTime = 1f;
        }
        else
        {
            _screenShake.shakeTime = 0f;
        }
    }
    
    void HandleMovement()
    {
        // Get the horizontal axis.
        // By default this is mapped to the arrow keys but also to A and D.
        // The value is in the range -1 to 1
        float horz = Input.GetAxis("Horizontal");
        
        // update animation
        animator.SetFloat("Horizontal", horz);
        
        // Mulitply with speed factor
        horz *= speed;
        // Convert from meters per frame to meters per second
        horz *= Time.deltaTime; 
        
        // Move translation along the object's z-axis
        transform.Translate(horz, 0, 0);
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Spawn Bullet
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"Player - OnTriggerEnter2D: {other.gameObject.name}");
        if (other.CompareTag("Enemy"))
        {
            // game lost
            Lost();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log($"Player - OnCollisionEnter2D: {other.gameObject.name}");
    }

    public int IncreaseScore()
    {
        if (_streakTime <= 0.0f)
        {
            _streak = 0;
        }
        _streakTime = StreakTime;
        
        int addScore = streakScore[Math.Min(streakScore.Count - 1, _streak)];
        
        _streak++;
        _score += addScore;
        _enemyCount--;
        
        scoreText.text = $"Score: {_score}";

        if (_enemyCount <= 0)
        {
            Won();
        }
        else
        {
            if (_streak >= AnnouceStreak && !_audioSource.isPlaying)
            {
                int newRand = Random.Range(0, annoucements.Count - 1);
                while ((newRand = Random.Range(0, annoucements.Count - 1)) == _random);
                _random = newRand;
                _audioSource.clip = annoucements[_random];
                _audioSource.Play();
            }
        }
        
        return _streak - 1;
    }

    public void Won()
    {
        _audioSource.clip = winning;
        _audioSource.Play();
        wonLostPanel.SetActive(true);
        wonLostText.text = "You Won!";
    }

    public void Lost()
    {
        AudioSource otherAudio = GameObject.Find("MusicAudio").GetComponent<AudioSource>();
        otherAudio.loop = false;
        otherAudio.clip = losing;
        otherAudio.Play();
        wonLostPanel.SetActive(true);
        wonLostText.text = "You Lost!";
    }
}
