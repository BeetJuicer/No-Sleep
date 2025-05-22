using UnityEngine;

public class Catfish : Entity
{

    public Catfish_IdleState idleState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;

    public override void Awake()
    {
        base.Awake();

        idleState = new Catfish_IdleState(this, stateMachine, "idle", idleStateData, this);


    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
