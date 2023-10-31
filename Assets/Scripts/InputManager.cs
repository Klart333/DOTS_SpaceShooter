using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private float movementSpeed = 5;

    [SerializeField]
    private float handling = 1f;

    private PlayerInput PlayerInput;

    public InputAction Move { get; private set; }
    public InputAction Fire { get; private set; }

    public Vector2 CurrentDirection { get; private set; }

    public float2 GetMovement
    {
        get
        {
            return CurrentDirection * movementSpeed;
        }
    }

    public bool GetFire
    {
        get
        {
            return Fire.ReadValue<float>() > 0;
        }
    }

    private void OnEnable()
    {
        PlayerInput = new PlayerInput();

        Move = PlayerInput.Player.Move;
        Move.Enable();

        Fire = PlayerInput.Player.Fire;
        Fire.Enable();

        CurrentDirection = Vector2.right;
    }

    private void Update()
    {
        Vector2 input = Move.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.1)
        {
            CurrentDirection += input * handling * Time.deltaTime;
        }
        else
        {
            CurrentDirection -= CurrentDirection * handling * Time.deltaTime * 2;
        }
        CurrentDirection = Vector3.ClampMagnitude(CurrentDirection, 1);
    }

    private void OnDisable()
    {
        Move.Disable();
        Fire.Disable();
    }
}
