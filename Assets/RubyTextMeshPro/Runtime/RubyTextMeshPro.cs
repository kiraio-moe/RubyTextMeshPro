﻿using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMPro
{
    using static RubyTextMeshProDefinitions;

    public class RubyTextMeshPro : TextMeshPro
    {
        [Tooltip("Vertical offset of ruby. (em, px, %).")]
        [SerializeField]
        private string rubyVerticalOffset = "1em";

        [Tooltip("Ruby scale. (1 = 100%)")]
        [SerializeField]
        private float rubyScale = 0.5f;

        [Tooltip("The height of the ruby line can be specified. (em, px, %).")]
        [SerializeField]
        private string rubyLineHeight = "";

        [FormerlySerializedAs("m_uneditedText")]
        [Tooltip("Text contain unparsed tags.")]
        [SerializeField]
        [TextArea(5, 10)]
        private string _uneditedText;

        [Tooltip("ruby show type.")]
        [SerializeField]
        private RubyShowType rubyShowType = RubyShowType.RUBY_ALIGNMENT;

        public string uneditedText
        {
            get
            {
                return this._uneditedText;
            }
            set
            {
                this._uneditedText = value;
                this.SetTextCustom(this._uneditedText);
            }
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetTextCustom(this._uneditedText);
        }
#endif

        private void SetTextCustom(string value)
        {
            this.text = this.ReplaceRubyTags(value);

            // m_havePropertiesChanged : text changed => true, ForceMeshUpdate in OnPreRenderCanvas => false
            if (this.m_havePropertiesChanged)
            // changes to the text object properties need to be applied immediately.
            {
                this.ForceMeshUpdate();
            }
        }

        public override void ForceMeshUpdate(
            bool ignoreActiveState = false,
            bool forceTextReparsing = false
        )
        {
            base.ForceMeshUpdate(ignoreActiveState, forceTextReparsing);

            if (this.m_enableAutoSizing)
            {
                // change auto size timing, update ruby tag size.
                this.text = this.ReplaceRubyTags(this._uneditedText);
                base.ForceMeshUpdate(ignoreActiveState, forceTextReparsing);
            }
        }

        /// <summary>
        /// replace ruby tags.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>relpaced str</returns>
        private string ReplaceRubyTags(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            // warning! bad Know-how
            // Can not get GetPreferredValues("\u00A0").x at width,
            // add string and calculate.
            // and Use GetPreferredValues, change this.m_maxFontSize value.
            float nonBreakSpaceW =
                this.GetPreferredValues("\u00A0a").x - this.GetPreferredValues("a").x;
            float fontSizeScale = 1f;

            if (this.m_enableAutoSizing)
            {
                fontSizeScale = this.m_fontSize / this.m_maxFontSize;
            }

            int dir = this.isRightToLeftText ? 1 : -1;
            // Q. Why (m_isOrthographic ? 1 : 10f) => A. TMP_Text.cs L7622, L7625
            float hiddenSpaceW =
                dir
                * nonBreakSpaceW
                * (this.m_isOrthographic ? 1 : 10f)
                * this.rubyScale
                * fontSizeScale;
            // Replace <ruby> tags text layout.
            MatchCollection matches = RubyTextMeshProDefinitions.RUBY_REGEX.Matches(str);

            foreach (Match match in matches)
            {
                if (match.Groups.Count != 5)
                {
                    continue;
                }

                string fullMatch = match.Groups[0].ToString();
                string rubyText = match.Groups["ruby"].ToString();
                string baseText = match.Groups["val"].ToString();

                float rubyTextW =
                    this.GetPreferredValues(rubyText).x
                    * (this.m_isOrthographic ? 1 : 10f)
                    * this.rubyScale;
                float baseTextW =
                    this.GetPreferredValues(baseText).x * (this.m_isOrthographic ? 1 : 10f);

                if (this.m_enableAutoSizing)
                {
                    rubyTextW *= fontSizeScale;
                    baseTextW *= fontSizeScale;
                }

                float rubyTextOffset = dir * (baseTextW / 2f + rubyTextW / 2f);
                float compensationOffset = -dir * ((baseTextW - rubyTextW) / 2f);
                string replace = this.CreateReplaceValue(
                    baseText,
                    rubyText,
                    rubyTextOffset,
                    compensationOffset,
                    this.isRightToLeftText
                );
                str = str.Replace(fullMatch, replace);
            }

            if (!string.IsNullOrWhiteSpace(this.rubyLineHeight))
            // warning! bad Know-how
            // line-height tag is down the next line start.
            // now line can't change, corresponding by putting a hidden ruby
            {
                str =
                    $"<line-height={this.rubyLineHeight}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>\u00A0</size></voffset><space={hiddenSpaceW}>"
                    + str;
            }

            return str;
        }

        private string CreateReplaceValue(
            string baseText,
            string rubyText,
            float rubyTextOffset,
            float compensationOffset,
            bool isRightToLeftText
        )
        {
            string replace = string.Empty;

            switch (this.rubyShowType)
            {
                case RubyShowType.BASE_ALIGNMENT:
                    replace =
                        $"<nobr>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                    break;

                case RubyShowType.RUBY_ALIGNMENT:
                    if (isRightToLeftText)
                    {
                        if (compensationOffset < 0)
                        {
                            replace =
                                $"<nobr>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                        else
                        {
                            replace =
                                $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset></nobr>";
                        }
                    }
                    else
                    {
                        if (compensationOffset < 0)
                        {
                            replace =
                                $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset></nobr>";
                        }
                        else
                        {
                            replace =
                                $"<nobr>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                    }

                    break;
            }

            return replace;
        }

        /// <summary>
        /// TMP_Text ConvertToFloat
        /// if (startIndex == 0) { lastIndex = 0; return -9999; } delete version
        /// </summary>
        protected float ConvertToFloatOrigin(char[] chars, int startIndex, int lastIndex)
        {
            int endIndex = lastIndex;
            bool isIntegerValue = true;
            float decimalPointMultiplier = 0;

            // Set value multiplier checking the first character to determine if we are using '+' or '-'
            int valueSignMultiplier = 1;

            if (chars[startIndex] == '+')
            {
                valueSignMultiplier = 1;
                startIndex += 1;
            }
            else if (chars[startIndex] == '-')
            {
                valueSignMultiplier = -1;
                startIndex += 1;
            }

            float value = 0;

            for (int i = startIndex; i < endIndex; i++)
            {
                uint c = chars[i];

#if UNITY_2021_1_OR_NEWER
                if (c is >= '0' and <= '9' || c == '.')
#else
                if (c >= '0' && c <= '9' || c == '.')
#endif
                {
                    if (c == '.')
                    {
                        isIntegerValue = false;
                        decimalPointMultiplier = 0.1f;
                        continue;
                    }

                    //Calculate integer and floating point value
                    if (isIntegerValue)
                    {
                        value = value * 10 + (c - 48) * valueSignMultiplier;
                    }
                    else
                    {
                        value = value + (c - 48) * decimalPointMultiplier * valueSignMultiplier;
                        decimalPointMultiplier *= 0.1f;
                    }

                    continue;
                }

                if (c == ',')
                {
                    return value;
                }
            }

            return value;
        }
    }
}
