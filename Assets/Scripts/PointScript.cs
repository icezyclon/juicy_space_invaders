using System;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    [SerializeField] public float ttl = 1f;
    [SerializeField] public float Delay = 1f;
    [SerializeField] public float AnimationSpeed = 1f;
    [SerializeField] public List<AudioClip> sounds;
    
    public int streak = 0;

    private float _delay = 0.0f;
    private bool _running = false;
    private AudioSource _audioSource;
    private Animator _animator;
    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _animator.speed = AnimationSpeed;
    }

    public void Update()
    {
        _delay += Time.deltaTime;
        if (_delay >= Delay && _running == false)
        {
            _running = true;
            _animator.SetInteger("Streak", streak);
            _renderer.enabled = true;
            _audioSource.clip = sounds[Math.Min(sounds.Count - 1, streak)];
            _audioSource.Play();
            Destroy(gameObject, ttl);
        }
    }
}