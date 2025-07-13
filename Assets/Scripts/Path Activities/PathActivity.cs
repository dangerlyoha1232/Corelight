using UnityEngine.UI;
using PlayerScripts;
using Services;
using UnityEngine;

namespace Path_Activities
{
    public class PathActivity : MonoBehaviour
    {
        [SerializeField] private Button _activityButton;
        [SerializeField] private RectTransform _activityRoot;
        [SerializeField] private float _distanceDetection;

        private bool _activated;

        private Player _player;

        private void Start()
        {
            _player = ServiceLocator.Current.Get<Player>();

            _activityButton.gameObject.SetActive(false);
            _activityRoot.gameObject.SetActive(false);

            ShowActivityPopup();
        }

        private void Update()
        {
            CheckPlayer();
        }

        private void CheckPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

            if (distanceToPlayer <= _distanceDetection && !_activated)
            {
                _activityButton.gameObject.SetActive(true);
            }
            else
            {
                _activityButton.gameObject.SetActive(false);
            }
        }

        private void ShowActivityPopup()
        {
            _activityButton.onClick.AddListener((() =>
            {
                _activityRoot.gameObject.SetActive(true);
                EventBus.SendOnActivityOpen();
                _activated = true;
            }));
        }
    }
}
