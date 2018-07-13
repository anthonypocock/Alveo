using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;
using Alveo.Interfaces.UserCode;

namespace Alveo.UserCode
{
    [Serializable]
    [Description("Three Bar Reversal")]
    public class ThreeBarReversal : IndicatorBase
    {
        #region Properties

        #endregion
        int tbrHighIcon;
        int tbrLowIcon;

        Array<double> tbrHigh;
        Array<double> tbrLow;

        public ThreeBarReversal()
        {

            indicator_buffers = 2;
            indicator_chart_window = true;

            tbrHigh = new Array<double>();
            tbrLow = new Array<double>();

            // Legals
            copyright = "Anthony Pocock";
            link = "https://github.com/anthonypocock/Alveo";

            tbrHighIcon = 225;
            tbrLowIcon = 226;

        }


        protected override int Init()
        {
            SetIndexStyle(0, DRAW_ARROW, STYLE_DOT);
            SetIndexArrow(0, tbrHighIcon);
            SetIndexBuffer(0, tbrHigh);
            SetIndexLabel(0, "Long 3 Bar Reversal");
            SetIndexStyle(1, DRAW_ARROW, STYLE_DOT);
            SetIndexArrow(1, tbrLowIcon);
            SetIndexBuffer(1, tbrLow);
            SetIndexLabel(1, "Short 3 Bar Reversal");


            IndicatorShortName("Three Bar Reversal");

            ArrayResize(tbrHigh, Bars);
            ArrayResize(tbrLow, Bars);

            return 0;
        }

        protected override int Deinit()
        {
            return 0;
        }

        protected override int Start()
        {
            for(int i = Bars - 4; i >= 0; i--)
            {

                double barOneHigh = iHigh(Symbol(), Period(), i + 2);
                double barOneLow = iLow(Symbol(), Period(), i + 2);
                double barOneOpen = iOpen(Symbol(), Period(), i + 2);
                double barOneClose = iClose(Symbol(), Period(), i + 2);
                double barTwoHigh = iHigh(Symbol(), Period(), i + 1);
                double barTwoLow = iLow(Symbol(), Period(), i + 1);
                double barThreeHigh = iHigh(Symbol(), Period(), i);
                double barThreeLow = iLow(Symbol(), Period(), i);
                double barThreeOpen = iOpen(Symbol(), Period(), i );
                double barThreeClose = iClose(Symbol(), Period(), i);


                if ((barThreeClose > barThreeOpen) && (barOneOpen > barOneClose) && (barTwoHigh < barOneHigh) && (barTwoHigh < barThreeHigh) && (barTwoLow < barOneLow) && (barTwoLow < barThreeLow))
                {

                    tbrHigh[i + 1] = MathAbs(barTwoHigh + MathAbs(iATR(Symbol(), Period(), 1, i + 1) * 2));
                    

                }
                else
                {
                    tbrHigh[i] = EMPTY_VALUE;
                }

                if ((barThreeClose < barThreeOpen) && (barOneOpen > barOneClose) && (barTwoHigh > barOneHigh) && (barTwoHigh > barThreeHigh) && (barTwoLow > barOneLow) && (barTwoLow > barThreeLow))
                {

                    tbrLow[i + 1] = MathAbs(barTwoLow - MathAbs(iATR(Symbol(), Period(), 1, i + 1) * 2));
                    

                }
                else
                {
                    tbrLow[i] = EMPTY_VALUE;
                }

            }


            return 0;
        }
        #region Auto Generated Code

        [Description("Parameters order Symbol, TimeFrame, Period, Shift")]
        public override bool IsSameParameters(params object[] values)
        {
            if (values.Length != 3)
                return false;

            if (!CompareString(Symbol, (string)values[0]))
                return false;

            if (TimeFrame != (int)values[1])
                return false;

            return true;
        }

        [Description("Parameters order Symbol, TimeFrame, Period")]
        public override void SetIndicatorParameters(params object[] values)
        {
            if (values.Length != 3)
                throw new ArgumentException("Invalid parameters number");

            Symbol = (string)values[0];
            TimeFrame = (int)values[1];
        }

        #endregion
    }
}
