using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;
using Alveo.Interfaces.UserCode;

namespace Alveo.UserCode
{

    [Description("Support & Resistance Lines")]
    public class SRLines : IndicatorBase
    {
        #region Properties
        [Category("Settings")]
        [Description("SR Bars")]
        public int SRBars { get; set; }

        #endregion

        Array<double> Resistance;
        Array<double> Support;

        int currentBars;
        int lastUpdatedBarCount;

        public SRLines()
        {

            indicator_buffers = 2;
            indicator_chart_window = true;

            Resistance = new Array<double>();
            Support = new Array<double>();

            // Legals
            copyright = "Anthony Pocock";
            link = "https://github.com/anthonypocock/Alveo";
            SRBars = 6;
            currentBars = 0;
            lastUpdatedBarCount = 0;



        }


        protected override int Init()
        {
            SetIndexStyle(0, DRAW_LINE, STYLE_SOLID);
            SetIndexBuffer(0, Resistance);
            SetIndexLabel(0, "Resistance");
            SetIndexStyle(1, DRAW_LINE, STYLE_SOLID);
            SetIndexBuffer(1, Support);
            SetIndexLabel(1, "Support");


            IndicatorShortName("Support & Resistance Lines");

            ArrayResize(Resistance, Bars);
            ArrayResize(Support, Bars);

            return 0;
        }

        protected override int Deinit()
        {
            return 0;
        }

        protected override int Start()
        {
            lastUpdatedBarCount = Bars;

            if (lastUpdatedBarCount > currentBars)
            {

                ArrayResize(Resistance, Bars);
                ArrayResize(Support, Bars);
                double lowest = Ask;
                for (int x = 1; x <= SRBars + 1; x++)
                {
                    double barLow = iLow(Symbol(), Period(), x);
                    if (barLow < lowest)
                    {
                        lowest = barLow;
                    }
                }


                double highest = Bid;
                for (int x = 1; x <= SRBars + 1; x++)
                {
                    double barHigh = iHigh(Symbol(), Period(), x);
                    if (barHigh > highest)
                    {
                        highest = barHigh;
                    }

                }

                for (int i = Bars - SRBars - 1; i >= 0; i--)
                {

                    Support[i, true] = EMPTY_VALUE;
                    Resistance[i, true] = EMPTY_VALUE;
                }
                for (int i = 1; i <= SRBars + 1; i++)
                {

                    Resistance[i, true] = highest;
                    Support[i, true] = lowest;
                }
                currentBars = lastUpdatedBarCount;
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
