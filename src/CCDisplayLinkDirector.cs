using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCDisplayLinkDirector : CCDirector
    {
        private bool isInvalid;

        public override double AnimationInterval
        {
            get { return base.AnimationInterval; }
            set
            {
                base.AnimationInterval = value;

                if (!isInvalid)
                {
                    StopAnimation();
                    StartAnimation();
                }
            }
        }

        public override void StopAnimation()
        {
            isInvalid = true;
        }

        public override void StartAnimation()
        {
            isInvalid = false;
            CCApplication.SharedApplication.AnimationInterval = AnimationInterval;
        }

        public override void MainLoop(CCGameTime gameTime)
        {
            if (IsPurgeDirectorInNextLoop)
            {
                PurgeDirector();
                IsPurgeDirectorInNextLoop = false;
            }
            else if (!isInvalid)
            {
                DrawScene(gameTime);
            }
        }
    }
}