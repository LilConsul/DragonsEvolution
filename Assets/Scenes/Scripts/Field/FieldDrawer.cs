using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scenes.Scripts.Field {
    public class FieldDrawer : MonoBehaviour {
        public static FieldDrawer Instance;

        private int _width = 16, _height = 9;
        private Tile[,] _spawnedTiles;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Object dragonPrefab;
        [SerializeField] private Object foodPrefab;
        [SerializeField] private DragonColors dragonsSprite;

        private void Awake() {
            Instance = this;
        }

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

        public void RenderUnits<T>() where T : Component {
            var fieldContainer = FieldContainer.Instance;
            var units = fieldContainer.GetUnitsField<T>();
            var size = fieldContainer.Size();

            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    if (units[i, j] == null) continue;

                    var currentTile = _spawnedTiles[i, j];
                    currentTile.IsOccupied = true;

                    var unitObject = Instantiate(typeof(T) == typeof(BotDragon) ? dragonPrefab : foodPrefab,
                        position: new Vector3(i, j, 1f), Quaternion.identity) as GameObject;
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

        public void UpdateUnits<T>(T unit) where T : Component {
            var fieldContainer = FieldContainer.Instance;
            if (unit is BotDragon botDragon) {
                var prevCords = botDragon.PrevCords();
                var newCords = botDragon.Cords();

                if (prevCords != newCords) {
                    var prevTile = _spawnedTiles[prevCords.x, prevCords.y];
                    var newTile = _spawnedTiles[newCords.x, newCords.y];

                    if(botDragon.PrevEaten() != null)
                        DestroyChild(newTile);
                    DestroyChild(prevTile);
                    InstantiateBotDragonOnTile(botDragon, newTile);
                }
            }
            //}
            else { }
        }

        private void DestroyChild(Tile tile) {
            var child = tile.transform.GetChild(0).gameObject;
            if (child != null) {
                Destroy(child);
                tile.IsOccupied = false;
            }
        }

        private void InstantiateBotDragonOnTile(BotDragon botDragon, Tile tile) {
            var newCords = new Vector3(botDragon.Cords().x, botDragon.Cords().y, 1f);
            var unitObject = Instantiate(dragonPrefab,
                new Vector3(newCords.x, newCords.y, 1f),
                Quaternion.identity) as GameObject;

            var unitComponent = unitObject?.GetComponent<BotDragon>();
            if (unitComponent != null) {
                var dragonSprite = dragonsSprite.Get(botDragon.Color);
                unitComponent.Init(dragonSprite);

                unitObject.transform.parent = tile.transform;
                unitObject.name = $"{typeof(BotDragon).Name} {newCords.x} {newCords.y}";
                tile.IsOccupied = true;
            }
        }
    }
}