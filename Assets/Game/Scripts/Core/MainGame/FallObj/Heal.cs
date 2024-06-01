using UnityEngine;

namespace Game.Scripts.Core.MainGame.FallObj
{
    public class Heal : MonoBehaviour
    {
        public int HealPoint { get; private set; }
        
        [SerializeField] private int MaxHealPoint;
        [SerializeField] private int MinHealPoint;

        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            SetHealPoint();
            SetColor();
        }
        private void SetHealPoint()
        {
            HealPoint = Random.Range(MinHealPoint, MaxHealPoint);
        }
        
        private void SetColor()
        {
            _spriteRenderer.color = new Color(0f, (float)HealPoint / MaxHealPoint, 0f);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
#endif
    }
}