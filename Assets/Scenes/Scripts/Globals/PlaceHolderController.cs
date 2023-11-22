using Scenes.Scripts.Units;
using UnityEngine;
using TMPro;

namespace Scenes.Scripts.Globals {
    public class PlaceHolderController : MonoBehaviour {
        public static PlaceHolderController Instance;
        [SerializeField] private TMP_Text statHolder;
        
        private void Awake() {
            Instance = this;
        }

        public void ShowInfo(BotDragon dragon) {
            statHolder.text = $"Dragon on {dragon.Cords()} \n" +
                              $"Color: {dragon.Color}\n" +
                              $"Health: {dragon.Health}\n" +
                              $"Speed: {dragon.Speed}\n" +
                              $"Intellect: {dragon.Intellect}";
        }
    }
}
