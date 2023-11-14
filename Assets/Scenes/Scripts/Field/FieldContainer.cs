using System.Collections.Generic;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Field {
    public class FieldContainer /*: MonoBehaviour*/{
        //public static FieldContainer Instance;
        private BotDragon[,] _dragons;
        private Chicken[,] _foods;
        private Queue<BotDragon> _dragonsQue;
        
        /*private void Awake() {
            Instance = this;
        }*/
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

            (int x, int y) = food.Cords();
            if (!AreCoordinatesValid(x, y) || _dragons[x, y] != null)
                return false;

            if (_foods[x, y] != null)
                food.Add(_foods[x, y]);

            _foods[x, y] = food;
            return true;
        }

        private bool AreCoordinatesValid(int x, int y) {
            return x >= 0 && x < _foods.GetLength(0) && y >= 0 && y < _foods.GetLength(1);
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

        public T[,] GetUnitsField<T>() {
            if (typeof(T) == typeof(Chicken))
                return _foods as T[,];

            if (typeof(T) == typeof(BotDragon))
                return _dragons as T[,];

            else {
                Debug.LogWarning("Unsupported type requested for units field.");
                return null;
            }
        }

        public T[,] GetUnitsField<T>(int x, int y, int radius) {
            if (typeof(T) == typeof(Chicken)) {
                var remover = new RadiusRemover<Chicken>();
                return remover.RemoveRadius(_foods, x, y, radius) as T[,];
            } 
            if (typeof(T) == typeof(BotDragon)) {
                var remover = new RadiusRemover<BotDragon>();
                return remover.RemoveRadius(_dragons, x, y, radius) as T[,];
            } 
            else {
                Debug.LogWarning("Unsupported type requested for units field.");
                return null;
            }
        }
        
        public int Size() => _foods.GetLength(0);

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