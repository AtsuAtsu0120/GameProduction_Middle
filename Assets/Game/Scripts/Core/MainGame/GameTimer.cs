using R3;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Core.MainGame
{
    public class GameTimer : MonoBehaviour
    {
        public Observable<Unit> OnClear => _onClear;
        
        [SerializeField] private float _startTime;
        [SerializeField] private TextMeshProUGUI _timer;

        private const string Format = "{0:0.00}";
        private float _time;

        private Subject<Unit> _onClear = new();

        private void Start()
        {
            ResetTime();
        }

        private void Update()
        {
            UpdateTimer();
            CheckTime();
        }

        private void ResetTime()
        {
            _time = _startTime;
        }
        private void UpdateTimer()
        {
            _time -= Time.deltaTime;
            _timer.SetText(string.Format(Format, _time));
        }

        private void CheckTime()
        {
            if (_time < 0.0f)
            {
                _onClear.OnNext(Unit.Default);
            }
        }
    }
}