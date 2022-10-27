using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class PublicVars
{
    public static int playerHealth = 3;
    public static int maxHealth = 3;
    public static bool canSwim = false;
    public static bool canDash = false;
    public static int numberOfKeys = 0;
    public static Vector3 currentCheckpoint = Vector3.zero;
    public static PlayerInput input = new PlayerInput();
    public static bool fullscreen = true;
    public static InputDevice currentDevice = new Gamepad();
}
