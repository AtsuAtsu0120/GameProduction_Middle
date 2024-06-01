using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.Core.MainGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private GameTimer _timer;

        [SerializeField] private Image _hpImage;
        
        private void Start()
        {
            SubscribePlayerObservable();
            SubscribeTimerObservable();
        }

        private void SubscribePlayerObservable()
        {
            _spawner.OnDamagePlayer.Subscribe(damage =>
            {
                _player.Damage(damage);
            }).AddTo(this);
            _spawner.OnHealPlayer.Subscribe(heal =>
            {
                _player.Heal(heal);
            }).AddTo(this);
            
            _player.OnHpChange.Subscribe(hp =>
            {
                _hpImage.fillAmount = hp / (float)_player.MaxHp;
            }).AddTo(this);
            _player.OnHpChange.Where(x => x < 0).Subscribe(_ =>
            {
                GameInfo.IsClear = false;
                
                _player?.Dispose();
                _spawner?.Dispose();
                
                SceneManager.LoadScene("ClearScreen");
            }).AddTo(this);
        }

        private void SubscribeTimerObservable()
        {
            _timer.OnClear.Subscribe(_ =>
            {
                GameInfo.IsClear = true;
                
                _player.Dispose();
                _spawner.Dispose();
                
                SceneManager.LoadScene("ClearScreen");
            }).AddTo(this);
        }

        private void Update()
        {
            ProvideInput();
        }

        private void ProvideInput()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _player.Move(true);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _player.Move(false);
            }
        }
    }
}
