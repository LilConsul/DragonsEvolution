using System;
using System.Linq;
using JetBrains.Annotations;
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
        [SerializeField] private Object bloodSprite;
        [SerializeField] private DragonColors dragonsSprite;

        private void Awake() {
            Instance = this;
        }

        public void DrawField() {
            var fieldContainer = FieldContainer.Instance;
            FoodFactory.Instance.OnFoodAdded += UpdateUnits;
            DragonFactory.Instance.OnDragonAdded += UpdateUnits;
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

                    switch (units[i, j]) {
                        case BotDragon:
                            InstantiateBotDragonOnTile(units[i, j] as BotDragon, currentTile);
                            break;
                        case Chicken:
                            InstantiateFoodOnTile(units[i, j] as Chicken, currentTile);
                            break;
                        default:
                            Debug.LogWarning($"The prefab doesn't have a {typeof(T).Name} component.");
                            break;
                    }
                }
            }
        }

        private Vector3 MakeVector(float i, float j) {
            return new Vector3(i, j, -1f);
        }

        public void UpdateUnits<T>(T unit) where T : Component {
            if (unit is BotDragon botDragon) {
                var prevCords = botDragon.PrevCords();
                var newCords = botDragon.Cords();

                var prevTile = _spawnedTiles[prevCords.x, prevCords.y];
                var newTile = _spawnedTiles[newCords.x, newCords.y];

                if (botDragon.PrevEaten() != null)
                    DestroyChild(newTile);
                DestroyChild(prevTile);
                InstantiateBotDragonOnTile(botDragon, newTile);
            }

            if (unit is Chicken chicken) {
                var cords = chicken.Cords();
                var tile = _spawnedTiles[cords.x, cords.y];
                InstantiateFoodOnTile(chicken, tile);
            }
        }

        private void DragonSubscription(BotDragon dragon) {
            dragon.OnTimeToLiveEnd += BotDragonOnOnTimeToLiveEnd;
            dragon.OnDeath += BotDragonOnDeath;
        }

        private void BotDragonOnOnTimeToLiveEnd(BotDragon sender) {
            var cords = sender.Cords();
            DestroyChild(_spawnedTiles[cords.x, cords.y]);
        }

        private void BotDragonOnDeath(BotDragon sender) {
            var cords = sender.Cords();
            DrawBlood(_spawnedTiles[cords.x, cords.y]);
        }

        private void DestroyChild([NotNull] Tile tile) {
            if (tile == null) throw new ArgumentNullException(nameof(tile));
            var tileTransform = tile.transform;
            foreach (Transform childTransform in tileTransform) {
                Destroy(childTransform.gameObject);
            }

            tile.IsOccupied = false;
        }

        private void DrawBlood([NotNull] Tile tile) {
            if (tile == null) throw new NullReferenceException($"The tile {tile.transform} is null");
            if (tile.transform.Cast<Transform>().Any(child => child.name.StartsWith("Blood Sprite"))) {
                return;
            }

            var position = tile.transform.position;
            var newCords = new Vector3(position.x, position.y, -2f);

            var unitObject = Instantiate(bloodSprite,
                position: newCords,
                Quaternion.identity) as GameObject;

            unitObject!.transform.parent = tile.transform;
            unitObject!.name = $"Blood Sprite {newCords.x} {newCords.y}";
        }

        private void InstantiateBotDragonOnTile(BotDragon botDragon, Tile tile) {
            var newCords = MakeVector(botDragon.Cords().x, botDragon.Cords().y);
            var unitObject = Instantiate(dragonPrefab,
                position: newCords,
                Quaternion.identity) as GameObject;

            var unitComponent = unitObject!.GetComponent<BotDragon>();
            if (unitComponent != null) {
                var dragonSprite = dragonsSprite.Get(botDragon.Color);
                unitComponent.Init(dragonSprite);
                DragonSubscription(botDragon);

                unitObject.transform.parent = tile.transform;
                unitObject.name = $"{typeof(BotDragon).Name} {newCords.x} {newCords.y}";
                tile.IsOccupied = true;
            }
        }

        private void InstantiateFoodOnTile(Chicken food, Tile tile) {
            var newCords = MakeVector(food.Cords().x, food.Cords().y);
            var unitObject = Instantiate(foodPrefab,
                position: newCords,
                Quaternion.identity) as GameObject;

            var unitComponent = unitObject!.GetComponent<Chicken>();
            if (unitComponent != null) {
                unitObject.transform.parent = tile.transform;
                unitObject.name = $"{typeof(Chicken).Name} {newCords.x} {newCords.y}";
                tile.IsOccupied = true;
            }
        }
    }
}