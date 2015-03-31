using System;
using System.Collections.Generic;
using System.Text;

namespace CocosSharp
{
    internal struct CCTLLine
    {

        private List<LetterInfo> glyphRun;
        private CCSize bounds;

        internal static CCTLLine NewCTLLine()
        {
            var newLine = new CCTLLine();
            newLine.glyphRun = new List<LetterInfo>();
            newLine.bounds = CCSize.Zero;
            return newLine;
        }

        internal bool AddGlyph(CCPoint point, CCFontLetterDefinition letterDef, int spriteIndex)
        {

            var letterInfo = new LetterInfo();

            letterInfo.Definition = letterDef;
            letterInfo.Position = point;
            letterInfo.ContentSize.Width = letterDef.Width;
            letterInfo.ContentSize.Height = letterDef.Height;
            bounds.Width = letterInfo.Position.X + letterDef.XAdvance + letterDef.Kerning;
            if (bounds.Height < letterDef.Height)
                bounds.Height = letterDef.Height;

            letterInfo.AtlasIndex = spriteIndex;
            glyphRun.Add(letterInfo);

            return letterInfo.Definition.IsValidDefinition;
        }

        public List<LetterInfo> GlyphRun
        {
            get { return glyphRun; }
        }

        public CCSize Bounds
        {
            get { return bounds; }
        }

        public float PenOffsetForFlush (float flushFactor, float flushWidth)
        {

            var ff = 1.0f * CCMathHelper.Clamp(flushFactor, 0, 1);
            return (flushWidth * flushFactor) - (bounds.Width * ff);
        }
    }


    internal class CCTLTextLayout //: IDisposable
    {
        CCLabel label;
        CCFontAtlas fontAtlas;
        CCFont font;
        string text;
        List<LineBreakElement> lineBreakElements = new List<LineBreakElement>();
        int[] kernings = null;

        public CCTLTextLayout(CCLabel label)
        {
            this.label = label;
            this.fontAtlas = label.FontAtlas;
            this.font = label.FontAtlas.Font;
            this.text = label.Text;

            // Obtain the kernings for the text.
            ComputeHorizontalKernings(this.text);

            // Flag the possible line breaks base on the current label text.
            LineBreak(this.text);
        }

        bool ComputeHorizontalKernings(string stringToRender)
        {

            int letterCount = 0;
            kernings = font.HorizontalKerningForText(stringToRender, out letterCount);

            if(kernings == null)
                return false;
            else
                return true;
        }

        public CCTLLine GetLine (int startRange, int endRange)
        {
            var breakText = label.Text;

            var stringRange = breakText.Substring(startRange, endRange - startRange);
            int stringLength = string.IsNullOrEmpty(stringRange) ? 0 : stringRange.TrimEnd().Length;

            if (stringLength <= 0)
                return CCTLLine.NewCTLLine();

            endRange = startRange + stringLength;

            CCFontLetterDefinition letterDefinition = new CCFontLetterDefinition();

            int charXOffset = 0;
            int charYOffset = 0;
            int charAdvance = 0;

            CCPoint letterPosition;
            var nextFontPositionX = 0.0f;
            var nextFontPositionY = 0.0f;
            var contentScaleFactorWidth = CCLabel.DefaultTexelToContentSizeRatios.Width;
            var contentScaleFactorHeight = CCLabel.DefaultTexelToContentSizeRatios.Height;
            var line = CCTLLine.NewCTLLine();
            var additionalKerning = label.AdditionalKerning;

            for (int x = startRange; x < endRange; x++)
            {
                var c = breakText[x];

                if (fontAtlas.GetLetterDefinitionForChar(c, out letterDefinition))
                {
                    charXOffset         = (int)letterDefinition.XOffset;
                    charYOffset         = (int)letterDefinition.YOffset;
                    charAdvance         = (int)letterDefinition.XAdvance;
                }
                else
                {
                    charXOffset         = -1;
                    charYOffset         = -1;
                    charAdvance         = -1;
                }

                var kerning = kernings[x];
                letterDefinition.Kerning = kerning;

                letterPosition.X = (nextFontPositionX + charXOffset + kerning) / contentScaleFactorWidth;
                letterPosition.Y = (nextFontPositionY - charYOffset) / contentScaleFactorHeight;


                nextFontPositionX += charAdvance + kerning + additionalKerning;

                line.AddGlyph(letterPosition, letterDefinition, x);
            }

            return line;
        }

