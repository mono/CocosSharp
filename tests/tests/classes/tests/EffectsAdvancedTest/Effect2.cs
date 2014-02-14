using CocosSharp;

namespace tests
{
    public class Effect2 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            // To reuse a grid the grid size and the grid type must be the same.
            // in this case:
            //     ShakyTiles is TiledGrid3D and it's size is (15,10)
            //     Shuffletiles is TiledGrid3D and it's size is (15,10)
            //	   TurnOfftiles is TiledGrid3D and it's size is (15,10)
			var shaky = new CCShakyTiles3D(5, new CCGridSize(15, 10), 4, false);
			var shuffle = new CCShuffleTiles(new CCGridSize(15, 10), 3, 0);
			var turnoff = new CCTurnOffTiles(3, new CCGridSize(15, 10), 0);
			var turnon = turnoff.Reverse();

            // reuse 2 times:
            //   1 for shuffle
            //   2 for turn off
            //   turnon tiles will use a new grid
			var reuse = new CCReuseGrid(2);

			var delay = new CCDelayTime (1);

            //	id orbit = [OrbitCamera::actionWithDuration:5 radius:1 deltaRadius:2 angleZ:0 deltaAngleZ:180 angleX:0 deltaAngleX:-90];
            //	id orbit_back = [orbit reverse];
            //
            //	[target runAction: [RepeatForever::actionWithAction: [Sequence actions: orbit, orbit_back, nil]]];
			_bgNode.RunActions (shaky, delay, reuse, shuffle, delay, turnoff, turnon);
        }

        public override string title()
        {
            return "ShakyTiles + ShuffleTiles + TurnOffTiles";
        }
    }
}