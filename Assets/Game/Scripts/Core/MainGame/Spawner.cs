using System;
using System.Linq;
using Game.Scripts.Core.MainGame.FallObj;
using Game.Scripts.Foundation;
using R3;
using R3.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Core.MainGame
{
    public class Spawner : MonoBehaviour, IDisposable
    {
        public Observable<int> OnDamagePlayer => _onDamagePlayer;
        public Observable<int> OnHealPlayer => _onHealPlayer;

        [Header("FallObject")] 
        [SerializeField] private int[] _spawnProbabilities;
        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private Heal _healPrefab;
        
        [Header("Settings")]
        [SerializeField] private float _spawnRate;
        [SerializeField] private int _objectPoolCapacity;

        private readonly Subject<int> _onDamagePlayer = new();
        private readonly Subject<int> _onHealPlayer = new();
        private ObjectPool<Arrow> _arrowObjectPool;
        private ObjectPool<Heal> _healObjectPool;
        private void Start()
        {
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(_spawnRate))
                .Subscribe(_ => SpawnObj()).AddTo(this);

            
            _arrowObjectPool = new (_objectPoolCapacity, _arrowPrefab, arrow =>
            {
                arrow.OnCollisionEnter2DAsObservable()
                    .Subscribe(other =>
                    {
                        var arrowSelf = arrow;
                        if (other.collider.CompareTag("Player"))
                        {
                            _onDamagePlayer.OnNext(arrowSelf.Damage);
                            _arrowObjectPool.InactiveObject(arrowSelf);  
                        }
                        else if(other.collider.CompareTag("Ground"))
                        {
                            _arrowObjectPool.InactiveObject(arrowSelf);
                        }
                    }).AddTo(arrow);
            });

            _healObjectPool = new(_objectPoolCapacity, _healPrefab, heal =>
            {
                heal.OnCollisionEnter2DAsObservable()
                    .Subscribe(other =>
                    {
                        var healSelf = heal;
                        if (other.collider.CompareTag("Player"))
                        {
                            _onHealPlayer.OnNext(healSelf.HealPoint);
                            _healObjectPool.InactiveObject(healSelf);  
                        }
                        else if(other.collider.CompareTag("Ground"))
                        {
                            _healObjectPool.InactiveObject(healSelf);
                        }
                    }).AddTo(heal);
            });
        }

        private void SpawnObj()
        {
            var random = Random.value;

            Transform objTransform = null;
            var result = random * _spawnProbabilities.Sum();
            if (result < _spawnProbabilities[0])
            {
                objTransform = _arrowObjectPool.ActiveObject().transform;
            }
            else
            {
                result -= _spawnProbabilities[0];
            }
            
            if (result < _spawnProbabilities[1] && objTransform is null)
            {
                objTransform = _healObjectPool.ActiveObject().transform;
            }
            else
            {
                result -= _spawnProbabilities[1];
            }
            
            var randomPositionX = Random.Range(ScreenInfo.ScreenLeft, ScreenInfo.ScreenRight);
            var position = objTransform.position;
            position.x = randomPositionX;
            position.y = transform.position.y;
            
            objTransform.position = position;
        }

        public void Dispose()
        {
            _onDamagePlayer?.Dispose();
            _onHealPlayer?.Dispose();
            _arrowObjectPool?.Dispose();
            _healObjectPool?.Dispose();
        }
    }
}
