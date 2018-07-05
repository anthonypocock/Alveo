using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;
using Alveo.Interfaces.UserCode;

namespace Alveo.UserCode
{
    [Serializable]
    [Description("TEMA Indicator")]
    public class TEMA : IndicatorBase
    {
        #region Properties

        [Category("Settings")]
        [Description("Period")]
        public int TEMAPeriod { get; set; }
        
        [Category("Settings")]
        [Description("Trend Shift")]
        public int TEMATrendShift { get; set; }

        #endregion

        Array<double> temaSignal;
        Array<double> temaTrend;

        public TEMA()
        {

            indicator_buffers = 2;
            indicator_chart_window = true;

            indicator_width1 = 2;
            indicator_color1 = Turquoise;
            indicator_style1 = STYLE_SOLID;

            indicator_width2 = 2;
            indicator_color2 = LimeGreen;
            indicator_style2 = STYLE_SOLID;

            temaSignal = new Array<double>();
            temaTrend = new Array<double>();

            // Legals
            copyright = "Anthony Pocock";
            link = "";
            TEMAPeriod = 14;
            TEMATrendShift = 2;

        }


        protected override int Init()
        {
            SetIndexBuffer(0, temaSignal);
            SetIndexStyle(0, 0);
            SetIndexLabel(0, "TEMA Signal");
            SetIndexBuffer(1, temaTrend);
            SetIndexStyle(1, 0);
            SetIndexLabel(1, "TEMA Trend");

            IndicatorShortName("TEMA");

            return 0;
        }

        protected override int Deinit()
        {
            return 0;
        }

        protected override int Start()
        {
            Array<double> baseArray = new Array<double>();
            Array<double> ema = new Array<double>();
            Array<double> ema_ema = new Array<double>();
            Array<double> ema_ema_ema = new Array<double>();
            
            ArrayResize(baseArray, Bars);
            ArrayResize(ema, Bars);
            ArrayResize(ema_ema, Bars);
            ArrayResize(ema_ema_ema, Bars);

            ArrayInitialize(temaSignal, EMPTY_VALUE);
            ArrayInitialize(temaTrend, EMPTY_VALUE);

            ArrayCopy(baseArray, GetPrice(GetHistory(Symbol(), TimeFrame), PRICE_CLOSE));
            ArrayCopy(ema, GetPrice(GetHistory(Symbol(), TimeFrame), PRICE_CLOSE));
            ArrayCopy(ema_ema, GetPrice(GetHistory(Symbol(), TimeFrame), PRICE_CLOSE));
            ArrayCopy(ema_ema_ema, GetPrice(GetHistory(Symbol(), TimeFrame), PRICE_CLOSE));

            if (baseArray.Count == 0)
                return 0;

            for (var i = (TEMAPeriod * 3); i < Bars; i++) 
            {
                ema[i, false] = (double)((decimal)baseArray[i] * (2M / ((decimal)TEMAPeriod + 1M)) + (1M - (2M / ((decimal)TEMAPeriod + 1M))) * (decimal)ema[i - 1]);
                ema_ema[i, false] = (double)((decimal)ema[i] * (2M / ((decimal)TEMAPeriod + 1M)) + (1M - (2M / ((decimal)TEMAPeriod + 1M))) * (decimal)ema_ema[i - 1]);
                ema_ema_ema[i, false] = (double)((decimal)ema_ema[i] * (2M / ((decimal)TEMAPeriod + 1M)) + (1M - (2M / ((decimal)TEMAPeriod + 1M))) * (decimal)ema_ema_ema[i - 1]);
                temaSignal[i, false] = (double)((3M * (decimal)ema[i]) - (3M * (decimal)ema_ema[i]) + (decimal)ema_ema_ema[i]);
                temaTrend[i + TEMATrendShift, false] = (double)((3M * (decimal)ema[i]) - (3M * (decimal)ema_ema[i]) + (decimal)ema_ema_ema[i]);
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

            if (TEMAPeriod != (int)values[2])
                return false;

            if (TEMATrendShift != (int)values[3])
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
            TEMAPeriod = (int)values[2];
            TEMATrendShift = (int)values[3];

        }

        #endregion
    }
}
