using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace Scenes.Scripts.Units {
    public class BotDragon : MonoBehaviour{
        [SerializeField] private SpriteRenderer _renderer;
        private int _prevX;
        private int _prevY;
        private Chicken _prevEaten;
        private int _x;
        private int _y;

        private int _health;
        private int _speed;
        private EntityState _state = EntityState.Alive;
        
        
        public Colors Color { get; set; }
        public void Initialization(int x, int y) {
            Color = GetRandomColor();
            _x = x;
            _y = y;
        }
        
        public bool Move(int newX, int newY) {
            _prevEaten = null;
            _prevX = _x;
            _prevY = _y;
            _x = newX;
            _y = newY;
            return true;
        }
        
        public void Eat(Chicken food) {
            if (food == null) 
                return;
            FoodFactory.Instance.SpawnFood();
            _prevEaten = food;
            _health += food.EatMe();
        }

        public EntityState GetState() {
            return _state;
        }
        
        private Colors GetRandomColor() {
            var colors = Enum.GetValues(typeof(Colors));
            return (Colors)colors.GetValue(UnityEngine.Random.Range(0, colors.Length - 1));
        }
        public (int x, int y) Cords() => (_x, _y);
        public (int x, int y) PrevCords() => (_prevX, _prevY);
        public void Init(Sprite sprite) => _renderer.sprite = sprite;
        public Chicken PrevEaten() => _prevEaten;
    }
}