using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Scripts.Core.MainGame.FallObj
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Arrow : MonoBehaviour
    {
        public int Damage { get; private set; }
        
        [SerializeField] private int MaxDamage;
        [SerializeField] private int MinDamage;

        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            SetDamage();
            SetColor();
        }

        private void SetDamage()
        {
            Damage = Random.Range(MinDamage, MaxDamage);
        }

        private void SetColor()
        {
            _spriteRenderer.color = new Color((float)Damage / MaxDamage, 0f, 0f);
        }
        
        #if UNITY_EDITOR
        private void Reset()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        #endif
    }
}
