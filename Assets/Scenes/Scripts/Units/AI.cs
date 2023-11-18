using Scenes.Scripts.Field;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenes.Scripts.Units {
    public class AI : MonoBehaviour {
        private int[,] weights;
        private BotDragon _dragon;
        private BotDragon[,] _dragons;
        private Chicken[,] _chickens;

        private void SetInitialization(BotDragon dragon) {
            if (dragon == null) {
                Debug.Log("No dragon on field!");
            }

            var container = FieldContainer.Instance;

            weights = new int[container.Size(), container.Size()];

            var size = dragon.GetIntelligence();
            (int x, int y) = dragon.Cords();
            _dragon = dragon;
            _dragons = container.GetUnitsField<BotDragon>(x, y, size);
            _chickens = container.GetUnitsField<Chicken>(x, y, size);
            /*_dragons = container.GetUnitsField<BotDragon>();
            _chickens = container.GetUnitsField<Chicken>();*/
        }

        public int[,] TakeWeight() {
            return weights;
        }

        public (int x, int y) GetNextMove(BotDragon dragon) {
            SetInitialization(dragon);
            var sortedMoves = SortedMoves();
            foreach (var (x, y) in sortedMoves) {
                if (FieldContainer.Instance.ValidCords(_dragon.Cords().x + x, _dragon.Cords().y + y))
                    return (x, y);
            }
            return (0, 0);
        }

        private void SetWeight() {
            int size = _dragons.GetLength(0);
            var dragonCoords = _dragon.Cords();

            for (var i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if ((i, j) == dragonCoords) {
                        weights[i, j] = 0;
                        continue;
                    }

                    if (_chickens[i, j] != null) {
                        weights[i, j] = _chickens[i, j].GetCalories();
                        continue;
                    }

                    if (_dragons[i, j] != null) {
                        var myDrag = _dragons[i, j];
                        weights[i, j] = myDrag.Color == _dragon.Color ? -100 : 50;
                        continue;
                    }
                }
            }
        }

        private IEnumerable<KeyValuePair<(int x, int y), int>> MoveDictionary() {
            SetWeight();
            var size = weights.GetLength(0);
            var dragonCoords = _dragon.Cords();
            var center = dragonCoords;
            int sumUp = 0, sumDown = 0, sumLeft = 0, sumRight = 0;

            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    if (i < center.x) {
                        sumUp += weights[i, j];
                    }
                    else if (i > center.x) {
                        sumDown += weights[i, j];
                    }

                    if (j < center.y) {
                        sumLeft += weights[i, j];
                    }
                    else if (j > center.y) {
                        sumRight += weights[i, j];
                    }
                }
            }

            var moves = new List<KeyValuePair<(int x, int y), int>> {
                new KeyValuePair<(int x, int y), int>((0, 1), sumUp), // Move up
                new KeyValuePair<(int x, int y), int>((0, -1), sumDown), // Move down
                new KeyValuePair<(int x, int y), int>((-1, 0), sumLeft), // Move left
                new KeyValuePair<(int x, int y), int>((1, 0), sumRight) // Move right
            };
            return moves;
        }

        private List<(int x, int y)> SortedMoves() {
            var moves = MoveDictionary();
            var sortedMoves = moves.OrderByDescending(pair => pair.Value)
                .Select(pair => pair.Key)
                .ToList();
            return sortedMoves;
        }
    }
}