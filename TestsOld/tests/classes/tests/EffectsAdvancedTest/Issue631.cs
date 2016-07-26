using CocosSharp;

namespace tests
{
    public class Issue631 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

			var effect = new CCSequence(new CCDelayTime (2.0f), new CCShaky3D(5.0f, new CCGridSize(5, 5), 16, false));

            // cleanup
			contentLayer.RemoveChild(bgNode, true);

            // background
            var layer = new CCDrawNode();
            layer.Color = CCColor3B.Red;
            layer.Opacity = 255;

            layer.DrawRect(VisibleBoundsWorldspace);

            AddChild(layer, -10);

			var sprite = new CCSprite("Images/grossini");
            sprite.Position = new CCPoint(50, 80);
            layer.AddChild(sprite, 10);

            // foreground
            var layer2BaseGrid = new CCNodeGrid ();
            var layer2 = new CCDrawNode();
            layer2.Color = CCColor3B.Green;
            layer2.Opacity = 255;
			
            layer2.DrawRect(VisibleBoundsWorldspace);

			var fog = new CCSprite("Images/Fog");

			var bf = new CCBlendFunc {Source = CCOGLES.GL_SRC_ALPHA, Destination = CCOGLES.GL_ONE_MINUS_SRC_ALPHA};
			fog.BlendFunc = bf;
			layer2.AddChild(fog, 1);
			AddChild(layer2BaseGrid, 1);
			layer2BaseGrid.AddChild (layer2);

			layer2BaseGrid.RepeatForever(effect);
        }

		public override string Title
		{
			get
			{
				return "Testing Opacity";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Effect image should be 100% opaque. Testing issue #631";
			}
		}
    }
}