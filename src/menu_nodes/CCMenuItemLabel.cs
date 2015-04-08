using System;

namespace CocosSharp
{
    public abstract class CCMenuItemLabelBase : CCMenuItem
    {
        CCPoint originalScale;


        #region Properties

        public CCColor3B DisabledColor { get; set; }
        protected CCColor3B ColorBackup { get; set; }

        public override bool Selected
        {
            set 
            {
                if(Enabled) 
                {
                    base.Selected = value;

                    CCPoint zoomScale = (Selected == true) ? originalScale * 1.2f : originalScale;
                    CCAction zoomAction = new CCScaleTo(0.1f, zoomScale.X, zoomScale.Y); 

                    if(Selected && (ZoomActionState == null || ZoomActionState.IsDone)) 
                    {
                        originalScale.X = ScaleX;
                        originalScale.Y = ScaleY;
                    }

                    if(ZoomActionState !=null)
                    { 
                        ZoomActionState.Stop(); 
                    }

                    ZoomActionState = RunAction(zoomAction);
                }   
            }
        }

        #endregion Properties


        #region Constructors

        protected CCMenuItemLabelBase(Action<object> target = null) : base(target)
        {
            originalScale = new CCPoint(1.0f, 1.0f);
            ColorBackup = CCColor3B.White;
            DisabledColor = new CCColor3B(126, 126, 126);
            IsColorCascaded = true;
            IsOpacityCascaded = true;
        }

        #endregion Constructors


        protected void LabelWillChange(CCNode oldValue, CCNode newValue)
        {
            if(newValue != null)
            {
                AddChild(newValue);
                newValue.AnchorPoint = CCPoint.AnchorLowerLeft;
                ContentSize = newValue.ScaledContentSize;
            }

            if(oldValue != null)
            {
                RemoveChild(oldValue, true);
            }
        }

        public override void Activate()
        {
            if (Enabled)
            {
                StopAllActions();
                ScaleX = originalScale.X;
                ScaleY = originalScale.Y;
                base.Activate();
            }
        }
    }


    public class CCMenuItemLabel : CCMenuItemLabelBase
    {
        CCLabel label;


        #region Properties

        public CCLabel Label
        {
            get { return label; }
            set
            {
                LabelWillChange(label, value);
                label = value;

                if(label !=null && Scene != null)
                    label.Scene = Scene;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if(base.Enabled != value && label != null)
                {
                    if (!value)
                    {
                        ColorBackup = label.Color;
                        label.Color = DisabledColor;
                    }
                    else
                    {
                        label.Color = ColorBackup;
                    }
                }
                base.Enabled = value;
            }
        }

        public override CCScene Scene 
        { 
            get { return base.Scene; }
            internal set 
            {
                base.Scene = value;

                if (value != null && Label != null)
                {
                    Label.Scene = value;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCMenuItemLabel(CCLabel label, Action<object> target = null) : base(target)
        {
            Label = label;
        }

        #endregion Constructors

    }

    public class CCMenuItemLabelAtlas : CCMenuItemLabelBase
    {
        CCLabelAtlas labelAtlas;


        #region Properties

        public CCLabelAtlas LabelAtlas
        {
            get { return labelAtlas; }
            set
            {
                LabelWillChange(labelAtlas, value);
                labelAtlas = value;

                if(labelAtlas != null && Scene != null)
                    labelAtlas.Scene = Scene;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if(base.Enabled != value && labelAtlas != null)
                {
                    if (!value)
                    {
                        ColorBackup = labelAtlas.Color;
                        labelAtlas.Color = DisabledColor;
                    }
                    else
                    {
                        labelAtlas.Color = ColorBackup;
                    }
                }
                base.Enabled = value;
            }
        }

        public override CCScene Scene 
        { 
            get { return base.Scene; }
            internal set 
            {
                base.Scene = value;

                if (value != null && LabelAtlas != null)
                {
                    LabelAtlas.Scene = value;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCMenuItemLabelAtlas(CCLabelAtlas labelAtlas, Action<object> target = null) : base(target)
        {
            LabelAtlas = labelAtlas;
        }

        public CCMenuItemLabelAtlas(Action<object> target) : this(null, target)
        {
        }

        public CCMenuItemLabelAtlas(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap, 
            ICCUpdatable updatable, Action<object> target) 
            : this(new CCLabelAtlas(value, charMapFile, itemWidth, itemHeight, startCharMap), target)
        {
        }

        public CCMenuItemLabelAtlas(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap) 
            : this(value, charMapFile, itemWidth, itemHeight, startCharMap, null, null)
        {
        }

        #endregion Constructors
    }

    [Obsolete("Use CCMenuItemLabel instead.")]
    public class CCMenuItemLabelTTF : CCMenuItemLabelBase
    {
        CCLabelTtf labelTTF;


        #region Properties

        public CCLabelTtf LabelTTF
        {
            get { return labelTTF; }
            set
            {
                LabelWillChange(labelTTF, value);
                labelTTF = value;

                if(labelTTF != null && Scene != null)
                    labelTTF.Scene = Scene;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if(base.Enabled != value && labelTTF != null)
                {
                    if (!value)
                    {
                        ColorBackup = labelTTF.Color;
                        labelTTF.Color = DisabledColor;
                    }
                    else
                    {
                        labelTTF.Color = ColorBackup;
                    }
                }
                base.Enabled = value;
            }
        }

        public override CCScene Scene 
        { 
            get { return base.Scene; }
            internal set 
            {
                base.Scene = value;

                if (value != null && LabelTTF != null)
                {
                    LabelTTF.Scene = value;
                }
            }
        }

        #endregion Properties


        #region Constructors
        [Obsolete("Use CCMenuItemLabel instead.")]
        public CCMenuItemLabelTTF(CCLabelTtf labelTTF, Action<object> target = null) : base(target)
        {
            LabelTTF = labelTTF;
        }
        [Obsolete("Use CCMenuItemLabel instead.")]
        public CCMenuItemLabelTTF(Action<object> target = null) : base(target)
        {

        }

        #endregion Constructors

    }

    [Obsolete("Use CCMenuItemLabel instead.")]
    public class CCMenuItemLabelBMFont : CCMenuItemLabelBase
    {
        CCLabelBMFont labelBMFont;


        #region Properties

        public CCLabelBMFont LabelBMFont
        {
            get { return labelBMFont; }
            set
            {
                LabelWillChange(labelBMFont, value);
                labelBMFont = value;

                if(labelBMFont != null && Scene != null)
                    labelBMFont.Scene = Scene;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if(base.Enabled != value && labelBMFont != null)
                {
                    if (!value)
                    {
                        ColorBackup = labelBMFont.Color;
                        labelBMFont.Color = DisabledColor;
                    }
                    else
                    {
                        labelBMFont.Color = ColorBackup;
                    }
                }
                base.Enabled = value;
            }
        }

        public override CCScene Scene 
        { 
            get { return base.Scene; }
            internal set 
            {
                base.Scene = value;

                if (value != null && LabelBMFont != null)
                {
                    LabelBMFont.Scene = value;
                }
            }
        }

        #endregion Properties


        #region Constructors
        [Obsolete("Use CCMenuItemLabel instead.")]
        public CCMenuItemLabelBMFont(CCLabelBMFont labelBMFont, Action<object> target = null) : base(target)
        {
            LabelBMFont = labelBMFont;
        }

        #endregion Constructors

    }
}