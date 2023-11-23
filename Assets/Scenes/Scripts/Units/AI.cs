using System.Linq;
using UnityEngine;
using Scenes.Scripts.Field;
using System.Collections.Generic;
using Scenes.Scripts.Enums;
using Random = System.Random;

namespace Scenes.Scripts.Units {
    public class AI : MonoBehaviour {
        private int[,] _weights;
        private bool _sameResults;
        private BotDragon _dragon;
        private BotDragon[,] _dragons;
        private Food[,] _chickens;

        private void SetInitialization(BotDragon dragon) {
            if (dragon == null) {
                Debug.Log("No dragon on field!");
            }

            var container = FieldContainer.Instance;

            _weights = new int[container.Size(), container.Size()];

            var size = dragon.GetIntelligence();
            var (x, y) = dragon.Cords();
            _dragon = dragon;
            _dragons = container.GetUnitsField<BotDragon>(x, y, size);
            _chickens = container.GetUnitsField<Food>(x, y, size);
        }

        public int[,] TakeWeight() {
            return _weights;
        }

        public (int x, int y, bool mate) GetNextMove(BotDragon dragon) {
            if (dragon.State == EntityState.Child) return (0, 0, false);
            SetInitialization(dragon);
            var sortedMoves = SortedMoves();
            if (_sameResults) {
                sortedMoves = RandomMove(sortedMoves);
            }

            foreach (var (x, y) in sortedMoves) {
                if (FieldContainer.Instance.ValidDragonCords(
                        x: _dragon.Cords().x + x,
                        y: _dragon.Cords().y + y)) {
                    return (x, y, false);
                }

                if (FieldContainer.Instance.CanMate( _dragon, x: _dragon.Cords().x + x, y: _dragon.Cords().y + y)) {
                    return (x, y, true);
                }
            }

            Debug.LogWarning($"Dragon is locked!");
            return (0, 0, false);
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
            var size = _dragons.GetLength(0);
            var dragonCoords = _dragon.Cords();

            for (var i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if ((i, j) == dragonCoords) {
                        _weights[i, j] = 0;
                        continue;
                    }

                    if (_chickens[i, j] != null) {
                        _weights[i, j] = _chickens[i, j].GetCalories();
                        continue;
                    }

                    if (_dragons[i, j] != null) {
                        var myDrag = _dragons[i, j];
                        if (myDrag.State is EntityState.Dead or EntityState.Child) {
                            _weights[i, j] = 0;
                            continue;
                        }

                        if (myDrag.IsParent || _dragon.IsParent) {
                            _weights[i, j] = 0;
                            continue;
                        }
                        _weights[i, j] = myDrag.Color == _dragon.Color ? -1 : 5;
                    }
                }
            }
        }

        private IEnumerable<KeyValuePair<(int x, int y), double>> MoveDictionary() {
            SetWeight();
            var size = _weights.GetLength(0);
            var dragonCoords = _dragon.Cords();
            var center = dragonCoords;
            double sumUp = 0.0, sumDown = 0.0, sumLeft = 0.0, sumRight = 0.0;

            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    if (i < center.x) {
                        sumUp += Evaluate(i, j, center);
                    }
                    else if (i > center.x) {
                        sumDown += Evaluate(i, j, center);
                    }

                    if (j < center.y) {
                        sumLeft += Evaluate(i, j, center);
                    }
                    else if (j > center.y) {
                        sumRight += Evaluate(i, j, center);
                    }
                }
            }

            _sameResults = sumDown == sumUp && sumUp == sumLeft && sumLeft == sumRight;
            //Check if all results are the same;
            var moves = new List<KeyValuePair<(int x, int y), double>> {
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

        private double Evaluate(int i, int j, (int x, int y) center) {
            // Calculate the Manhattan distance between (i, j) and the center (dragonCoords)
            var distanceX = Mathf.Abs(center.x - i);
            var distanceY = Mathf.Abs(center.y - j);
            var totalDistance = distanceX + distanceY;

            var isNeighbor = totalDistance == 1;
            // If (i, j) is a neighbor, return the weight, otherwise return the weight divided by the distance
            if (isNeighbor && _weights[i, j] == 0) {
                return _weights[i, j];
            }
            return _weights[i, j] / (totalDistance == 0 ? 1.0 : totalDistance);
        }
    }
}