using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCLayerMultiplex : CCLayer
    {
        public const int NoLayer = -1;


        #region Properties

        public bool ShowFirstLayerOnEnter { get; set; }
        public int EnabledLayerIndex { get; private set; }

        public CCFiniteTimeAction InAction { get; set; }
        public CCFiniteTimeAction OutAction { get; set; }

        public CCLayer ActiveLayer
        {
            get { return Layers == null || EnabledLayerIndex == NoLayer ? null : Layers[EnabledLayerIndex]; }
        }

        public override CCScene Scene 
        { 
            get { return base.Scene; }
            internal set 
            {
                base.Scene = value;

                if (value != null && Layers != null)
                {
                    foreach (CCLayer layer in Layers) 
                        layer.Scene = value;
                }
            }
        }

        List<CCLayer> Layers { get; set; }

        #endregion Properties


        #region Constructors

        public CCLayerMultiplex (params CCLayer[] layers) 
            : this(null, null, layers)
        {
        }

        public CCLayerMultiplex(CCFiniteTimeAction inAction, CCFiniteTimeAction outAction) 
            : this(inAction, outAction, null)
        {
        }

        public CCLayerMultiplex(CCFiniteTimeAction inAction, CCFiniteTimeAction outAction, params CCLayer[] layers) 
            : base()
        {
            InAction = inAction;
            OutAction = outAction;
            ShowFirstLayerOnEnter = true;
            EnabledLayerIndex = NoLayer;
            Layers = new List<CCLayer>(layers);

            foreach(CCLayer layer in Layers)
            {
                AddChild(layer);
                layer.Visible = false;
            }
        }

        #endregion Constructors


        public override void OnEnter()
        {
            if (EnabledLayerIndex == NoLayer && Layers.Count > 0 && ShowFirstLayerOnEnter)
                SwitchTo(0);

            base.OnEnter();
        }


        #region Switching layers

        public CCLayer SwitchToFirstLayer()
        {
            return SwitchTo(0);
        }

        public void SwitchToNone() 
        {
            SwitchTo(NoLayer);
        }
            
        public CCLayer SwitchToNextLayer()
        {
            int index = NoLayer;
            int maxIndex = Layers.Count - 1;

            if(EnabledLayerIndex == NoLayer || EnabledLayerIndex == maxIndex)
                index = 0;
            else
                index = EnabledLayerIndex + 1;

            return SwitchTo(index);
        }

        public CCLayer SwitchToPreviousLayer()
        {
            int index = NoLayer;
            int maxIndex = Layers.Count - 1;

            if(EnabledLayerIndex == NoLayer || EnabledLayerIndex == 0)
                index = maxIndex;
            else
                index = EnabledLayerIndex - 1;

            return SwitchTo(index);
        }

        public CCLayer SwitchTo(int index)
        {
            if(index < NoLayer || index >= Layers.Count)
                return null;

            CCLayer outLayer = EnabledLayerIndex != NoLayer ? Layers[EnabledLayerIndex] : null;
            CCLayer inLayer = index != NoLayer ? Layers[index] : null;

            if(outLayer != null)
            {
                if(OutAction != null)
                {
                    outLayer.RunAction(new CCSequence(
                        OutAction,
                        new CCCallFunc(() => { outLayer.Visible = false; } )
                        )
                    );
                }
                else
                    outLayer.Visible = false;
            } 


            if(inLayer != null)
            {
                if(InAction != null)
                {
                    inLayer.RunAction(
                        new CCSequence(
                            InAction,
                            new CCCallFunc(() => { inLayer.Visible = true; } )
                        )
                    );
                }
                else
                    inLayer.Visible = true;
            }

            EnabledLayerIndex = index;

            if(EnabledLayerIndex != NoLayer)
                ShowFirstLayerOnEnter = false;

            return inLayer;

        }

        #endregion Switching
    }
}