using NUnit.Framework;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Tests.PlayMode {
    public class Testing {
        [Test]
        [Category("BotDragon")]
        public void DragonBasicSetting() {
            var game = new GameObject();
            var dragon = game.gameObject.AddComponent<BotDragon>();
            dragon.Initialization(5, 5);
            dragon.Speed = -10;
            dragon.Health = -10;
            dragon.Intellect = -10;

            Assert.IsTrue(dragon.Speed > 0);
            Assert.IsTrue(dragon.Health > 0);
            Assert.IsTrue(dragon.Intellect > 0);
            Object.DestroyImmediate(game);
        }


        [Test]
        [Category("BotDragon")]
        public void DragonCorrectCords() {
            var game = new GameObject();
            var dragon = game.gameObject.AddComponent<BotDragon>();
            dragon.Initialization(5, 5);
            Assert.AreEqual((5, 5), dragon.Cords());
            Object.DestroyImmediate(game);
        }

        [Test]
        [Category("BotDragon")]
        public void DragonTakeFoodOnMove() {
            var game = new GameObject();
            var dragon = game.gameObject.AddComponent<BotDragon>();
            game.gameObject.AddComponent<FieldContainer>();
            FieldContainer.Instance.SetSize(15);
            dragon.Initialization(5, 5);
            dragon.Health = 5;
            dragon.Speed = 5;

            dragon.Move(1, 0);
            Assert.AreEqual(5 - 1 / dragon.Speed, dragon.Health);

            Object.DestroyImmediate(game);
        }

        [Test]
        [Category("BotDragon")]
        public void DragonDiesWhenHealthDepleted() {
            var game = new GameObject();
            var dragon = game.gameObject.AddComponent<BotDragon>();
            game.gameObject.AddComponent<FieldContainer>();
            FieldContainer.Instance.SetSize(50);
            dragon.Initialization(5, 5);
            dragon.Speed = 1;

            for (var i = 0; i < 15; i++) {
                dragon.Move(1, 0); // Health will become negative, causing the dragon to die
            }

            Assert.AreEqual(EntityState.Dead, dragon.State);
            Object.DestroyImmediate(game);
        }

        [Test]
        [Category("BotDragon")]
        public void DragonDespawnsAfterTimeToLiveEnds() {
            var game = new GameObject();
            var dragon = game.gameObject.AddComponent<BotDragon>();
            dragon.Initialization(5, 5);
            dragon.TimeToLive = 0;
            dragon.State = EntityState.Dead;

            var endCycle = false;
            dragon.OnTimeToLiveEnd += (sender) => { endCycle = true; };

            dragon.Move(0, 0);
            Assert.IsTrue(endCycle);
            Object.DestroyImmediate(game);
        }

        [Test]
        [Category("BotDragon")]
        public void DragonMateEventTriggered() {
            var game = new GameObject();
            var dragon = game.gameObject.AddComponent<BotDragon>();
            dragon.Initialization(5, 5);
            var mateX = 6;
            var mateY = 7;

            BotDragon mate = null;
            dragon.OnMate += (sender, x, y) => {
                mate = sender;
                mate.Initialization(x, y);
            };

            dragon.Mate(mateX - dragon.Cords().x, mateY - dragon.Cords().y);

            Assert.IsNotNull(mate);
            Assert.AreEqual((mateX, mateY), mate.Cords());
            Object.DestroyImmediate(game);
        }
        
        [Test]
        [Category("AI")]
        public void AIReturnMoveToUp() {
            var game = new GameObject();
            game.gameObject.AddComponent<FieldContainer>();
            game.gameObject.AddComponent<FieldGenerator>();
            FieldContainer.Instance.SetSize(5);
            FieldGenerator.Instance.GeneratePreset(0, 1);
            
            var ai = game.gameObject.AddComponent<AI>();
            
            var dragon = FieldContainer.Instance.GetNextDragon();
            var nextMove = ai.GetNextMove(dragon);
            
            Assert.AreEqual(nextMove.x, 0);
            Assert.AreEqual(nextMove.y, 1);
            Object.DestroyImmediate(game);
        }
        
        [Test]
        [Category("AI")]
        public void AIReturnMoveToDown() {
            var game = new GameObject();
            game.gameObject.AddComponent<FieldContainer>();
            game.gameObject.AddComponent<FieldGenerator>();
            FieldContainer.Instance.SetSize(5);
            FieldGenerator.Instance.GeneratePreset(0, -1);
            
            var ai = game.gameObject.AddComponent<AI>();
            
            var dragon = FieldContainer.Instance.GetNextDragon();
            var nextMove = ai.GetNextMove(dragon);
            
            Assert.AreEqual(nextMove.x, 0);
            Assert.AreEqual(nextMove.y, -1);
            Object.DestroyImmediate(game);
        }
        
        
        [Test]
        [Category("AI")]
        public void AIReturnMoveToLeft() {
            var game = new GameObject();
            game.gameObject.AddComponent<FieldContainer>();
            game.gameObject.AddComponent<FieldGenerator>();
            FieldContainer.Instance.SetSize(5);
            FieldGenerator.Instance.GeneratePreset(1, 0);
            
            var ai = game.gameObject.AddComponent<AI>();
            
            var dragon = FieldContainer.Instance.GetNextDragon();
            var nextMove = ai.GetNextMove(dragon);
            
            Assert.AreEqual(nextMove.x, 1);
            Assert.AreEqual(nextMove.y, 0);
            Object.DestroyImmediate(game);
        }
        
        
        [Test]
        [Category("AI")]
        public void AIReturnMoveToRight() {
            var game = new GameObject();
            game.gameObject.AddComponent<FieldContainer>();
            game.gameObject.AddComponent<FieldGenerator>();
            FieldContainer.Instance.SetSize(5);
            FieldGenerator.Instance.GeneratePreset(-1, 0);
            
            var ai = game.gameObject.AddComponent<AI>();
            
            var dragon = FieldContainer.Instance.GetNextDragon();
            var nextMove = ai.GetNextMove(dragon);
            
            Assert.AreEqual(nextMove.x, -1);
            Assert.AreEqual(nextMove.y, 0);
            Object.DestroyImmediate(game);
        }
    }
}