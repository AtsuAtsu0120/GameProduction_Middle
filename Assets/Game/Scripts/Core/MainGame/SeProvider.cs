using System;
using UnityEngine;

namespace Game.Scripts.Core.MainGame
{
    public class SeProvider : MonoBehaviour
    {
        public static SeProvider Instance => _instance;

        [Header("クリップ")] 
        [SerializeField] private AudioClip _damage;
        [SerializeField] private AudioClip _heal;
        
        [Header("コンポーネント")]
        [SerializeField] private AudioSource _audioSource;
        
        private static SeProvider _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void PlayDamageSound()
        {
            _audioSource.PlayOneShot(_damage);
        }
        public void PlayHealSound()
        {
            _audioSource.PlayOneShot(_heal);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _audioSource = GetComponent<AudioSource>();
        }
#endif
    }
}
