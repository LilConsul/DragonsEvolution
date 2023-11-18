using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using UnityEngine;

namespace Scenes.Scripts.Units {
    public class BotDragon : MonoBehaviour{
        [SerializeField] private SpriteRenderer _renderer;
        public int TimeToLive { get; set; }
        private Chicken _prevEaten;
        private int _prevX;
        private int _prevY;
        private int _x;
        private int _y;

        private double _health;
        private double _speed;
        private double _intelligence;
        public EntityState State { get; set; }
        
        public event Action OnTimeToLiveEnd;
        
        public Colors Color { get; set; }
        public void Initialization(int x, int y) {
            TimeToLive = 5;
            Color = GetRandomColor();
            State = EntityState.Alive;
            _intelligence = 3;
            _prevX = x;
            _prevY = y;
            _x = x;
            _y = y;
        }
        
        public void Move(int delX, int delY) {
            if (State == EntityState.Dead) {
                TimeToLive -= 1;
                if(TimeToLive < 0)
                    OnTimeToLiveEnd?.Invoke();
            }
            if (_health < 0){
                State = EntityState.Dead;
                Color = Colors.Dead;
                return;
            }
            
            _prevEaten = null;
            
            _prevX = _x;
            _prevY = _y;
            
            _x += delX;
            _y += delY;
            
            _health -= 1;
        }
        
        public void Eat(Chicken food) {
            if (food == null) 
                return;
            FoodFactory.Instance.SpawnFood();
            _prevEaten = food;
            _health += food.GetCalories();
        }

        public EntityState GetState() {
            return State;
        }
        
        private Colors GetRandomColor() {
            var colors = Enum.GetValues(typeof(Colors));
            return (Colors)colors.GetValue(UnityEngine.Random.Range(0, colors.Length - 1));
        }
        public (int x, int y) Cords() => (_x, _y);
        public (int x, int y) PrevCords() => (_prevX, _prevY);
        public void Init(Sprite sprite) => _renderer.sprite = sprite;
        public Chicken PrevEaten() => _prevEaten;
        public int GetIntelligence() => (int)_intelligence;
    }
}