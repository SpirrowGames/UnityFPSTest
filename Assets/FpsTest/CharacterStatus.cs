using System;
using UnityEngine;
using UnityEngine.Events;

namespace FpsTest
{
    /// <summary>
    /// プレイヤーの現在の状態・値をまとめて管理するクラス
    /// </summary>
    [Serializable]
    public class CharacterStatus : MonoBehaviour
    {
        [ReadOnly] public float health;

        public float maxHealth;
        
        [HideInInspector] public bool isDead;

        private CharacterMovement _character;
        private CharacterStatus _status;
        private MovementStates _states;
        
        private void Start()
        {
            GetAllReferences();

            health = maxHealth;
        }

        private void Update()
        {
            if (isDead) return;

            if (health <= 0) Die();
        }

        public void Damage(float damage)
        {
            var actualDamage = Mathf.Abs(damage);
            
            health -= actualDamage;
        }

        private void Die()
        {
            isDead = true;
        }

        private void GetAllReferences()
        {
            _status = GetComponent<CharacterStatus>();
            _states = GetComponent<MovementStates>();
            _character = GetComponent<CharacterMovement>();
        }
    }
}