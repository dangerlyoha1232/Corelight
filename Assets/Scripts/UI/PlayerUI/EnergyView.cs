using UnityEngine;
using PlayerScripts;
using Services;
using TMPro;

namespace UI.PlayerUI
{
    public class EnergyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _energyText;
        
        private float _energy;
        private PlayerData _playerData;

        private void Start()
        {
            _playerData = ServiceLocator.Current.Get<PlayerData>();
            _energy = _playerData.PlayerStats.StartEnergyCount;
            _energyText.text = $"Energy: {_energy:F0}";
            
            EventBus.OnEnergyChanged += OnEnergyChanged;
        }

        private void OnEnergyChanged(int energy)
        {
            _energyText.text = $"Energy: {energy}";
        }
    }
}