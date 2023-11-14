using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Food;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Scenes.Scripts.Dragon {
    public class BotDragon : MonoBehaviour{
        [SerializeField] private SpriteRenderer _renderer;
        private EntityState _state = EntityState.Alive;
        private int _x;
        private int _y;
        public Colors Color { get; set; }
        
        /*public BotDragon(int x, int y) {
            Color = (Colors)System.Enum.GetValues(typeof(Colors)).Length - 1;
            _x = x;
            _y = y;
        }*/
        
        public void Move(int newX, int newY) {
            //TODO: Check if it can move 
            _x = newX;
            _y = newY;
            throw new System.NotImplementedException();
        }

        public void Eat(IFood food) {
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
        
        public (int x, int y) Cords() => (_x, _y);
        //public void Init(Sprite sprite) => _renderer.sprite = sprite;
    }
}