using R3;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class Player : MonoBehaviour
    {
        public int MaxHp => _maxHp;
        public Observable<int> OnHpChange => _hp;
        [SerializeField] private int _maxHp;

        private ReactiveProperty<int> _hp = new();

        private void Start()
        {
            _hp.Value = _maxHp;
        }

        public void Move(bool isRight)
        {
            var position = transform.position;
            position.x += isRight ? 0.1f : -0.1f;

            if (position.x > 10 || position.x < -10)
            {
                position.x *= -1;
            }
            transform.position = position;
        }

        public void Damage(int damage)
        {
            _hp.Value -= damage;
        }
    }
}
