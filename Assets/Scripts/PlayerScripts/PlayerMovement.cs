using System.Collections.Generic;
using UnityEngine;
using Plates;
using Services;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour, IService
    {
        [SerializeField] private Plate _currentPlate;
        public Plate CurrentPlate => _currentPlate;
        
        [SerializeField] private float _speed = 3;
        [SerializeField] private List<Plate> _path = new List<Plate>();
        
        private List<Plate> _notConfirmedPath = new List<Plate>();
        private bool _isActivityPopupOpen;
        private bool _isMoving;

        private PlayerInput _playerInput;
        private EnergyWallet _playerWallet;
        private Animator _animator;

        public void Init()
        {
            _playerInput = ServiceLocator.Current.Get<PlayerInput>();
            _playerWallet = ServiceLocator.Current.Get<EnergyWallet>();
            _animator = GetComponent<Animator>();
            
            EventBus.OnActivityOpen += IsActivityPopupOpen;
            EventBus.OnActivityClose += IsActivityPopupClosed;
        }

        private void OnDestroy()
        {
            EventBus.OnActivityOpen -= IsActivityPopupOpen;
            EventBus.OnActivityClose -= IsActivityPopupClosed;
        }

        private void Update()
        {
            if (_playerInput.MouseLeftDown())
            {
                CreatePath();
            }
            Move();
            PlayerFinishedPath();
            
            _animator.SetBool("IsMoving", _isMoving);
        }

        private void CreatePath()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent<Plate>(out var plate) && !_isActivityPopupOpen && !_isMoving)
                {
                    _notConfirmedPath = PlatePath.Instance.GeneratePath(_currentPlate, plate);

                    if (_playerWallet.Energy > 0 && _playerWallet.IsSpendSuccessful(_notConfirmedPath.Count - 1))
                    {
                        _path = _notConfirmedPath;
                        transform.LookAt(new Vector3(plate.transform.position.x, transform.position.y, plate.transform.position.z));
                    }
                }
            }
        }

        private void Move()
        {
            if (_path.Count > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_path[0].transform.position.x, transform.position.y, _path[0].transform.position.z),
                    _speed * Time.deltaTime);

                _isMoving = true;
                
                if (Vector3.Distance(transform.position, new Vector3(_path[0].transform.position.x, transform.position.y, _path[0].transform.position.z)) < 0.01f)
                {
                    _currentPlate = _path[0];
                    _path.RemoveAt(0);
                }
            }
            else
            {
                _isMoving = false;
            }
        }

        private void IsActivityPopupOpen()
        {
            _isActivityPopupOpen = true;
        }

        private void IsActivityPopupClosed()
        {
            _isActivityPopupOpen = false;
        }

        private void PlayerFinishedPath()
        {
            if (_currentPlate != null && _currentPlate.GetComponentInChildren(typeof(Generator)))
                EventBus.SendOnPlayerFinishedPath();
        }
    }
}