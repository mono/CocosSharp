using Cocos2D;

namespace tests
{
    internal class LabelTTFA8Test : AtlasDemo
    {
        public LabelTTFA8Test()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLayerColor layer = new CCLayerColor(new CCColor4B(128, 128, 128, 255));
            AddChild(layer, -10);

            // CCLabelBMFont
            CCLabelTTF label1 = new CCLabelTTF("Testing A8 Format", "Marker Felt", 38);
            AddChild(label1);
            label1.Color = CCTypes.CCRed;
            label1.Position = new CCPoint(s.Width / 2, s.Height / 2);

            CCFadeOut fadeOut = new CCFadeOut  (2);
            CCFadeIn fadeIn = new CCFadeIn  (2);
            CCFiniteTimeAction seq = CCSequence.FromActions(fadeOut, fadeIn);
            CCRepeatForever forever = new CCRepeatForever ((CCActionInterval) seq);
            label1.RunAction(forever);
        }

        public override string title()
        {
            return "Testing A8 Format";
        }

        public override string subtitle()
        {
            return "RED label, fading In and Out in the center of the screen";
        }
    }
}