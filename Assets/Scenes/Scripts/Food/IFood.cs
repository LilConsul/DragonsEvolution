namespace Scenes.Scripts.Food {
    public abstract class IFood {
        protected int _x;
        protected int _y;
        protected int _calories;

        public virtual (int x, int y) Cords() => (_x, _y);
        public virtual int GetCalories() => _calories;
        public abstract IFood Add(IFood otherFood);
    }
}