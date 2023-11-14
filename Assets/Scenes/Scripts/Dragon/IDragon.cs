using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Food;
using UnityEngine;

namespace Scenes.Scripts.Dragon {
    public abstract class IDragon{
        [SerializeField] private SpriteRenderer _renderer;
        protected EntityState _state = EntityState.Alive;
        public Colors Color { get; set; }
        protected int _x;
        protected int _y;
        
        public abstract void Move(int newX, int newY);
        public abstract void PerformDecision();
        public abstract void Eat(IFood food);

        public abstract EntityState GetState();
        public (int x, int y) Cords() => (_x, _y);
        public void Init(Sprite sprite) => _renderer.sprite = sprite;
    }
}