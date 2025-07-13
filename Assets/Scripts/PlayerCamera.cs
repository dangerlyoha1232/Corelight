using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private float _offsetZ;
    private float _offsetY;
    
    private void Start()
    {
        _offsetZ = transform.position.z;
        _offsetY = transform.position.y;
    }
    
    private void Update()
    {
        transform.position = new Vector3(_player.position.x, _player.position.y + _offsetY, _player.position.z + _offsetZ);
    }
}
