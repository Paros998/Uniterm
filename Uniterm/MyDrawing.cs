using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uniterm
{

    public class MyDrawing
    {

        #region Fields

        public static Pen pen
        {
            get
            {
                return new Pen(Brushes.SteelBlue, (int)Math.Log(fontsize, 3));
            }
        }

        private static Brush br = Brushes.White;

        public static FontFamily fontFamily = new FontFamily("Arial");

        public static /*double*/ Int32 fontsize = 12;

        public static string sA, sB, sOp;

        public static string eA, eB, eC;

        public static char oper = ' ';


        public static int FS = 12;

        public DrawingContext dc;
        #endregion

        #region Initalizers

        public MyDrawing(DrawingContext drawingContext)
        {
            dc = drawingContext;
        }

        #endregion

        #region Public Methods

        public void Redraw()
        {
            if (oper != ' ')          
            {
                DrawSwitched(new Point(20, fontsize + 30));
            }
            else
            {
                if (sA != "" && sA != null)
                {
                    DrawSekVert(new Point(30, fontsize));
                }
                if (eA != "" && eA != null)
                {
                    DrawElimVert(new Point(30 + (2* fontsize), fontsize));
                }
            }
        }

        public static void ClearAll()
        {
            sA = sB = sOp = "";
            eA = eB = "";
            oper = ' ';
        }


        public void DrawSekVert(Point pt)
        {
            if (sA == "" || sB == "" || sOp == "") return;

            string text = sA + Environment.NewLine.ToString() + sOp + Environment.NewLine.ToString() + sB + Environment.NewLine.ToString();

            double len = GetTextHeight(text);

            DrawText(pt, text);

            DrawBezier(new Point(pt.X, pt.Y - 1), (int)len + (fontsize / 3));
        }

        public void DrawElimVert(Point pt)
        {
            if (eA == "" || eB == "") return;

            Point p2 = new Point(pt.X + 2, pt.Y);
            string text = eA + Environment.NewLine.ToString() + ";" + Environment.NewLine.ToString() + eB + Environment.NewLine.ToString() ;

            double l = GetTextHeight(text) + 2;

            DrawText(p2, text);
            DrawVert(pt, (int)l);
        }

        public void DrawSwitched(Point pt)
        {
            if (sA == "" || sB == "" || sOp == "" || eA == "" || eB == "" ) return;


            string textElim = eA + ";" + eB;

            string textSeq1 = textElim + Environment.NewLine.ToString() + sOp + Environment.NewLine.ToString() + sB;
            string textSeq2 = sA + Environment.NewLine.ToString() + sOp + Environment.NewLine.ToString() + textElim;

            if (oper == 'A')
            {
                DrawBezier(pt, GetTextHeight(textSeq1) + 10);
                DrawHor(new Point(pt.X + 5, pt.Y + 0.5), GetTextLength(textSeq1));
                DrawText(new Point(pt.X + 5, pt.Y), textSeq1);
            }
            if (oper == 'B')
            {
                DrawBezier(pt, GetTextHeight(textSeq2) + 10);
                DrawHor(new Point(pt.X + 5, pt.Y + GetTextHeight(textSeq2) - fontsize), GetTextLength(textSeq2));
                DrawText(new Point(pt.X + 5, pt.Y), textSeq2);
            }

        }
        #endregion

        #region Private Methods

        private void DrawVert(Point pt, int length)
        {
            dc.DrawLine(pen, pt, new Point { X = pt.X, Y = pt.Y + length });

            dc.DrawLine(pen, new Point(pt.X - 0.5, pt.Y), new Point(pt.X + (fontsize / 2), pt.Y));
            dc.DrawLine(pen, new Point(pt.X - 0.5, pt.Y + length), new Point(pt.X + (fontsize / 2), pt.Y + length));

        }

        private void DrawHor(Point pt, int length)
        {
            pt.X = pt.X - 2;
            pt.Y = pt.Y + fontsize + 5;

            dc.DrawLine(pen, pt, new Point { X = pt.X + length + 4, Y = pt.Y });

            dc.DrawLine(pen, new Point(pt.X, pt.Y + 0.5), new Point(pt.X, pt.Y - (fontsize / 2)));
            dc.DrawLine(pen, new Point(pt.X + 4 + length, pt.Y + 0.5), new Point(pt.X + length + 4, pt.Y - (fontsize / 2)));

        }

        private void DrawBezier(Point p0, int length)
        {
            Point start = p0;
            Point p1 = new Point(), p2 = new Point(), p3 = new Point();

            p3.X = p0.X;
            p3.Y = p0.Y + length;

            int b = (int)Math.Sqrt(length) + 2;

            p1.Y = p0.Y + (int)(length * 0.25);
            p1.X = p0.X - b;

            p2.Y = p0.Y + (int)(length * 0.75);
            p2.X = p0.X - b;

            foreach (Point pt in GetBezierPoints(p0, p1, p2, p3))
            {
                dc.DrawLine(pen, start, pt);
                start = pt;
            }
        }

        private void DrawText(Point point, string text)
        {
            dc.DrawText(GetFormattedText(text), point);
        }

        private int GetTextHeight(string text)
        {
            return (int)GetFormattedText(text).Height;
        }

        private int GetTextLength(string text)
        {
            return (int)GetFormattedText(text).Width;
        }

        private IEnumerable<Point> GetBezierPoints(Point A, Point B, Point C, Point D)
        {
            List<Point> points = new List<Point>();

            for (double t = 0.0d; t <= 1.0; t += 1.0 / 500)
            {
                double tbs = Math.Pow(t, 2);
                double tbc = Math.Pow(t, 3);
                double tas = Math.Pow((1 - t), 2);
                double tac = Math.Pow((1 - t), 3);

                points.Add(new Point
                {
                    Y = +tac * A.Y
                        + 3 * t * tas * B.Y
                        + 3 * tbs * (1 - t) * C.Y
                        + tbc * D.Y,
                   X = +tac * A.X
                        + 3 * t * tas * B.X
                        + 3 * tbs * (1 - t) * C.X
                        + tbc * D.X
                });
            }

            return points;
        }

        private FormattedText GetFormattedText(string text)
        {
            FontStyle style = FontStyles.Normal;

            style = FontStyles.Normal;
            Typeface typeface = new Typeface(fontFamily, style, FontWeights.Light, FontStretches.Medium);

            FormattedText formattedText = new FormattedText(text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface, fontsize, Brushes.Black);

            formattedText.TextAlignment = TextAlignment.Left;

            return formattedText;
        }

        #endregion
    }
}
