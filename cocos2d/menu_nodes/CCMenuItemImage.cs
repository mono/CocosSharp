
namespace cocos2d
{
    public class CCMenuItemImage : CCMenuItemSprite
    {
        public CCMenuItemImage() : this(null, null, null, null)
        {
        }

        public CCMenuItemImage(string normalImage, string selectedImage)
            :this(normalImage, selectedImage, null, null)
        {
        }

        public CCMenuItemImage(string normalImage, string selectedImage, SEL_MenuHandler selector)
            :this(normalImage, selectedImage, null, selector)
        {
        }

        public CCMenuItemImage(string normalImage, string selectedImage, string disabledImage, SEL_MenuHandler selector)
            : base(selector)
        {

            if (!string.IsNullOrEmpty(normalImage))
            {
                NormalImage = new CCSprite(normalImage);
            }

            if (!string.IsNullOrEmpty(selectedImage))
            {
                SelectedImage = new CCSprite(selectedImage);
            }

            if (!string.IsNullOrEmpty(disabledImage))
            {
                DisabledImage = new CCSprite(disabledImage);
            }
        }

        public CCMenuItemImage(string normalImage, string selectedImage, string disabledImage)
            : this(normalImage, selectedImage, disabledImage, null)
        {
        }

        public void SetNormalSpriteFrame(CCSpriteFrame frame)
        {
            NormalImage = new CCSprite(frame);
        }

        public void SetSelectedSpriteFrame(CCSpriteFrame frame)
        {
            SelectedImage = new CCSprite(frame);
        }

        public void SetDisabledSpriteFrame(CCSpriteFrame frame)
        {
            DisabledImage = new CCSprite(frame);
        }
    }
}