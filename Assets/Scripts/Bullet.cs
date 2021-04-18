using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public GameObject deathAnim;
    [SerializeField] public GameObject scoreAnim;
    public float speed = 1f;

    private Player player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        var movementY = speed * Time.deltaTime;
        // Move the bullet along the y-axis
        transform.Translate(0f, movementY, 0f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"Bullet - OnTriggerEnter: {other.name}");
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy.aboutToDestroy)
            {
                return;
            }
            enemy.aboutToDestroy = true;
            
            
            // hit enemy, hence increase score
            if (deathAnim)
            {
                Instantiate(deathAnim, other.transform.position, Quaternion.identity);
            }

            int streak = player.IncreaseScore();
            if (streak > 0 && scoreAnim)
            {
                GameObject scoreAnimObj = Instantiate(scoreAnim, other.transform.position, Quaternion.identity);
                scoreAnimObj.GetComponent<PointScript>().streak = streak;
            }
            
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("Top"))
        { // bullet self-destroys when hitting top screen bounds collider
            Destroy(gameObject);
        }
    }
}
