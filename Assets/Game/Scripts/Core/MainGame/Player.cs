using System;
using Game.Scripts.Core.MainGame;
using Game.Scripts.Foundation;
using R3;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class Player : MonoBehaviour, IDisposable
    {
        public int MaxHp => _maxHp;
        public Observable<int> OnHpChange => _hp;
        
        [SerializeField] private int _maxHp;
        [SerializeField] private HPChangeText _hpChangeTextPrefab;

        private ReactiveProperty<int> _hp = new();
        private ObjectPool<HPChangeText> _textObjectPool;

        private void Start()
        {
            _hp.Value = _maxHp;

            _textObjectPool = new(5, _hpChangeTextPrefab, text =>
            {
                text.OnEndAnimation.Subscribe(obj => _textObjectPool.InactiveObject(text)).AddTo(text);
            });
            _textObjectPool.OnActive.Subscribe(x =>
            {
                x.transform.position = transform.position;
            });
        }

        public void Move(bool isRight)
        {
            var position = transform.position;
            position.x += isRight ? 0.1f : -0.1f;
            if (position.x > ScreenInfo.ScreenRight || position.x < ScreenInfo.ScreenLeft)
            {
                position.x *= -1;
            }
            transform.position = position;
        }

        public void Damage(int damage)
        {
            SeProvider.Instance.PlayDamageSound();
            _hp.Value -= damage;

            ShowText(-damage);
        }

        public void Heal(int heal)
        {
            SeProvider.Instance.PlayHealSound();
            if (_hp.Value < _maxHp)
            {
                _hp.Value += heal;   
            }
            
            ShowText(heal);
        }

        private void ShowText(int changeValue)
        {
            var hpChangeText = _textObjectPool.ActiveObject();
            hpChangeText.HPChange = changeValue;
        }

        public void Dispose()
        {
            _hp?.Dispose();
            _textObjectPool?.Dispose();
        }
    }
}
