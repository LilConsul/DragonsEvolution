using System.Collections.Generic;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Units;

namespace Scenes.Scripts.Field {
    public class FieldContainer {
        private BotDragon[,] _dragons;
        private Chicken[,] _foods;
        private Queue<BotDragon> _dragonsQue;

        public FieldContainer(uint size) {
            _dragons = new BotDragon[size, size];
            _foods = new Chicken[size, size];
            _dragonsQue = new Queue<BotDragon>();
        }

        public bool Add(BotDragon dragon) {
            if (dragon == null || !InitializeOnField(dragon))
                return false;
            _dragonsQue.Enqueue(dragon);
            return true;
        }

        public bool Add(Chicken food) {
            if (food == null)
                return false;
            var (x, y) = food.Cords();
            if (_foods[x, y] != null)
                food.Add(_foods[x, y]);

            _foods[food.Cords().x, food.Cords().y] = food;
            return true;
        }

        public void DeleteFood(int x, int y) {
            _foods[x, y] = null;
        }

        public BotDragon GetNextDragon() {
            var next = _dragonsQue.Dequeue();
            _dragons[next.Cords().x, next.Cords().y] = null;
            if (next.GetState() == EntityState.Dead) {
                return GetNextDragon();
            }

            return next;
        }

        public Chicken[,] GetFoodField(int x, int y, int radius) {
            var remover = new RadiusRemover<Chicken>();
            var newField = remover.RemoveRadius(_foods, x, y, radius);
            return newField;
        }

        public Chicken[,] GetFoodField() {
            return _foods;
        }

        public BotDragon[,] GetDragonsField(int x, int y, int radius) {
            var remover = new RadiusRemover<BotDragon>();
            var newField = remover.RemoveRadius(_dragons, x, y, radius);
            return newField;
        }

        public BotDragon[,] GetDragonsField() {
            return _dragons;
        }

        public int Size() => _foods.GetLength(1);
        private bool InitializeOnField(BotDragon dragon) {
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