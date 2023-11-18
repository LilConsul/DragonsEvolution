using Scenes.Scripts.Field;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Scenes.Scripts.Units {
    public class AI : MonoBehaviour {
        private int[,] weights;
        private bool _sameResults;
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
            if (_sameResults) {
                sortedMoves = RandomMove(sortedMoves);
            }

            foreach (var (x, y) in sortedMoves) {
                if (FieldContainer.Instance.ValidCords(_dragon.Cords().x + x, _dragon.Cords().y + y)) {
                    Debug.LogWarning($"The best move is {x} {y}");
                    return (x, y);
                }

                Debug.LogWarning($"Dragon is locked!");
            }

            return (0, 0);
        }

        private List<(int x, int y)> RandomMove(List<(int x, int y)> sortedMoves) {
            var rand = new Random();
            for (var i = sortedMoves.Count - 1; i > 0; i--) {
                var j = rand.Next(0, i + 1);
                (sortedMoves[i], sortedMoves[j]) = (sortedMoves[j], sortedMoves[i]);
            }
            return sortedMoves;
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


            _sameResults = sumDown == sumUp && sumUp == sumLeft && sumLeft == sumRight;
            //Check if all results are the same;
            var moves = new List<KeyValuePair<(int x, int y), int>> {
                new((-1, 0), sumUp), // Move up
                new((1, 0), sumDown), // Move down
                new((0, -1), sumLeft), // Move left
                new((0, 1), sumRight) // Move right
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