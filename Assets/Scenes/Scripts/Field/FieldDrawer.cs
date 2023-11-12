using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Scripts.Field {
    public class FieldDrawer : MonoBehaviour {
        private int _width = 16, _height = 9;
        [SerializeField]private Tile tilePrefab;
        
        public void Draw(FieldContainer fieldContainer = null) {
            if (fieldContainer != null) {
                _width = fieldContainer.Size();
                _height = fieldContainer.Size();
            }
            
            for (var i = 0; i < _width; i++) {
                for (var j = 0; j < _height; j++) {
                    var isBase = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                    tilePrefab.Init(isBase);
                    
                    var spawnedTile = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity);
                    spawnedTile.name = $"Tile{i} {j}";
                }
            }
        }
    }
}