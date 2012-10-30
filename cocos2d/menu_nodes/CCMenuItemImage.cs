
namespace cocos2d
{
    public class CCMenuItemImage : CCMenuItemSprite
    {
        public new static CCMenuItemImage Create()
        {
            var ret = new CCMenuItemImage();
            ret.Init();
            return ret;
        }

        public bool Init()
        {
            return InitWithNormalImage(null, null, null, null);
        }

        public static CCMenuItemImage Create(string normalImage, string selectedImage)
        {
            return Create(normalImage, selectedImage, null, null);
        }

        public new static CCMenuItemImage Create(string normalImage, string selectedImage, SEL_MenuHandler selector)
        {
            return Create(normalImage, selectedImage, null, selector);
        }

        public static CCMenuItemImage Create(string normalImage, string selectedImage, string disabledImage, SEL_MenuHandler selector)
        {
            var pRet = new CCMenuItemImage();
            pRet.InitWithNormalImage(normalImage, selectedImage, disabledImage, selector);
            return pRet;
        }

        public static CCMenuItemImage Create(string normalImage, string selectedImage, string disabledImage)
        {
            var pRet = new CCMenuItemImage();
            pRet.InitWithNormalImage(normalImage, selectedImage, disabledImage, null);
            return pRet;
        }

        private bool InitWithNormalImage(string normalImage, string selectedImage, string disabledImage, SEL_MenuHandler selector)
        {
            CCNode normalSprite = null;
            CCNode selectedSprite = null;
            CCNode disabledSprite = null;

            if (!string.IsNullOrEmpty(normalImage))
            {
                normalSprite = CCSprite.Create(normalImage);
            }

            if (!string.IsNullOrEmpty(selectedImage))
            {
                selectedSprite = CCSprite.Create(selectedImage);
            }

            if (!string.IsNullOrEmpty(disabledImage))
            {
                disabledSprite = CCSprite.Create(disabledImage);
            }

            return InitFromNormalSprite(normalSprite, selectedSprite, disabledSprite, selector);
        }

        public void SetNormalSpriteFrame(CCSpriteFrame frame)
        {
            NormalImage = CCSprite.Create(frame);
        }

        public void SetSelectedSpriteFrame(CCSpriteFrame frame)
        {
            SelectedImage = CCSprite.Create(frame);
        }

        public void SetDisabledSpriteFrame(CCSpriteFrame frame)
        {
            DisabledImage = CCSprite.Create(frame);
        }
    }
}