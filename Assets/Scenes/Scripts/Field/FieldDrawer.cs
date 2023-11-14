using Scenes.Scripts.Enums;
using Scenes.Scripts.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Scripts.Field {
    public class FieldDrawer : MonoBehaviour {
        private int _width = 16, _height = 9;
        private Tile[,] _spawnedTiles;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Object dragonPrefab;
        [SerializeField] private Object foodPrefab;
        [SerializeField] private DragonColors dragonsSprite;

        public void DrawField() {
            var fieldContainer = FieldContainer.Instance;
            if (fieldContainer != null) {
                _width = fieldContainer.Size();
                _height = fieldContainer.Size();
            }

            _spawnedTiles = new Tile[_width, _height];

            for (var i = 0; i < _width; i++) {
                for (var j = 0; j < _height; j++) {
                    var isBase = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                    tilePrefab.Init(isBase);

                    var spawnedTile = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity);
                    _spawnedTiles[i, j] = spawnedTile;
                    spawnedTile.name = $"Tile {i} {j}";
                }
            }
        }

        public void RenderDragons() {
            RenderUnits<BotDragon>();
        }
        
        public void RenderFood() {
            RenderUnits<Chicken>();
        }

        private void RenderUnits<T>() where T : Component {
            var fieldContainer = FieldContainer.Instance;
            var units = fieldContainer.GetUnitsField<T>();
            var size = fieldContainer.Size();
            
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    if (units[i, j] == null) continue;

                    var currentTile = _spawnedTiles[i, j];
                    currentTile.IsOccupied = true;

                    var unitObject = Instantiate(typeof(T) == typeof(BotDragon) ? dragonPrefab : foodPrefab, new Vector3(i, j, 1f), Quaternion.identity) as GameObject;
                    var unitComponent = unitObject!.GetComponent<T>();

                    if (unitComponent != null) {
                        if (typeof(T) == typeof(BotDragon)) {
                            var dragonSprite = dragonsSprite.Get((units[i, j] as BotDragon)!.Color);
                            (unitComponent as BotDragon)?.Init(dragonSprite);
                        }

                        if (typeof(T) == typeof(Chicken)) {
                            // Initialize Chicken 
                        }

                        unitObject.transform.parent = currentTile.transform;
                        unitObject.name = $"{typeof(T).Name} {i} {j}";
                    }
                    else {
                        Debug.LogWarning($"The prefab doesn't have a {typeof(T).Name} component.");
                    }
                }
            }
        }
    }
}