using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement
    {
        get => GenericNotImplementedError<Movement>.TryGet(movement, transform.parent.name);
        private set => movement = value;
    }
    public CollisionSenses CollisionSenses
    {
        get => GenericNotImplementedError<CollisionSenses>.TryGet(collisionSenses, transform.parent.name);
        private set => collisionSenses = value;
    }
    public Combat Combat
    {
        get => GenericNotImplementedError<Combat>.TryGet(combat, transform.parent.name);
        private set => combat = value;
    }
    public Health Health
    {
        get => GenericNotImplementedError<Health>.TryGet(health, transform.parent.name);
        private set => health = value;
    }

    private Movement movement;
    private CollisionSenses collisionSenses;
    private Combat combat;
    private Health health;

    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();
        Combat = GetComponentInChildren<Combat>();
        Health = GetComponentInChildren<Health>();
    }

    private void Start()
    {
        Combat.onDamaged += OnDamaged;
        Health.onPlayerDeath += Health_onPlayerDeath;
    }

    private void Health_onPlayerDeath()
    {
        Debug.Log($"{gameObject.name} dead!");
        Destroy(transform.parent.gameObject);//assuming the parent is the main object.
    }

    private void OnDestroy()
    {
        Combat.onDamaged -= OnDamaged;
        Health.onPlayerDeath -= Health_onPlayerDeath;
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
        Combat.LogicUpdate();
        Health.LogicUpdate();
    }


    private void OnDamaged(float damage)
    {
        Health.TakeDamage(damage);
    }
}
