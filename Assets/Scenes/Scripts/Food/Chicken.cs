namespace Scenes.Scripts.Food {
    public class Chicken : IFood {
        public Chicken(int x, int y, int calories) {
            _x = x;
            _y = y;
            _calories = calories;
        }
        public override IFood Add(IFood otherFood) {
            return new Chicken(x: _x,y: _y, this._calories + otherFood.GetCalories());
        }
    }
}