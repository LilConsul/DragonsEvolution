using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using Scenes.Scripts.Globals;
using UnityEngine;

namespace Scenes.Scripts.Units {
    public class BotDragon : MonoBehaviour{
        [SerializeField] private SpriteRenderer _renderer;
        private int _x;
        private int _y;

        private int health;
        
        
        private EntityState _state = EntityState.Alive;
        public Colors Color { get; set; }
        public void Initialization(int x, int y) {
            Color = GetRandomColor();
            _x = x;
            _y = y;
        }
        
        public bool Move(int newX, int newY) {
            _x = newX;
            _y = newY;
            return true;
        }

        private void HandleMove(int oldX, int oldY, int newX, int newY) {
            Debug.Log($"Object moved from ({oldX}, {oldY}) to ({newX}, {newY})");
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