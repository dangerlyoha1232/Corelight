using Services;
using UnityEngine.UI;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Button _fillGeneratorButton;
    [SerializeField] private Slider _fillGeneratorSlider;
    [SerializeField] private int _energyToFill;
    [SerializeField] private float _cooldownDuringFilling;

    private EnergyWallet _playerWallet;
    
    private bool _isPlayerFinishedPath;
    private bool _isHeld;
    private bool _isFull;
    private float _currentCooldownDuringFilling;
    private int _currentEnergyInGenerator;
    
    private void Start()
    {
        _playerWallet = ServiceLocator.Current.Get<EnergyWallet>();
        
        _fillGeneratorSlider.maxValue = _energyToFill;

        EventBus.OnPlayerFinishedPath += PlayerOnFinalPlate;
    }

    private void Update()
    {
        if (_isHeld && !_isFull)
        {
            _currentCooldownDuringFilling -= Time.deltaTime;

            if (_currentCooldownDuringFilling <= 0)
            {
                FillingGenerator();
                _currentCooldownDuringFilling = _cooldownDuringFilling;
            }
        }
    }

    public void StartFillGenerator()
    {
        if (!_isPlayerFinishedPath)
            return;
        
        _isHeld = true;
        _currentCooldownDuringFilling = 0;
    }

    public void StopFillGenerator()
    {
        if (!_isPlayerFinishedPath)
            return;
        
        _isHeld = false;
    }

    private void FillingGenerator()
    {
        if (_playerWallet.IsSpendSuccessful(1))
            _fillGeneratorSlider.value += 1;
        else
            Debug.Log("Can't fill generator");
        
        if (_fillGeneratorSlider.value >= _energyToFill)
            _isFull = true;
    }

    private void PlayerOnFinalPlate()
    {
        _isPlayerFinishedPath = true;
        Debug.Log("Finished path");
    }
}
