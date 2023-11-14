using Scenes.Scripts.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Scripts.Field {
    public class FieldDrawer : MonoBehaviour {
        private int _width = 16, _height = 9;
        private Tile[,] _spawnedTiles;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Tile dragonPrefab;
        [SerializeField] private DragonColors dragonsSprite;

        public void DrawField(FieldContainer fieldContainer = null) {
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

        public void RenderDragons(FieldContainer fieldContainer) {
            var dragons = fieldContainer.GetDragonsField();
            var size = fieldContainer.Size();
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if (dragons[i, j] != null) {
                        var currentTile = _spawnedTiles[i, j];
                        currentTile.IsOccupied = true;
                        var dragonSprite = dragonsSprite.Get(dragons[i, j].Color);

                        // var spawnedDragon = new GameObject($"Dragon {i} {j}");
                        // var dragonRenderer = spawnedDragon.AddComponent<SpriteRenderer>();
                        // dragonRenderer.sprite = dragonSprite;
                        // spawnedDragon.transform.position = new Vector3(i, j, 1f);
                    }
                }
            }
        }
    }
}