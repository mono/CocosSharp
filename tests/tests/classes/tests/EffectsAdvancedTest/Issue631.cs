using cocos2d;

namespace tests
{
    public class Issue631 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCActionInterval effect = (CCSequence.FromActions(new CCDelayTime (2.0f), CCShaky3D.Create(16, false, new CCGridSize(5, 5), 5.0f)));

            // cleanup
            CCNode bg = GetChildByTag(EffectAdvanceScene.kTagBackground);
            RemoveChild(bg, true);

            // background
            CCLayerColor layer = new CCLayerColor(new CCColor4B(255, 0, 0, 255));
            AddChild(layer, -10);
            CCSprite sprite = new CCSprite("Images/grossini");
            sprite.Position = new CCPoint(50, 80);
            layer.AddChild(sprite, 10);

            // foreground
            CCLayerColor layer2 = new CCLayerColor(new CCColor4B(0, 255, 0, 255));
            CCSprite fog = new CCSprite("Images/Fog");

            var bf = new CCBlendFunc {Source = OGLES.GL_SRC_ALPHA, Destination = OGLES.GL_ONE_MINUS_SRC_ALPHA};
            fog.BlendFunc = bf;
            layer2.AddChild(fog, 1);
            AddChild(layer2, 1);

            layer2.RunAction(new CCRepeatForever (effect));
        }

        public override string title()
        {
            return "Testing Opacity";
        }

        public override string subtitle()
        {
            return "Effect image should be 100% opaque. Testing issue #631";
        }
    }
}