        public int SuggestLineBreak (int startIndex, float width)
        {
            var breakText = label.Text;

            int stringLength = string.IsNullOrEmpty(breakText) ? 0 : breakText.Length;
            if (stringLength <= 0)
                return 0;

            CCFontLetterDefinition letterDefinition = new CCFontLetterDefinition();

            bool isStartOfLine = false;
            float startOfLine = -1;
            var additionalKerning = label.AdditionalKerning;

            float scaleX = label.ScaleX;

            var limit = stringLength;
            int charXOffset = 0;
            int charYOffset = 0;
            int charAdvance = 0;

            CCPoint letterPosition;
            var nextFontPositionX = 0.0f;
            var nextFontPositionY = 0.0f;
            var contentScaleFactorWidth = CCLabel.DefaultTexelToContentSizeRatios.Width;
            var contentScaleFactorHeight = CCLabel.DefaultTexelToContentSizeRatios.Height;

            var breakPosition = 0;

            var lineBreaking = label.LabelFormat.LineBreaking;

            for (int j = startIndex; j < limit; j++)
            {            
                var c = breakText[j];
                var breakElement = lineBreakElements[j];
                breakPosition = breakElement.Position;

                if (breakElement.Condition == LineBreakCondition.Mandatory)
                    return breakPosition - startIndex;

                if (fontAtlas.GetLetterDefinitionForChar(c, out letterDefinition))
                {
                    charXOffset         = (int)letterDefinition.XOffset;
                    charYOffset         = (int)letterDefinition.YOffset;
                    charAdvance         = (int)letterDefinition.XAdvance;
                }
                else
                {
                    charXOffset         = -1;
                    charYOffset         = -1;
                    charAdvance         = -1;
                }

                var kerning = kernings[j];
                letterPosition.X = (nextFontPositionX + charXOffset + kerning) / contentScaleFactorWidth;
                letterPosition.Y = (nextFontPositionY - charYOffset) / contentScaleFactorHeight;

                nextFontPositionX += charAdvance + kerning + additionalKerning;

                if (!isStartOfLine)
                {
                    startOfLine = letterPosition.X * scaleX;;
                    isStartOfLine  = true;
                }

                // 1) Whitespace.
                // TODO: Handle CJK
                bool isCJK = false;

                // Whitespace. Allowed
                if (breakElement.Condition == LineBreakCondition.Allowed)
                {
                    continue;
                }


                float posRight = (letterPosition.X + letterDefinition.Width) * scaleX;

                //Console.WriteLine(c + " " + posRight + "lw: " + lineWidth + " le: " + breakElement);

                // Out of bounds.
                if (posRight - startOfLine > width)
                {
                    if (lineBreaking == CCLabelLineBreak.Word && !isCJK)
                    {
                        // Calculate the previous allowable position based on the Line Breaking element condition.
                        int na = j;
                        while (breakElement.Condition != LineBreakCondition.Allowed && na-- > 0)
                        {
                            breakElement = lineBreakElements[na];
                        }
                        return breakElement.Position + 1 - startIndex;
                    }
                    else
                    {
                        return breakElement.Position - startIndex;
                    }
                }
            }

            return breakPosition + 1 - startIndex;
        }

        // Flags the positions in the text where there are possible breaks
        //
        // Future enhancements
        // 1) would be based on the Unicode line break algorithm 
        // 2) would be based on a language-based hyphenation algorithm.
        void LineBreak (string labelText, float width = 0)
        {
            lineBreakElements.Clear();
            var strWhole = labelText;

            int stringLength = string.IsNullOrEmpty(strWhole) ? 0 : strWhole.Length;
            if (stringLength <= 0)
                return;

            bool isMandatory = false;
            int j = 0;
            for (j = 0; j < stringLength; j++)
            {
                var character = strWhole[j];

                // 1) Whitespace.
                bool isWhiteSpace = Char.IsWhiteSpace(character);
                // TODO: Handle CJK
                bool isCJK = false;

                // Whitespace.
                if (isWhiteSpace)
                {
                    if (character == '\r')
                    {
                        lineBreakElements.Add(new LineBreakElement(j, LineBreakCondition.Prohibited));
                        isMandatory = true;
                        continue;
                    }

                    if (character == '\n')
                    {
                        isMandatory = true;
                        continue;
                    }

                    lineBreakElements.Add(new LineBreakElement(j, LineBreakCondition.Allowed));
                    isMandatory = false;
                    continue;
                }

                if (isMandatory)
                    lineBreakElements.Add(new LineBreakElement(j, LineBreakCondition.Mandatory));

                lineBreakElements.Add(new LineBreakElement(j, LineBreakCondition.Prohibited));
                isMandatory = false;
            }

            lineBreakElements.Add(new LineBreakElement(j, LineBreakCondition.Mandatory));
        }

    }
}

namespace CocosSharp
{
    /// <summary>
    /// Describes the line breaking condition.
    /// </summary>
    internal enum LineBreakCondition
    {
        /// <summary>
        /// Break is allowed.
        /// </summary>
        Allowed,
        /// <summary>
        /// Break is mandatory.
        /// </summary>
        Mandatory,
        /// <summary>
        /// Break is prohibited.
        /// </summary>
        Prohibited,
    }
}

namespace CocosSharp
{
    /// <summary>
    /// Represents struct to store information about line break position and condition.
    /// </summary>
    internal struct LineBreakElement
    {
        private int m_position;
        private LineBreakCondition m_condition;

        /// <summary>
        /// Initializes a new instance of the class <see cref="LineBreakElement"/>.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="condition"></param>
        public LineBreakElement(int position, LineBreakCondition condition)
        {
            m_position = position;
            m_condition = condition;
        }

        /// <summary>
        /// Gets or sets line break position.
        /// </summary>
        public int Position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
            }
        }

        /// <summary>
        /// Gets or sets line break condition.
        /// </summary>
        public LineBreakCondition Condition
        {
            get
            {
                return m_condition;
            }
            set
            {
                m_condition = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="LineBreakElement"/>.
        /// </summary>
        /// <returns>A string that represents the instance.</returns>
        public override string ToString()
        {
            return string.Format("Position={0};Condition={1}", m_position, m_condition);
        }
    }
}


