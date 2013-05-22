using Cocos2D;

namespace tests
{
    public class Effect2 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCNode target = GetChildByTag(EffectAdvanceScene.kTagBackground);

            // To reuse a grid the grid size and the grid type must be the same.
            // in this case:
            //     ShakyTiles is TiledGrid3D and it's size is (15,10)
            //     Shuffletiles is TiledGrid3D and it's size is (15,10)
            //	   TurnOfftiles is TiledGrid3D and it's size is (15,10)
            CCActionInterval shaky = new CCShakyTiles3D(4, false, new CCGridSize(15, 10), 5);
            CCActionInterval shuffle = new CCShuffleTiles(0, new CCGridSize(15, 10), 3);
            CCActionInterval turnoff = new CCTurnOffTiles(0, new CCGridSize(15, 10), 3);
            CCFiniteTimeAction turnon = turnoff.Reverse();

            // reuse 2 times:
            //   1 for shuffle
            //   2 for turn off
            //   turnon tiles will use a new grid
            CCFiniteTimeAction reuse = new CCReuseGrid(2);

            CCActionInterval delay = new CCDelayTime (1);

            //	id orbit = [OrbitCamera::actionWithDuration:5 radius:1 deltaRadius:2 angleZ:0 deltaAngleZ:180 angleX:0 deltaAngleX:-90];
            //	id orbit_back = [orbit reverse];
            //
            //	[target runAction: [RepeatForever::actionWithAction: [Sequence actions: orbit, orbit_back, nil]]];
            target.RunAction((CCSequence.FromActions(shaky, delay, reuse, shuffle, (CCFiniteTimeAction) delay.Copy(), turnoff, turnon)));
        }

        public override string title()
        {
            return "ShakyTiles + ShuffleTiles + TurnOffTiles";
        }
    }
}