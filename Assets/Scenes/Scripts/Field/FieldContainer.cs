using System;
using System.Collections.Generic;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Globals;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Field {
    public class FieldContainer : MonoBehaviour {
        public static FieldContainer Instance;
        private BotDragon[,] _dragons;
        private Food[,] _foods;
        private LinkedList<BotDragon> _dragonsQue;
        private bool _gameStarted;

        private void Awake() {
            Instance = this;
        }

        public void SetSize(uint size) {
            _gameStarted = false;
            _dragons = new BotDragon[size, size];
            _foods = new Food[size, size];
            _dragonsQue = new LinkedList<BotDragon>();
        }

        public void StartGame() {
            _gameStarted = true;
            FoodFactory.Instance.StartGame();
            DragonFactory.Instance.StartGame();
        }

        public bool Add(BotDragon dragon, bool updateField = true) {
            if (dragon == null || !InitializeOnField(dragon))
                return false;
            _dragonsQue.AddLast(dragon);
            if (_gameStarted && updateField && dragon.State != EntityState.Dead) {
                FieldDrawer.Instance.UpdateUnits<BotDragon>(dragon);
            }
            dragon.OnMate += MateAction;
            return true;
        }

        public bool AddFirst(BotDragon dragon) {
            if (dragon == null || !InitializeOnField(dragon))
                return false;
            _dragonsQue.AddFirst(dragon);
            if (_gameStarted && dragon.State != EntityState.Dead) {
                FieldDrawer.Instance.UpdateUnits<BotDragon>(dragon);
            }
            dragon.OnMate += MateAction;
            return true;
        }

        public void ReturnMove(BotDragon dragon) {
            dragon.Move(dragon.PrevCords().x, dragon.PrevCords().y);
            Add(dragon, false);
        }

        public bool Add(Food food) {
            if (food == null)
                return false;
            var (x, y) = food.Cords();
            if (!ValidFoodCords(x, y) || !ValidDragonCords(x, y)) return false;

            _foods[x, y] = food;
            return true;
        }
        
        public BotDragon GetNextDragon() {
            var next = _dragonsQue.First.Value;
            _dragonsQue.RemoveFirst();
            _dragons[next.Cords().x, next.Cords().y] = null;
            if (next.TimeToLive < 0)
                return GetNextDragon();
            return next;
        }

        public T[,] GetUnitsField<T>() {
            if (typeof(T) == typeof(Food))
                return _foods as T[,];

            if (typeof(T) == typeof(BotDragon))
                return _dragons as T[,];

            Debug.LogWarning("Unsupported type requested for units field.");
            return null;
        }

        public T[,] GetUnitsField<T>(int x, int y, int radius) {
            if (typeof(T) == typeof(Food)) {
                var remover = new RadiusRemover<Food>();
                return remover.RemoveRadius(_foods, x, y, radius) as T[,];
            }

            if (typeof(T) == typeof(BotDragon)) {
                var remover = new RadiusRemover<BotDragon>();
                return remover.RemoveRadius(_dragons, x, y, radius) as T[,];
            }

            Debug.LogWarning("Unsupported type requested for units field.");
            return null;
        }

        public int Size() => _dragons.GetLength(0);

        private bool InitializeOnField(BotDragon dragon) {
            var (x, y) = dragon.Cords();

            if (dragon.State == EntityState.Dead) {
                _dragons[x, y] = dragon;
                return true;
            }

            if (!ValidDragonCords(x, y))
                return false;

            _dragons[x, y] = dragon;
            if (_foods[x, y] != null) {
                dragon.Eat(_foods[x, y]);
                if(GlobalSettings.Instance.spawnNewChickens) 
                    FoodFactory.Instance.SpawnFood();
                _foods[x, y] = null;
            }

            return true;
        }

        private void MateAction(BotDragon sender, int x, int y) {
            if(!CanMate(sender, x, y)) return;
            var dragon = _dragons[x, y];
            if(dragon.IsParent || sender.IsParent) return;
            
            dragon.Color = sender.Color;
            dragon.IsParent = sender.IsParent = true;
            
            var newSpeed = (sender.Speed + dragon.Speed) / 2;
            var newIntellect = (sender.Intellect + dragon.Intellect) / 2;
            var newFood = (dragon.Health + sender.Health) / 2;
            dragon.Health = sender.Health = newFood;

            DragonFactory.Instance.SpawnSpecialDragon(cords: NearestFree(x, y), 
                newFood, newSpeed, newIntellect, dragon.Color);
            
            FieldDrawer.Instance.UpdateUnits<BotDragon>(dragon);
            FieldDrawer.Instance.UpdateUnits<BotDragon>(sender);
        }
        
        public bool CanMate(BotDragon sender, int x, int y) {
            try {
                var dragon = _dragons[x, y];
                if (dragon == null) return false;
                if (sender.Color == dragon.Color) return false;
                if (sender.IsParent || dragon.IsParent) return false;
                var cords = sender.Cords();
                return Mathf.Abs(cords.x - x) + Mathf.Abs(cords.y - y) == 1;
            }
            catch (IndexOutOfRangeException) { return false; }
        }

        public bool ValidDragonCords(int x, int y) {
            try {
                if (_dragons[x, y] != null) return false;
            }
            catch (IndexOutOfRangeException) {
                return false;
            }

            return true;
        }

        private bool ValidFoodCords(int x, int y) {
            try {
                if (_foods[x, y] != null) return false;
            }
            catch (IndexOutOfRangeException) {
                return false;
            }

            return true;
        }
        
        private (int x, int y) NearestFree(int x, int y) {
            var queue = new Queue<(int x, int y)>();
            var visited = new HashSet<(int x, int y)>();

            queue.Enqueue((x, y));
            visited.Add((x, y));

            while (queue.Count > 0) {
                var current = queue.Dequeue();
                if (ValidDragonCords(current.x, current.y) && ValidFoodCords(current.x, current.y)) {
                    return current; 
                }
                EnqueueAdjacentCoordinates(current.x, current.y, queue, visited);
            }

            return (x, y);
        }
        private void EnqueueAdjacentCoordinates(int x, int y, Queue<(int x, int y)> queue, HashSet<(int x, int y)> visited) {
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            for (var i = 0; i < 4; i++) {
                var newX = x + dx[i];
                var newY = y + dy[i];

                var newCoord = (newX, newY);

                if (!visited.Contains(newCoord) && newX >= 0 && newX < Size() && newY >= 0 && newY < Size()) {
                    visited.Add(newCoord);
                    queue.Enqueue(newCoord);
                }
            }
        }
    }
}