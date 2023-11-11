namespace Scenes.Scripts {
    public abstract class IFood {
        private int _x;
        private int _y;
        private int _calories;

        public abstract (int x, int y) Cords();
        public abstract int GetCalories();
        public abstract IFood Add(IFood otherFood);
    }
}