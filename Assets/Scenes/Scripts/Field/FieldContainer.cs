using System;
using System.Collections.Generic;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Field {
    public class FieldContainer : MonoBehaviour {
        public static FieldContainer Instance;
        public bool OnlyMove { get; set; }
        private BotDragon[,] _dragons;
        private Chicken[,] _foods;
        private Queue<BotDragon> _dragonsQue;
        private bool _gameStarted;

        private void Awake() {
            Instance = this;
        }

        public void SetSize(uint size) {
            _gameStarted = false;
            _dragons = new BotDragon[size, size];
            _foods = new Chicken[size, size];
            _dragonsQue = new Queue<BotDragon>();
        }

        public void StartGame() {
            _gameStarted = true;
        }

        public bool Add(BotDragon dragon, bool UpdateField = true) {
            if (dragon == null || !InitializeOnField(dragon))
                return false;
            _dragonsQue.Enqueue(dragon);
            if (_gameStarted && UpdateField) FieldDrawer.Instance.UpdateUnits<BotDragon>(dragon);
            return true;
        }

        public bool ReturnMove(BotDragon dragon) {
            dragon.Move(dragon.PrevCords().x, dragon.PrevCords().y);
            return Add(dragon, false);
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

        public int Size() => _dragons.GetLength(0);

        private bool InitializeOnField(BotDragon dragon) {
            var (x, y) = dragon.Cords();
            if (!ValidCords(x, y))
                return false;

            _dragons[x, y] = dragon;
            OnlyMove = true;
            if (_foods[x, y] != null) {
                OnlyMove = false;
                dragon.Eat(_foods[x, y]);
                _foods[x, y] = null;
            }

            return true;
        }

        private bool ValidCords(int x, int y) {
            try {
                if (_dragons[x, y] != null)
                    return false;
            }
            catch (IndexOutOfRangeException) {
                return false;
            }
            return true;
        }
    }
}