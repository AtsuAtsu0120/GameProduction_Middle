using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Core.MainGame
{
    public class HPChangeText : MonoBehaviour
    {
        public int HPChange
        {
            get => _hpChange;
            set
            {
                _hpChange = value;
                if (value > 0)
                {
                    _text.color = Color.green;
                }
                else
                {
                    _text.color = Color.red;
                }
                _text.text = _hpChange.ToString();
            }
        }

        public Observable<Unit> OnEndAnimation => _onEndAnimation;

        [SerializeField] private TextMeshPro _text;
        
        private int _hpChange;
        private Subject<Unit> _onEndAnimation = new();
        private async void OnEnable()
        {
            var position = transform.position;
            var newPosition = position;
            newPosition.y += 2;
            
            await UniTask.WhenAll
            (
                LMotion.Create(position, newPosition, 0.3f).BindToPosition(transform).AddTo(this).ToUniTask(destroyCancellationToken), 
                LMotion.Create(1f, 0f, 0.3f).WithEase(Ease.InOutBounce).Bind(x => _text.alpha = x).AddTo(this).ToUniTask(destroyCancellationToken)
            ).SuppressCancellationThrow();
            
            _onEndAnimation.OnNext(Unit.Default);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _text = GetComponent<TextMeshPro>();
        }
#endif
    }
}