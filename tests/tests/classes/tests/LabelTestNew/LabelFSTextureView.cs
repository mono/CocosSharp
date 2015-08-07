using System;
using CocosSharp;

namespace tests
{
    public class LabelSFTextureView : AtlasDemoNew
    {
        CCNode spriteFontNode;

        public LabelSFTextureView()
        {
            Color = new CCColor3B(128, 128, 128);
            Opacity = 255;


        }

        public override void OnEnter ()
        {
            base.OnEnter ();

            var origin = Layer.VisibleBoundsWorldspace.Size;

            var label1 = new CCLabel(string.Empty, "debuguncompressed", 0, CCLabelFormat.SpriteFont);

            var texture = label1.TextureAtlas != null ? label1.TextureAtlas.Texture : null;

            if (texture != null) {
                spriteFontNode = new CCSprite (texture);
                spriteFontNode.Scale = 2;
            }
            else
            {
                spriteFontNode = new CCLabel("Texture can not be loaded", "arial", 24, CCLabelFormat.SpriteFont);
            }
            //spriteFontNode.Color = CCColor3B.Magenta;
            spriteFontNode.Position = origin.Center;


            AddChild (spriteFontNode);

            var itemUncompressed = new CCMenuItemLabel(new CCLabel("Uncompressed", "fonts/arial", 24, CCLabelFormat.SpriteFont));
            var itemCompressed = new CCMenuItemLabel(new CCLabel("Compressed", "fonts/arial", 24, CCLabelFormat.SpriteFont));
            itemUncompressed.AnchorPoint = CCPoint.AnchorMiddleLeft;
            itemCompressed.AnchorPoint = CCPoint.AnchorMiddleLeft;
            var mi1 = new CCMenuItemToggle(OnToggle, itemUncompressed, itemCompressed);
            var menu = new CCMenu(mi1);

            AddChild(menu);
            menu.Position = VisibleBoundsWorldspace.Left();
        }

        void OnToggle(object sender)
        {
            var toggle = sender as CCMenuItemToggle;
            if (toggle == null)
                return;

            var origin = Layer.VisibleBoundsWorldspace.Size;
            spriteFontNode.RemoveFromParent (true);

            switch(toggle.SelectedIndex)
            {
            case 0:
                var label1 = new CCLabel (string.Empty, "debuguncompressed", 0, CCLabelFormat.SpriteFont);

                var texture = label1.TextureAtlas != null ? label1.TextureAtlas.Texture : null;

                if (texture != null) {
                    spriteFontNode.RemoveFromParent (true);
                    spriteFontNode = new CCSprite (texture);
                    spriteFontNode.Scale = 2;
                } else {
                    spriteFontNode = new CCLabel ("Texture can not be loaded", "arial", 24, CCLabelFormat.SpriteFont);
                }
                break;
            case 1:
                label1 = new CCLabel (string.Empty, "debugcompressed", 0, CCLabelFormat.SpriteFont);

                texture = label1.TextureAtlas != null ? label1.TextureAtlas.Texture : null;

                if (texture != null) {
                    spriteFontNode.RemoveFromParent (true);
                    spriteFontNode = new CCSprite (texture);
                    spriteFontNode.Scale = 2;
                } else {
                    spriteFontNode = new CCLabel ("Texture can not be loaded", "arial", 24, CCLabelFormat.SpriteFont);
                }
                break;

            }

            AddChild (spriteFontNode);
            //spriteFontNode.Color = CCColor3B.Magenta;
            spriteFontNode.Position = origin.Center;


        }

        public override string Title
        {
            get {
                return "SpriteFont TextureAtlas";
            }
        }

        public override string Subtitle
        {
            get {
                return string.Empty;
            }
        }
    }
}

