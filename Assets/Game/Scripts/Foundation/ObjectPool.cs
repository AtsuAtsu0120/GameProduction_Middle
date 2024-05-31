using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Scripts.Foundation
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        public Observable<T> OnActive => _onActive;
        public Observable<T> OnInactive => _onInactive;
        
        private Subject<T> _onActive = new();
        private Subject<T> _onInactive = new();
        private Subject<T> _onInitialize = new();
 
        private T _prefab;
        private Queue<T> _pool;
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="capacity">オブジェクトの最大数</param>
        /// <param name="prefab">使用するPrefab</param>
        public ObjectPool(int capacity, T prefab)
        {
            _pool = new Queue<T>(capacity);
            _prefab = prefab;
        
            Initialize(capacity);
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="capacity">オブジェクトの最大数</param>
        /// <param name="prefab">使用するPrefab</param>
        /// <param name="onInitialize">初期化するときの関数</param>
        public ObjectPool(int capacity, T prefab, Action<T> onInitialize)
        {
            _pool = new Queue<T>(capacity);
            _prefab = prefab;

            _onInitialize.Subscribe(obj => onInitialize.Invoke(obj));
            Initialize(capacity);
        }

        /// <summary>
        /// プールから取り出してオブジェクトを表示する。
        /// </summary>
        /// <returns>取り出されたオブジェクト</returns>
        public T ActiveObject()
        {
            if (_pool.TryDequeue(out var releaseObj))
            {
                _onActive.OnNext(releaseObj);
                releaseObj.gameObject.SetActive(true);
            
                return releaseObj;
            }
            
            #if UNITY_EDITOR
            Debug.LogWarning("オブジェクトプールで新規にオブジェクトが生成されました。最適化するにはcapacityの調整をしてください。");
            #endif
            var obj = Object.Instantiate(_prefab);
            _onInitialize.OnNext(obj);
            return obj;
        }
        /// <summary>
        /// オブジェクトを非表示にしてプールする。
        /// </summary>
        /// <param name="obj">その対象</param>
        public void InactiveObject(T obj)
        {
            _onInactive.OnNext(obj);
            obj.gameObject.SetActive(false);
        
            _pool.Enqueue(obj);
        }

        private void Initialize(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                var obj = Object.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
                
                _onInitialize.OnNext(obj);
            }
        }
    }
}