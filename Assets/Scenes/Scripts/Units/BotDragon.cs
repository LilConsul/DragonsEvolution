using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using UnityEngine;

namespace Scenes.Scripts.Units {
    public class BotDragon : MonoBehaviour {
        [SerializeField] private SpriteRenderer _renderer;
        public int TimeToLive { get; set; }
        public bool IsParent { get; set; }
        private Food _prevEaten;
        private int _prevX;
        private int _prevY;
        private int _x;
        private int _y;

        private double _health;
        private double _speed;
        private double _intelligence;

        private int _isChild;
        [SerializeField] private EntityState _state;

        public double Health {
            get => _health;
            set {
                if (value > 5) {
                    _health = value;
                    return;
                }
                _health = 5;
            }
        }

        public double Speed {
            get => _speed;
            set {
                if (value > 1) {
                    _speed = value;
                    return;
                }
                _speed = 1;
            }
        }

        public double Intellect {
            get => _intelligence;
            set {
                if (value > 3){
                    _intelligence = value;
                    return;
                }
                _intelligence = 3;
            }
        }

        public EntityState State {
            get => _state;
            set {
                if (value == EntityState.Child)
                    _isChild = 3 * (int)Speed;
                _state = value;
            }
        }

        public delegate void DragonAction(BotDragon sender);

        public delegate void DragonBirth(BotDragon sender, int x, int y);

        public event DragonAction OnTimeToLiveEnd;
        public event DragonAction OnDeath;
        public event DragonBirth OnMate;

        public Colors Color { get; set; }

        public void Initialization(int x, int y) {
            TimeToLive = 5;
            Color = GetRandomColor();
            State = EntityState.Alive;
            IsParent = false;

            _health = 2;
            _intelligence = 3;
            _prevX = x;
            _prevY = y;
            _x = x;
            _y = y;
        }

        public void Move(int delX, int delY) {
            if (State == EntityState.Child) {
                _isChild -= 1;
                if (_isChild < 0)
                    State = EntityState.Alive;
                return;
            }
            if (State == EntityState.Dead) {
                TimeToLive -= 1;
                if (TimeToLive < 0) {
                    OnTimeToLiveEnd?.Invoke(this);
                }

                return;
            }

            if (_health < 0) {
                State = EntityState.Dead;
                OnDeath?.Invoke(this);
                return;
            }

            if (FieldContainer.Instance.ValidDragonCords(_x + delX, _y + delY)) {
                _prevEaten = null;
                _prevX = _x;
                _prevY = _y;
                _x += delX;
                _y += delY;
                _health -= 1 / Speed;
            }
        }

        public void Eat(Food food) {
            if (food == null)
                return;
            _prevEaten = food;
            _health += food.GetCalories();
        }
        
        public void Mate(int delX, int delY) {
            OnMate?.Invoke(this, _x + delX, _y + delY);
        }

        private Colors GetRandomColor() {
            var colors = Enum.GetValues(typeof(Colors));
            return (Colors)colors.GetValue(UnityEngine.Random.Range(0, colors.Length - 1));
        }

        public (int x, int y) Cords() => (_x, _y);
        public (int x, int y) PrevCords() => (_prevX, _prevY);
        public void Init(Sprite sprite) => _renderer.sprite = sprite;
        public Food PrevEaten() => _prevEaten;
        public int GetIntelligence() => (int)_intelligence;
    }
}