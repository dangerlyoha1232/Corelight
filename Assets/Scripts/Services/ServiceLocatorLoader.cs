using UnityEngine;
using PlayerScripts;
using Plates;

namespace Services
{
    public class ServiceLocatorLoader : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Player _player;
        private EnergyWallet _playerWallet;
        
        [Header("Plates")]
        [SerializeField] private PlatePath _platePath;
        
        private void Awake()
        {
            _playerWallet = new EnergyWallet(_playerData.PlayerStats.StartEnergyCount);
            
            RegisterServices();
            Init();
        }

        private void RegisterServices()
        {
            ServiceLocator.Initialize();
            
            ServiceLocator.Current.Register<PlayerInput>(_playerInput);
            ServiceLocator.Current.Register<PlatePath>(_platePath);
            ServiceLocator.Current.Register<PlayerData>(_playerData);
            ServiceLocator.Current.Register<EnergyWallet>(_playerWallet);
            ServiceLocator.Current.Register<Player>(_player);
            ServiceLocator.Current.Register<PlayerMovement>(_playerMovement);
        }

        private void Init()
        {
            _platePath.Init();
            _player.Init();
            _playerMovement.Init();
        }
    }
}