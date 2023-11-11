namespace Scenes.Scripts {
    public abstract class IDragon {
        protected int _x;
        protected int _y;
        protected EntityState _state = EntityState.Alive;
        
        public abstract void Move(int newX, int newY);
        public abstract void PerformDecision();
        public abstract void Eat(IFood food);
        public abstract (int x, int y) Cords();
        public abstract EntityState GetState();
    }
}