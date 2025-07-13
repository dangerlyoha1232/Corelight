using System;
using UnityEngine;
using Services;

namespace PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public class PlayerData : ScriptableObject, IService
    {
        [SerializeField] private PlayerStats _playerStats;
        
        public PlayerStats PlayerStats => _playerStats;
    }


    [Serializable]
    public struct PlayerStats
    {
        public int StartEnergyCount;
    }
}