using UnityEngine;

public interface IDamageable
{
    void damageTaken(int damage, Vector2 knockback, float force);
}