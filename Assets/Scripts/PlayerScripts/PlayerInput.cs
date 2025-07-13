using Services;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerInput : MonoBehaviour, IService
    {
        public bool MouseLeftDown() => Input.GetMouseButtonDown(0);
    }
}