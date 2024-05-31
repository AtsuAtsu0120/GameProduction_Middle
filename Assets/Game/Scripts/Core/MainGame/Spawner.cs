using System;
using Game.Scripts.Foundation;
using R3;
using R3.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Core.MainGame
{
    public class Spawner : MonoBehaviour
    {
        public Observable<int> OnDamagePlayer => _onDamagePlayer;
        
        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private float _spawnRate;
        [SerializeField] private int _damage;
        [SerializeField] private int _objectPoolCapacity;

        private readonly Subject<int> _onDamagePlayer = new();
        private ObjectPool<Arrow> _arrowObjectPool;
        private void Start()
        {
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(_spawnRate))
                .Subscribe(_ => SpawnArrow()).AddTo(this);

            
            _arrowObjectPool = new (_objectPoolCapacity, _arrowPrefab, arrow =>
            {
                arrow.OnCollisionEnter2DAsObservable()
                    .Subscribe(other =>
                    {
                        var arrowSelf = arrow;
                        if (other.collider.CompareTag("Player"))
                        {
                            _onDamagePlayer.OnNext(_damage);
                            _arrowObjectPool.InactiveObject(arrowSelf);  
                        }
                        else if(other.collider.CompareTag("Ground"))
                        {
                            _arrowObjectPool.InactiveObject(arrowSelf);
                        }
                    }).AddTo(arrow);
            });
        }
        private void SpawnArrow()
        {
            var arrow = _arrowObjectPool.ActiveObject();
            var randomPositionX = Random.Range(-10f, 10f);
            var position = arrow.transform.position;
            position.x = randomPositionX;
            position.y = transform.position.y;
            
            arrow.transform.position = position;
        }
    }
}
