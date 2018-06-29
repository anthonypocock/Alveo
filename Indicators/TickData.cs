using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;
using Alveo.Interfaces.UserCode;

namespace Alveo.UserCode
{
    [Serializable]
    [Description("Tick Data")]
    public class TickData : IndicatorBase
    {
        #region Properties


        #endregion

        Array<double> tickData;

        public TickData()
        {

            indicator_buffers = 1;
            indicator_chart_window = false;

            indicator_width1 = 2;
            indicator_color1 = Red;
            indicator_style1 = STYLE_SOLID;
            
            tickData = new Array<double>();

            // Legals
            copyright = "Anthony Pocock";
            link = "https://github.com/anthonypocock/Alveo";

        }


        protected override int Init()
        {
            SetIndexBuffer(0, tickData);
            SetIndexStyle(0, 0); 
            SetIndexLabel(0, "Tick Value");
            
            IndicatorShortName("Tick Data");

            ArrayResize(tickData, Bars);

            return 0;
        }

        protected override int Deinit()
        {
            return 0;
        }

        protected override int Start()
        {
            if (tickData[0] == 0)
                ArrayInitialize(tickData, Ask);
            for (int i = Bars; i >= 0; i--)
            {
                tickData[i, true] = tickData[i - 1];
            }
            tickData[0, true] = Ask;
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
