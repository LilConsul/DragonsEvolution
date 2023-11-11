using System;
namespace Scenes.Scripts {
    public class RadiusRemover<T> where T : class {
        
        public T[,] RemoveRadius(T[,] field, int x, int y, int radius) {
            if (x < 0 || x >= field.GetLength(0) || y < 0 || y >= field.GetLength(1)) {
                throw new ArgumentException("Invalid position provided.");
            }

            var newField = (T[,])field.Clone();

            for (var i = Math.Max(0, x - radius); i <= Math.Min(field.GetLength(0) - 1, x + radius); i++) {
                for (var j = Math.Max(0, y - radius); j <= Math.Min(field.GetLength(1) - 1, y + radius); j++) {
                    if (Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2)) <= radius) {
                        newField[i, j] = null;
                    }
                }
            }

            return newField;
        }
    }
}