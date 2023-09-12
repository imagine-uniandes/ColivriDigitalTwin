using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

sealed class GyroInputTester : MonoBehaviour
{
    #region Static class members

    // Layout extension written in JSON
    const string LayoutJson = @"{
      ""name"": ""DualShock4GamepadHIDCustom"",
      ""extend"": ""DualShock4GamepadHID"",
      ""controls"": [
        {""name"":""gyro"", ""format"":""VC3S"", ""offset"":13,
         ""layout"":""Vector3"", ""processors"":""ScaleVector3(x=-1,y=-1,z=1)""},
        {""name"":""gyro/x"", ""format"":""SHRT"", ""offset"":0 },
        {""name"":""gyro/y"", ""format"":""SHRT"", ""offset"":2 },
        {""name"":""gyro/z"", ""format"":""SHRT"", ""offset"":4 },
        {""name"":""accel"", ""format"":""VC3S"", ""offset"":19,
         ""layout"":""Vector3"", ""processors"":""ScaleVector3(x=-1,y=-1,z=1)""},
        {""name"":""accel/x"", ""format"":""SHRT"", ""offset"":0 },
        {""name"":""accel/y"", ""format"":""SHRT"", ""offset"":2 },
        {""name"":""accel/z"", ""format"":""SHRT"", ""offset"":4 }
      ]}";

    // Gyro vector data to movement conversion
    static Vector3 GyroInputToMovement(in InputAction.CallbackContext ctx)
    {
        // Gyro input data
        var gyro = ctx.ReadValue<Vector3>();

        // Define a threshold for when the gyro input is considered zero
        const float GyroZeroThreshold = 0.01f;

        // Check if the gyro input is close to zero
        if (gyro.sqrMagnitude < GyroZeroThreshold * GyroZeroThreshold)
        {
            return Vector3.zero; // No movement
        }

        // Coefficient converting a gyro data value into a movement factor
        const float GyroToMovementFactor = 0.2f;

        // Delta time from the last event
        var dt = ctx.time - ctx.control.device.lastUpdateTime;
        dt = Mathf.Min((float)dt, 1.0f / 60f); // Cast dt to float and discard large deltas

        // Create a Vector3 with only X and Y components, and then scale it
        var gyro2D = new Vector3(-gyro.z * 1000f, -gyro.x * 1000f, 0f);

        // Define a drift reduction factor (adjust as needed)
        const float driftFactor = 0.001f;

        // Apply drift reduction
        gyro2D -= _instance._gyroDrift * driftFactor;

        return gyro2D * (GyroToMovementFactor * (float)dt);
    }

    #endregion

    #region Private members

    // Accumulation of gyro input for movement
    Vector3 _accGyroMovement = Vector3.zero;

    // Gyro drift reduction factor
    Vector3 _gyroDrift = Vector3.zero;

    #endregion

    #region MonoBehaviour implementation

    // Singleton instance
    static GyroInputTester _instance;

    void Awake()
    {
        // Assign the singleton instance
        _instance = this;
    }

    void Start()
    {
        // DS4 input layout extension
        InputSystem.RegisterLayoutOverride(LayoutJson);

        // Gyroscope input callback
        var action = new InputAction(binding: "<Gamepad>/gyro");
        action.performed += ctx => _accGyroMovement += GyroInputToMovement(ctx);
        action.Enable();
    }

    void Update()
    {
        // Current position
        var position = transform.localPosition;

        // Apply accumulated gyro input to position
        position += _accGyroMovement;

        // Update the GameObject's position
        transform.localPosition = position;

        // Update gyro drift
        _gyroDrift += _accGyroMovement;
    }

    #endregion
}
