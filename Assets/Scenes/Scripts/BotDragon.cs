using System;

namespace Scenes.Scripts {
    public class BotDragon : IDragon {
        public BotDragon(int x, int y) {

            _x = x;
            _y = y;
        }
        
        public override void Move(int newX, int newY) {
            //TODO: Check if it can move 
            _x = newX;
            _y = newY;
            throw new System.NotImplementedException();
        }

        public override void Eat(IFood food) {
            if (food == null) 
                return;
            //TODO: Eat food;
            throw new NotImplementedException();
        }

        public override (int x, int y) Cords() {
            return (_x, _y);
        }

        public override void PerformDecision() {
            throw new NotImplementedException();
        }

        public override EntityState GetState() {
            return _state;
        }
    }
}