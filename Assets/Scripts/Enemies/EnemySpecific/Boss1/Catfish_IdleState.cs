using UnityEngine;

public class Catfish_IdleState : IdleState
{
    private Catfish catfish;
    public Catfish_IdleState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Catfish catfish) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.catfish = catfish;
    }
    
}
