using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    public UnityEvent OnDead;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    private float _currentHealth;

    private void Start() => _currentHealth = _maxHealth;

    public bool IsDead => _currentHealth <= 0;

    public void TakeDamage(int damage)
    {
        if (IsDead) { return; }
        
        _currentHealth -= damage;
        if (IsDead)
        {
            Die();
        }
    }

    private void Die() => OnDead.Invoke();
}