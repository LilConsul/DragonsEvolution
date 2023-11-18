using System;
namespace Scenes.Scripts {
    public class RadiusRemover<T> where T : class {
        
        public T[,] RemoveRadius(T[,] field, int x, int y, int radius) {
            if (x < 0 || x >= field.GetLength(0) || y < 0 || y >= field.GetLength(1)) {
                throw new ArgumentException("Invalid position provided.");
            }

            var newField = (T[,])field.Clone();

            for (var i = 0; i < field.GetLength(0); i++) {
                for (var j = 0; j < field.GetLength(1); j++) {
                    double distance = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2));
                    if (distance > radius) {
                        newField[i, j] = null;
                    }
                }
            }

            return newField;
        }

    }
}