using System.Collections.Generic;
using System.Collections;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Path_Activities
{
    public class DiceMiniGame : MonoBehaviour
    {
        [SerializeField] private Button _betOnRegion;
        [SerializeField] private Button _betOnNumber;
        [SerializeField] private Button _leaveButton;
        [SerializeField] private TMP_InputField _playerBet;
        [SerializeField] private TMP_Text _playerFinalBetText;
        [SerializeField] private TMP_Text _triesCountText;
        [SerializeField] private Image _gameDice;
        [SerializeField] private int _rollCount;
        [SerializeField] private int _maxBet;
        [SerializeField] private int _triesCount;
        [SerializeField] private int _exitPenalty;
        [SerializeField] private int _regionWinMultiplier;
        [SerializeField] private int _numberWinMultiplier;
        [SerializeField] private List<Toggle> _sidesForBet;
        [SerializeField] private List<Sprite> _diceSides;

        private Toggle _currentSideForBet;
        private int _currentRollSide;
        private bool _isRolling;
        private bool _isBetPlaced;
        private int _playerBetAmount;
        private int _currentRegionBet;
        private bool _isBetOnRegion;

        private EnergyWallet _playerWallet;

        private void Start()
        {
            _playerWallet = ServiceLocator.Current.Get<EnergyWallet>();

            ButtonActions();
            PlaceBet();
        }

        private void Update()
        {
            ToggleActions();

            _triesCountText.text = $"Tries: {_triesCount}";
        }

        private void ButtonActions()
        {
            _betOnRegion.onClick.AddListener(() =>
            {
                TryRollDice();
                _isBetOnRegion = true;
            });
            _betOnNumber.onClick.AddListener((() =>
            {
                TryRollDice();
                _isBetOnRegion = false;
            }));
            _leaveButton.onClick.AddListener((() =>
            {
                gameObject.SetActive(false);
                _playerWallet.IsSpendSuccessful(_exitPenalty);
                EventBus.SendOnActivityClose();
            }));
        }

        private void ToggleActions()
        {
            if (!_isRolling)
            {
                foreach (var toggle in _sidesForBet)
                {
                    toggle.onValueChanged.AddListener((val =>
                    {
                        if (val)
                        {
                            _currentSideForBet = toggle;
                        }
                        else
                        {
                            _currentSideForBet = null;
                        }
                    }));
                    if (_currentSideForBet != null && toggle != _currentSideForBet)
                        toggle.gameObject.SetActive(false);
                    else
                        toggle.gameObject.SetActive(true);
                }
            }
        }

        IEnumerator RollDice()
        {
            var currentSideForBet = _currentSideForBet;
            _isRolling = true;
            _triesCount--;

            if (_sidesForBet.IndexOf(currentSideForBet) <= 2)
                _currentRegionBet = 1;
            else
                _currentRegionBet = 2;

            for (int i = 0; i < _rollCount; i++)
            {
                _currentRollSide = Random.Range(0, _diceSides.Count);
                _gameDice.sprite = _diceSides[_currentRollSide];
                yield return new WaitForSeconds(0.1f);
            }

            _isRolling = false;
            _isBetPlaced = false;

            if (_isBetOnRegion)
            {
                if (_currentRollSide <= 2 && _currentRegionBet == 1 || _currentRollSide > 2 && _currentRegionBet == 2)
                {
                    _playerBetAmount *= _regionWinMultiplier;
                    _playerWallet.IsGetEnergy(_playerBetAmount);
                    _playerBetAmount = 0;
                    Debug.Log("Player win on region");
                }
                else
                {
                    _playerBetAmount = 0;
                    Debug.Log("Player loose on region");
                }
            }
            else
            {
                if (_sidesForBet.IndexOf(currentSideForBet) == _currentRollSide)
                {
                    _playerBetAmount *= _numberWinMultiplier;
                    _playerWallet.IsGetEnergy(_playerBetAmount);
                    _playerBetAmount = 0;
                    Debug.Log("Player win on number");
                }
                else
                {
                    _playerBetAmount = 0;
                    Debug.Log("Player loose on number");
                }
            }
            
            _playerFinalBetText.text = "Your bet: 0";
        }

        private void TryRollDice()
        {
            if (_currentSideForBet != null && _isBetPlaced && _triesCount > 0)
            {
                StartCoroutine(RollDice());
            }
        }

        private void PlaceBet()
        {
            _playerBet.onEndEdit.AddListener((val =>
            {
                int value;
                bool bet = int.TryParse(val, out value);
                if (bet && value <= _maxBet)
                {
                    _playerWallet.IsGetEnergy(_playerBetAmount);
                    _playerBetAmount = value;
                    _playerFinalBetText.text = $"Your bet: {value}";
                    _playerWallet.IsSpendSuccessful(value);
                    _isBetPlaced = true;
                    Debug.Log(_playerBetAmount);
                }
            }));
        }
    }
}