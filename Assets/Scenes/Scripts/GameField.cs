using System.Collections.Generic;

namespace Scenes.Scripts {
    public class GameField {
        private IDragon[,] _dragons;
        private IFood[,] _foods;
        private Queue<IDragon> _dragonsQue;

        public GameField(uint size) {
            _dragons = new IDragon[size, size];
            _foods = new IFood[size, size];
        }

        public bool AddDragon(IDragon dragon) {
            if (dragon == null || !InitializeOnField(dragon))
                return false;
            _dragonsQue.Enqueue(dragon);
            return true;
        }

        public void AddFood(IFood food) {
            if (food == null)
                return;
            var (x, y) = food.Cords();
            if (_foods[x, y] != null)
                food.Add(_foods[x, y]);

            _foods[food.Cords().x, food.Cords().y] = food;
        }

        public void DeleteFood(int x, int y) {
            _foods[x, y] = null;
        }

        public IDragon GetNextDragon() {
            var next = _dragonsQue.Dequeue();
            _dragons[next.Cords().x, next.Cords().y] = null;
            if (next.GetState() == EntityState.Dead) {
                return GetNextDragon();
            }

            return next;
        }

        public IFood[,] GetFoodField(int x, int y, int radius) {
            var remover = new RadiusRemover<IFood>();
            var newField = remover.RemoveRadius(_foods, x, y, radius);
            return newField;
        }

        public IFood[,] GetFoodField() {
            return _foods;
        }

        public IDragon[,] GetDragonsField(int x, int y, int radius) {
            var remover = new RadiusRemover<IDragon>();
            var newField = remover.RemoveRadius(_dragons, x, y, radius);
            return newField;
        }

        public IDragon[,] GetDragonsField() {
            return _dragons;
        }

        private bool InitializeOnField(IDragon dragon) {
            var (x, y) = dragon.Cords();
            if (_dragons[x, y] != null)
                return false;

            if (_foods != null) {
                dragon.Eat(_foods[x, y]);
                _foods[x, y] = null;
            }

            _dragons[x, y] = dragon;
            return true;
        }
    }
}