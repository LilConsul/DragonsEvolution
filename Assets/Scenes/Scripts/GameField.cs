using System.Collections.Generic;
using System.Linq;

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

        public IDragon GetNextDragon() {
            var next = _dragonsQue.Dequeue();

            return next;
        }

        private bool InitializeOnField(IDragon dragon) {
            var (x, y) = dragon.GetCords();
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