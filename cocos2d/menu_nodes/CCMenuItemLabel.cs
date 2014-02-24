using System;

namespace CocosSharp
{
    public abstract class CCMenuItemLabelBase : CCMenuItem
    {
        #region Properties

        public CCColor3B DisabledColor { get; set; }
        protected CCColor3B ColorBackup { get; set; }
        protected float OriginalScale { get; set; }

        public override bool Selected
        {
            set 
            {
                if(Enabled) 
                {
                    base.Selected = value;

                    float zoomScale = (Selected == true) ? OriginalScale * 1.2f : OriginalScale;
                    CCAction zoomAction = new CCScaleTo(0.1f, zoomScale); 

                    if (Selected) 
                    {
                        OriginalScale = Scale;
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
            OriginalScale = 1.0f;
            ColorBackup = CCTypes.CCWhite;
            DisabledColor = new CCColor3B(126, 126, 126);
            CascadeColorEnabled = true;
            CascadeOpacityEnabled = true;
        }

        #endregion Constructors


        protected void LabelWillChange(CCNode oldValue, CCNode newValue)
        {
            if(newValue != null)
            {
                AddChild(newValue);
                newValue.AnchorPoint = new CCPoint(0, 0);
                ContentSize = newValue.ContentSize;
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
                Scale = OriginalScale;
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


    public class CCMenuItemLabelTTF : CCMenuItemLabelBase
    {
        CCLabelTTF labelTTF;


        #region Properties

        public CCLabelTTF LabelTTF
        {
            get { return labelTTF; }
            set
            {
                LabelWillChange(labelTTF, value);
                labelTTF = value;
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

        #endregion Properties


        #region Constructors

        public CCMenuItemLabelTTF(CCLabelTTF labelTTF, Action<object> target = null) : base(target)
        {
            LabelTTF = labelTTF;
        }

        #endregion Constructors
    }


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

        #endregion Properties


        #region Constructors

        public CCMenuItemLabelBMFont(CCLabelBMFont labelBMFont, Action<object> target = null) : base(target)
        {
            LabelBMFont = labelBMFont;
        }

        #endregion Constructors
    }
}