using Scenes.Scripts.Enums;
using Scenes.Scripts.Food;

namespace Scenes.Scripts.Dragon {
    public abstract class IDragon {
        protected EntityState _state = EntityState.Alive;
        protected int _x;
        protected int _y;
        
        public abstract void Move(int newX, int newY);
        public abstract void PerformDecision();
        public abstract void Eat(IFood food);

        public virtual (int x, int y) Cords() => (_x, _y);
        public abstract EntityState GetState();
    }
}