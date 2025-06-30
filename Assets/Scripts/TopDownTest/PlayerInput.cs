using UnityEngine;
 namespace TopDown
{

public class PlayerInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    
    public Vector2 MovementInput { get; private set; }
    public bool IsMoving => MovementInput.magnitude > 0.1f;
    
    private void Update()
    {
        GetMovementInput();
    }
    
    private void GetMovementInput()
    {
        float horizontal = 0f;
        float vertical = 0f;
        
        // Keyboard input
        if (Input.GetKey(leftKey)) horizontal -= 1f;
        if (Input.GetKey(rightKey)) horizontal += 1f;
        if (Input.GetKey(downKey)) vertical -= 1f;
        if (Input.GetKey(upKey)) vertical += 1f;
        
        // Alternative: Use Input Manager axes
        // horizontal = Input.GetAxisRaw("Horizontal");
        // vertical = Input.GetAxisRaw("Vertical");
        
        MovementInput = new Vector2(horizontal, vertical).normalized;
    }
}
}