using Scenes.Scripts.Field;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scenes.Scripts.Globals {
    public class ButtonsController : MonoBehaviour {
        [SerializeField] private Toggle newChickens;
        [SerializeField] private Toggle newDragons;

        [SerializeField] private Button spawnDragon;
        [SerializeField] private Button spawnChicken;
        [SerializeField] private Button restartGame;

        [SerializeField] private TMP_InputField health;
        [SerializeField] private TMP_InputField speed;
        [SerializeField] private TMP_InputField intellect;

        [SerializeField] private TMP_InputField minChicken;
        [SerializeField] private TMP_InputField maxChicken;

        [SerializeField] private TMP_InputField fieldSize;


        public void Start() {
            newChickens.onValueChanged.AddListener(delegate(bool arg0) {
                GlobalSettings.Instance.spawnNewChickens = arg0;
            });

            newDragons.onValueChanged.AddListener(delegate(bool arg0) {
                GlobalSettings.Instance.spawnNewDragons = arg0;
            });

            spawnDragon.onClick.AddListener(delegate { DragonFactory.Instance.SpawnDragons(); });

            spawnChicken.onClick.AddListener(delegate { FoodFactory.Instance.SpawnFood(); });
            
            //restartGame.onClick.AddListener(delegate { GameUI.Instance.RestartGame(); });

            health.onValueChanged.AddListener(delegate(string newValue) {
                if (!int.TryParse(newValue, out var healthValue) || healthValue < 3) {
                    health.text = "3";
                    GlobalSettings.Instance.basicHealth = 3;
                }
                else {
                    GlobalSettings.Instance.basicHealth = healthValue;
                }
            });

            speed.onValueChanged.AddListener(delegate(string newValue) {
                if (!int.TryParse(newValue, out var speedValue) || speedValue < 1) {
                    speed.text = "1";
                    GlobalSettings.Instance.basicSpeed = 1;
                }
                else {
                    GlobalSettings.Instance.basicSpeed = speedValue;
                }
            });

            intellect.onValueChanged.AddListener(delegate(string newValue) {
                if (!int.TryParse(newValue, out var intellectValue) || intellectValue < 3) {
                    intellect.text = "3";
                    GlobalSettings.Instance.basicIntellect = 3;
                }
                else {
                    GlobalSettings.Instance.basicIntellect = intellectValue;
                }
            });


            minChicken.onValueChanged.AddListener(delegate(string newValue) {
                if (!int.TryParse(newValue, out var minFood) || minFood < 3 || minFood > GlobalSettings.Instance.maxChicken) {
                    minChicken.text = "";
                    GlobalSettings.Instance.minChicken = 3;
                }
                else {
                    GlobalSettings.Instance.minChicken = minFood;
                }
            });

            maxChicken.onValueChanged.AddListener(delegate(string newValue) {
                if (!int.TryParse(newValue, out var maxFood) || maxFood > 1000 || maxFood < GlobalSettings.Instance.minChicken) {
                    maxChicken.text = $"{GlobalSettings.Instance.minChicken + 10}";
                    GlobalSettings.Instance.maxChicken = GlobalSettings.Instance.minChicken + 10;
                }
                else {
                    GlobalSettings.Instance.maxChicken = maxFood;
                }
            });
            
            fieldSize.onValueChanged.AddListener(delegate(string newValue) {
                if (!uint.TryParse(newValue, out var size) || size < 9 || size > 100) {
                    fieldSize.text = "9";
                    GlobalSettings.Instance.fieldSize = 9;
                }
                else {
                    GlobalSettings.Instance.fieldSize = size;
                }
            });
            
        }
    }
}