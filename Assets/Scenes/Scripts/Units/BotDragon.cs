using System;
using Scenes.Scripts.Enums;
using UnityEngine;

namespace Scenes.Scripts.Units {
    public class BotDragon : MonoBehaviour{
        [SerializeField] private SpriteRenderer _renderer;
        private EntityState _state = EntityState.Alive;
        private int _x;
        private int _y;
        public Colors Color { get; set; }
        public void Initialization(int x, int y) {
            Color = GetRandomColor();
            _x = x;
            _y = y;
        }
        
        public void Move(int newX, int newY) {
            //TODO: Check if it can move 
            _x = newX;
            _y = newY;
            throw new System.NotImplementedException();
        }

        public void Eat(Chicken food) {
            if (food == null) 
                return;
            //TODO: Eat food;
            throw new NotImplementedException();
        }

        public void PerformDecision() {
            throw new NotImplementedException();
        }

        public EntityState GetState() {
            return _state;
        }
        
        private Colors GetRandomColor() {
            var colors = Enum.GetValues(typeof(Colors));
            return (Colors)colors.GetValue(UnityEngine.Random.Range(0, colors.Length - 1));
        }
        public (int x, int y) Cords() => (_x, _y);
        public void Init(Sprite sprite) => _renderer.sprite = sprite;
    }
}