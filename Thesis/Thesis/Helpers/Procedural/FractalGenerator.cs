using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Thesis.Helpers.Procedural
{
    public class FractalGenerator
    {
        public static bool[] iterateAxis = new bool[4] { false, true, false, false };
        private static List<KeyValuePair<string, string>> axiomAndRules;
        private static (string, int?) InitializeString(int source, int? iterationCustom = null)
        {
            int? iteration = iterationCustom != null ? iterationCustom : source & 7; // take 4 bits [from 0 to 7]

            //int ele = (source & 7) + 3;
            int ele = new Random().Next(0, 10);
            var RuAndAx = axiomAndRules.ElementAt(ele);
            string initialString = RuAndAx.Key;

            StringBuilder sB = new StringBuilder();
            for (int i = 0; i < iteration; i++)
            {
                for (int y = 0; y < initialString.Length; y++)
                {
                    if (char.IsLetter(initialString[y]))
                        sB.Append(RuAndAx.Value);
                    else
                        sB.Append(initialString[y]);
                }
                initialString = sB.ToString();
                sB = new StringBuilder();
            }

            return (initialString, iteration);
        }
        private static List<CustomPoint> pointsOfImage = new List<CustomPoint>();

        public static Bitmap GenereateFractal1(string email)
        {
            InitializeRulesAndAxioms();
            Random rand = new Random();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            decimal allpoints = 0;

            int ccounter = 0;
            StringBuilder sB = new StringBuilder();



            sB = new StringBuilder();
            //30% for user to get rule from list == nice image but repetable
            bool shouldGetFromRuleList = rand.NextDouble() < 0.3;
            string emailOfUser = email;
            string hash = "";
            using (MD5 md5 = MD5.Create())
            {
                hash = GetMd5Hash(md5, emailOfUser);
            }
            var source = hash.GetHashCode();
            int iterationOfFractal = source & 7;

            if (iterationOfFractal <= 1)
                iterationOfFractal = 2;

            if (iterationOfFractal >= 6)
                iterationOfFractal = 3;
            string axiom = "";
            string rule = "";


            if (shouldGetFromRuleList)
            {
                int index = source & 15;
                if (index > 12) index = 12;
                int bitEle = (source >> 4) & 15;
                bitEle = bitEle > 10 ? 10 : bitEle;

                var getElement = axiomAndRules[bitEle];
                axiom = getElement.Key;
                rule = getElement.Value;
            }
            else
            {
                //get next 3 bits do & operation on it and add since (since number will be from 0 to 3 and we want from 1 to 4
                int amountForAxiom = (source & 3) + 1; 
                int amountForRule = 0;
                switch (iterationOfFractal)
                {
                    case 1: amountForRule = 30; break;
                    case 2: amountForRule = 25; break;
                    case 3: amountForRule = 21; break;
                    case 4: amountForRule = 17; break;
                    case 5: amountForRule = 11; break;
                    case 6: amountForRule = 9; break;
                    case 7: amountForRule = 7; break;
                }

                axiom = GenerateContent(axiom, amountForAxiom, rand);
                if (!axiom.Contains('F'))
                {
                    if (axiom.Length >= 2)
                        axiom = axiom.Remove(1, 1).Insert(1, "F");
                    else
                        axiom += "F";
                }
                //generate rule
                rule = GenerateContent(rule, amountForRule, rand);

            }

            for (int i = 0; i <= iterationOfFractal; i++)
            {
                for (int y = 0; y < axiom.Length; y++)
                {
                    if (char.IsLetter(axiom[y]))
                        sB.Append(rule);
                    else
                        sB.Append(axiom[y]);
                }
                axiom = sB.ToString();
                if (axiom.Length >= 10000000)
                    break;
                sB = new StringBuilder();
            }
            if (axiom.Length >= 8500000)
            {
                //Take 7000000 from end to end - 700000
                Random rnd = new Random();
                int numToTake = rnd.Next(3000000, 6000000);
                var smallerInitialString = axiom.Substring(axiom.Length - numToTake, numToTake);
                axiom = smallerInitialString;
            }

            Bitmap mainImage = new Bitmap(1000, 1000);
            float currentX = 500;
            float currentY = 500;

            float iterate = 10;

            int longWidth = (source >> 8) & 1;
            if (longWidth > 1)
                longWidth = rand.Next(20, 30);
            else
                longWidth = rand.Next(2, 13);


            int widthOfPen = 0;
            if (shouldGetFromRuleList)
                widthOfPen = 15;
            else
                widthOfPen = rand.Next(10, 55);

            GraphicsPath gP;

            var color1 = int.Parse(hash.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var color2 = int.Parse(hash.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var color3 = int.Parse(hash.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            Color foreGround = Color.FromArgb(color1, color2, color3);
            foreGround = ControlPaint.Dark(foreGround, 50);

            var color4 = int.Parse(hash.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            var color5 = int.Parse(hash.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            var color6 = int.Parse(hash.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
            Color backGround = Color.FromArgb(color4, color5, color6);
            backGround = ControlPaint.Light(foreGround, 25);

            if (foreGround.ToArgb() == backGround.ToArgb())
            {
                foreGround = Color.Red;
                backGround = Color.Black;
            }
            if (foreGround.R == backGround.R || foreGround.G == backGround.G || foreGround.B == backGround.B)
            {
                if (foreGround.R == backGround.R)
                {
                    color2 = color2 - 50 < 0 ? 0 : color2 + 50;
                    color3 = color3 - 50 < 0 ? 0 : color3 - 50;
                    foreGround = Color.FromArgb(color1, color2, color3);
                    foreGround = ControlPaint.Dark(foreGround, 50);

                    color5 = color5 + 50 > 260 ? 260 : color5 + 50;
                    color6 = color6 + 50 > 260 ? 260 : color6 + 50;
                    backGround = Color.FromArgb(color4, color5, color6);
                    backGround = ControlPaint.Light(foreGround, 25);
                }
                else if (foreGround.G == backGround.G)
                {
                    color1 = color1 - 50 < 0 ? 0 : color1 + 50;
                    color3 = color3 - 50 < 0 ? 0 : color3 - 50;
                    foreGround = Color.FromArgb(color1, color2, color3);
                    foreGround = ControlPaint.Dark(foreGround, 50);

                    color4 = color4 + 50 > 260 ? 260 : color4 + 50;
                    color6 = color6 + 50 > 260 ? 260 : color6 + 50;
                    backGround = Color.FromArgb(color4, color5, color6);
                    backGround = ControlPaint.Light(foreGround, 25);
                }
                else if (foreGround.B == backGround.B)
                {
                    color1 = color1 - 50 < 0 ? 0 : color1 + 50;
                    color2 = color2 - 50 < 0 ? 0 : color2 - 50;
                    foreGround = Color.FromArgb(color1, color2, color3);
                    foreGround = ControlPaint.Dark(foreGround, 50);

                    color4 = color4 + 50 > 260 ? 260 : color4 + 50;
                    color5 = color5 + 50 > 260 ? 260 : color5 + 50;
                    backGround = Color.FromArgb(color4, color5, color6);
                    backGround = ControlPaint.Light(foreGround, 25);
                }

            }

            gP = new GraphicsPath();
            for (int i = 0; i < axiom.Length; i++)
            {
                if (char.IsLetter(axiom[i]))
                {
                    for (int iA = 0; iA < iterateAxis.Length; iA++)
                    {
                        //up
                        if (iA == 0 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX, currentY - iterate);
                            currentY -= iterate;
                        }
                        //right
                        else if (iA == 1 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX + iterate, currentY);
                            currentX += iterate;
                        }
                        //down
                        else if (iA == 2 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX, currentY + iterate);
                            currentY += iterate;
                        }
                        //left
                        else if (iA == 3 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX - iterate, currentY);
                            currentX -= iterate;
                        }
                    }
                }
                else if (axiom[i] == '+')
                {
                    changeAxis(false);
                }
                else if (axiom[i] == '-')
                {
                    changeAxis(true);
                }
            }
            var T = GetMatrixFitRectInBounds(gP.GetBounds(), new RectangleF(0, 0, mainImage.Width, mainImage.Height));
            gP.Transform(T);

            using (Graphics g = Graphics.FromImage(mainImage))
            {
                g.Clear(backGround);
                g.DrawPath(new Pen(foreGround, widthOfPen), gP);
            }
            allpoints += gP.PointCount;

            //  mainImage.Save(@"C:\Users\Ragnus\Desktop\PI\Fractal\FractalPro\Fractal\" + ccounter + ".jpg");
            return mainImage;

        }
        public static Bitmap GenereateFractal2(string email)
        {
            InitializeRulesAndAxioms();
            Random rand = new Random();
            StringBuilder sB = new StringBuilder();
            bool shouldGetFromRuleList = rand.NextDouble() < 0.3; //30% for user to get rule from list == nice image but repetable
            string emailOfUser = email;
            string hash = "";
            using (MD5 md5 = MD5.Create())
            {
                hash = GetMd5Hash(md5, emailOfUser);
            }
            var source = hash.GetHashCode();
            int iterationOfFractal = source & 7;
            if (iterationOfFractal < 1)
                iterationOfFractal = 1;

            string axiom = "";
            string rule = "";


            if (shouldGetFromRuleList)
            {
                int index = source & 15;
                if (index > 12) index = 12;
                int bitEle = (source >> 4) & 15;
                bitEle = bitEle > 10 ? 10 : bitEle;

                var getElement = axiomAndRules[bitEle];
                axiom = getElement.Key;
                rule = getElement.Value;
            }
            else
            {
                int amountForAxiom = (source & 3) + 1; //get next 3 bits do & operation on it and add since (since number will be from 0 to 3 and we want from 1 to 4
                int amountForRule = 0;
                switch (iterationOfFractal)
                {
                    case 1: amountForRule = 30; break;
                    case 2: amountForRule = 25; break;
                    case 3: amountForRule = 21; break;
                    case 4: amountForRule = 17; break;
                    case 5: amountForRule = 11; break;
                    case 6: amountForRule = 9; break;
                    case 7: amountForRule = 7; break;
                }

                axiom = GenerateContent(axiom, amountForAxiom, rand);
                if (!axiom.Contains('F'))
                {
                    if (axiom.Length >= 2)
                        axiom = axiom.Remove(1, 1).Insert(1, "F");
                    else
                        axiom += "F";
                }
                //generate rule
                rule = GenerateContent(rule, amountForRule, rand);

            }

            for (int i = 0; i <= iterationOfFractal; i++)
            {
                for (int y = 0; y < axiom.Length; y++)
                {
                    if (char.IsLetter(axiom[y]))
                        sB.Append(rule);
                    else
                        sB.Append(axiom[y]);
                }
                axiom = sB.ToString();
                if (axiom.Length >= 10000000)
                    break;
                sB = new StringBuilder();
            }
            if (axiom.Length >= 8500000)
            {
                //Take 7000000 from end to end - 700000
                Random rnd = new Random();
                int numToTake = rnd.Next(3000000, 6000000);
                var smallerInitialString = axiom.Substring(axiom.Length - numToTake, numToTake);
                axiom = smallerInitialString;
            }

            Bitmap mainImage = new Bitmap(1000, 1000);
            float currentX = 500;
            float currentY = 500;

            float iterate = 1;

            int longWidth = (source >> 8) & 1;
            if (longWidth > 1)
                longWidth = rand.Next(20, 30);
            else
                longWidth = rand.Next(2, 13);

            int widthOfPen = 3;

            GraphicsPath gP;

            var color1 = int.Parse(hash.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var color2 = int.Parse(hash.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var color3 = int.Parse(hash.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            Color foreGround = Color.FromArgb(color1, color2, color3);
            foreGround = ControlPaint.Dark(foreGround, 50);

            var color4 = int.Parse(hash.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            var color5 = int.Parse(hash.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            var color6 = int.Parse(hash.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
            Color backGround = Color.FromArgb(color4, color5, color6);
            backGround = ControlPaint.Light(foreGround, 25);

            if (foreGround.R == backGround.R || foreGround.G == backGround.G || foreGround.B == backGround.B)
            {
                if (foreGround.R == backGround.R)
                {
                    color2 = color2 - 50 < 0 ? 0 : color2 + 50;
                    color3 = color3 - 50 < 0 ? 0 : color3 - 50;
                    foreGround = Color.FromArgb(color1, color2, color3);
                    foreGround = ControlPaint.Dark(foreGround, 50);

                    color5 = color5 + 50 > 260 ? 260 : color5 + 50;
                    color6 = color6 + 50 > 260 ? 260 : color6 + 50;
                    backGround = Color.FromArgb(color4, color5, color6);
                    backGround = ControlPaint.Light(foreGround, 25);
                }
                else if (foreGround.G == backGround.G)
                {
                    color1 = color1 - 50 < 0 ? 0 : color1 + 50;
                    color3 = color3 - 50 < 0 ? 0 : color3 - 50;
                    foreGround = Color.FromArgb(color1, color2, color3);
                    foreGround = ControlPaint.Dark(foreGround, 50);

                    color4 = color4 + 50 > 260 ? 260 : color4 + 50;
                    color6 = color6 + 50 > 260 ? 260 : color6 + 50;
                    backGround = Color.FromArgb(color4, color5, color6);
                    backGround = ControlPaint.Light(foreGround, 25);
                }
                else if (foreGround.B == backGround.B)
                {
                    color1 = color1 - 50 < 0 ? 0 : color1 + 50;
                    color2 = color2 - 50 < 0 ? 0 : color2 - 50;
                    foreGround = Color.FromArgb(color1, color2, color3);
                    foreGround = ControlPaint.Dark(foreGround, 50);

                    color4 = color4 + 50 > 260 ? 260 : color4 + 50;
                    color5 = color5 + 50 > 260 ? 260 : color5 + 50;
                    backGround = Color.FromArgb(color4, color5, color6);
                    backGround = ControlPaint.Light(foreGround, 25);
                }

            }

            gP = new GraphicsPath();
            for (int i = 0; i < axiom.Length; i++)
            {
                if (char.IsLetter(axiom[i]))
                {
                    for (int iA = 0; iA < iterateAxis.Length; iA++)
                    {
                        //up
                        if (iA == 0 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX, currentY - iterate);
                            currentY -= iterate;
                        }
                        //right
                        else if (iA == 1 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX + iterate, currentY);
                            currentX += iterate;
                        }
                        //down
                        else if (iA == 2 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX, currentY + iterate);
                            currentY += iterate;
                        }
                        //left
                        else if (iA == 3 && iterateAxis[iA])
                        {
                            gP.AddLine(currentX, currentY, currentX - iterate, currentY);
                            currentX -= iterate;
                        }
                    }
                }
                else if (axiom[i] == '+')
                {
                    changeAxis(false);
                }
                else if (axiom[i] == '-')
                {
                    changeAxis(true);
                }
            }
            var T = GetMatrixFitRectInBounds(gP.GetBounds(), new RectangleF(0, 0, mainImage.Width, mainImage.Height));
            gP.Transform(T);

            Console.WriteLine("amount of points is " + gP.PointCount);

            using (Graphics g = Graphics.FromImage(mainImage))
            {
                g.Clear(backGround);
                g.DrawPath(new Pen(foreGround, widthOfPen), gP);
            }
            return mainImage;
        }
        public static Bitmap GenereateFractal3(string email)
        {
            InitializeRulesAndAxioms();
            string initialString;
            int cc = 0;
            Random rand = new Random();
            string emailOfUser = email;
            string hash = "";
            using (MD5 md5 = MD5.Create())
            {
                hash = GetMd5Hash(md5, emailOfUser);
            }
            var source = hash.GetHashCode();


            var values = InitializeString(source);

            initialString = values.Item1;
            int? iteration = values.Item2;


            int w;
            int h;
            float initialCurrentX;
            float initialCurrentY;
            float currentX;
            float currentY;


            float lineLen;
            float lineWidth;

            //TODO: Poprawić switcha dla małych i dużych wartościś
            switch (iteration)
            {
                case 7:
                    w = 2000;
                    h = 2000;
                    initialCurrentX = 1000;
                    initialCurrentY = 1100;
                    currentX = 1000f;
                    currentY = 1100f;
                    lineLen = rand.Next(1, 3);
                    break;
                case 6:
                case 5:
                    w = 1500;
                    h = 1500;
                    initialCurrentX = 750;
                    initialCurrentY = 800;
                    currentX = 750f;
                    currentY = 800f;
                    lineLen = rand.Next(4, 5);
                    break;
                case 4:
                    w = 1000;
                    h = 1000;
                    initialCurrentX = 550;
                    initialCurrentY = 600;
                    currentX = 750f;
                    currentY = 800f;
                    lineLen = rand.Next(5, 6);
                    break;
                case 3:
                    w = 500;
                    h = 500;
                    initialCurrentX = 250;
                    initialCurrentY = 300;
                    currentX = 250f;
                    currentY = 300f;
                    lineLen = rand.Next(7, 8);
                    break;
                default:
                    w = 200;
                    h = 200;
                    initialCurrentX = 75;
                    initialCurrentY = 75;
                    currentX = 75f;
                    currentY = 75f;
                    lineLen = rand.Next(9, 10);
                    break;
            }

            Bitmap mainImage = new Bitmap(w, h);

            lineWidth = rand.Next(0, 63);

            //float lineLen = new Random().Next(1, 10);
            //float lineWidth = new Random().Next(1, 63);

            int counter = 0;

            var color1 = int.Parse(hash.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var color2 = int.Parse(hash.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var color3 = int.Parse(hash.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            Color foreGround = Color.FromArgb(color1, color2, color3);

            var color4 = int.Parse(hash.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            var color5 = int.Parse(hash.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            var color6 = int.Parse(hash.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
            Color backGround = Color.FromArgb(color4, color5, color6);

            OneMoreItem:
            if (counter >= 3)
            {
                //TODO: POPRWAIĆ TE WARUNKI
                if (counter > 5)
                {
                    values = InitializeString(source, 1);
                    initialString = values.Item1;
                    mainImage = new Bitmap(3500, 3500);
                    counter = 0;
                    currentX = 2000;
                    currentY = 2100;
                    initialCurrentX = 2000;
                    initialCurrentY = 2100;
                }
                if (counter > 3)
                {
                    values = InitializeString(source, new Random().Next(1, 3));
                    initialString = values.Item1;
                    mainImage = new Bitmap(3000, 3000);
                    lineLen = 10;
                    lineWidth = 63;
                    currentX = 1600;
                    currentY = 1700;
                    initialCurrentX = 1600;
                    initialCurrentY = 1700;
                }
                else
                {
                    w += 1000;
                    h += 1000;
                    mainImage = new Bitmap(w, h);
                    currentX = (w / 2) + 100;
                    currentY = (w / 2) + 200;
                    initialCurrentX = (w / 2) + 100;
                    initialCurrentY = (w / 2) + 200;
                    lineLen = 6;
                    lineWidth = 30;
                }
            }

            using (Graphics g = Graphics.FromImage(mainImage))
            {
                g.Clear(foreGround);

                //g.ScaleTransform(-1, -1);
                for (int i = 0; i < initialString.Length; i++)
                {
                    if (char.IsLetter(initialString[i]))
                    {
                        for (int iA = 0; iA < iterateAxis.Length; iA++)
                        {
                            //up
                            if (iA == 0 && iterateAxis[iA])
                            {
                                g.DrawLine(new Pen(backGround, lineWidth), currentX, currentY, currentX, currentY - lineLen);
                                pointsOfImage.Add(new CustomPoint(currentX, currentY, currentX, currentY - lineLen));
                                currentY -= lineLen;
                                if (currentY < 0)
                                {
                                    lineLen = lineLen - 1 > 0 ? lineLen - 1 : 1;
                                    g.Clear(Color.Black);
                                    pointsOfImage = new List<CustomPoint>();
                                    initialCurrentY += 100;
                                    currentY = initialCurrentY;
                                    counter++;
                                    goto OneMoreItem;
                                }
                            }
                            //right
                            else if (iA == 1 && iterateAxis[iA])
                            {
                                g.DrawLine(new Pen(backGround, lineWidth), currentX, currentY, currentX + lineLen, currentY);
                                pointsOfImage.Add(new CustomPoint(currentX, currentY, currentX + lineLen, currentY));
                                currentX += lineLen;
                                if (currentX > mainImage.Width)
                                {
                                    lineLen = lineLen - 1 > 0 ? lineLen - 1 : 1;
                                    g.Clear(Color.Black);
                                    pointsOfImage = new List<CustomPoint>();
                                    initialCurrentX -= 100;
                                    currentX = initialCurrentX;
                                    counter++;
                                    goto OneMoreItem;
                                }
                            }
                            //down
                            else if (iA == 2 && iterateAxis[iA])
                            {
                                g.DrawLine(new Pen(backGround, lineWidth), currentX, currentY, currentX, currentY + lineLen);
                                pointsOfImage.Add(new CustomPoint(currentX, currentY, currentX, currentY + lineLen));
                                currentY += lineLen;
                                if (currentY > mainImage.Height)
                                {
                                    lineLen = lineLen - 1 > 0 ? lineLen - 1 : 1;
                                    g.Clear(Color.Black);
                                    pointsOfImage = new List<CustomPoint>();
                                    initialCurrentY -= 100;
                                    currentY = initialCurrentY;
                                    counter++;
                                    goto OneMoreItem;
                                }
                            }
                            //left
                            else if (iA == 3 && iterateAxis[iA])
                            {
                                g.DrawLine(new Pen(backGround, lineWidth), currentX, currentY, currentX - lineLen, currentY);
                                pointsOfImage.Add(new CustomPoint(currentX, currentY, currentX - lineLen, currentY));
                                currentX -= lineLen;
                                if (currentX < 0)
                                {
                                    lineLen = lineLen - 1 > 0 ? lineLen - 1 : 1;
                                    g.Clear(Color.Black);
                                    pointsOfImage = new List<CustomPoint>();
                                    initialCurrentX += 100;
                                    currentX = initialCurrentX;
                                    counter++;
                                    goto OneMoreItem;
                                }
                            }
                        }
                    }
                    else if (initialString[i] == '+')
                    {
                        changeAxis(false);
                    }
                    else if (initialString[i] == '-')
                    {
                        changeAxis(true);
                    }
                }
            }

            var allPixels = PixelOfGivenColor(mainImage, backGround);

            Point leftPixel = allPixels.OrderBy(a => a.X).First();
            Point rightPixel = allPixels.OrderByDescending(a => a.X).First();
            Point topPixel = allPixels.OrderBy(a => a.Y).First();
            Point bottomPixel = allPixels.OrderByDescending(a => a.Y).First();

            //float distance1 = distanceBetweenTwoPoints(leftPixel, rightPixel);
            //float distance2 = distanceBetweenTwoPoints(bottomPixel, topPixel);
            float distance1 = (rightPixel.X - leftPixel.X) + 40;
            float distance2 = (bottomPixel.Y - topPixel.Y) + 40;
            int position1 = leftPixel.X - 20;
            int position2 = topPixel.Y - 20;


            Bitmap newImg1 = new Bitmap(Convert.ToInt32(distance1), Convert.ToInt32(distance2));
            Rectangle rectToPlace = new Rectangle(position1, position2, Convert.ToInt32(distance1), Convert.ToInt32(distance2));

            using (Graphics zxc = Graphics.FromImage(newImg1))
            {
                zxc.DrawImage(mainImage, new Rectangle(0, 0, newImg1.Width, newImg1.Height), rectToPlace, GraphicsUnit.Pixel);
            }

            //mainImage.Save(@"C:\Users\Ragnus\source\repos\ConsoleApp2\ConsoleApp2\" + cc.ToString() + ".jpg");
            newImg1 = ResizeImage(newImg1, 500, 500);
            return newImg1;
        }
       


        //up,right,down,left
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private static List<Point> PixelOfGivenColor(Bitmap scrBitmap, Color c)
        {
            Color actualColor;
            List<Point> yos = new List<Point>();

            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = scrBitmap.GetPixel(i, j);

                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.ToArgb() == c.ToArgb())
                        yos.Add(new Point(i, j));
                }
            }

            return yos;
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        private static void changeAxis(bool nextAxis)
        {
            for (int i = 0; i < iterateAxis.Length; i++)
            {
                if (iterateAxis[i] && nextAxis)
                {
                    iterateAxis[i] = false;
                    if (i + 1 >= iterateAxis.Length)
                        iterateAxis[0] = true;
                    else
                        iterateAxis[i + 1] = true;

                    break;
                }
                else if (iterateAxis[i] && !nextAxis)
                {
                    iterateAxis[i] = false;
                    if (i == 0)
                        iterateAxis[iterateAxis.Length - 1] = true;
                    else
                        iterateAxis[i - 1] = true;

                    break;
                }
            }
        }
        private static Matrix GetMatrixFitRectInBounds(RectangleF fitrect, RectangleF boundsrect)
        {
            var T = new Matrix();

            var bounds_center = new PointF(boundsrect.Width / 2, boundsrect.Height / 2);

            //Set translation centerpoint
            T.Translate(bounds_center.X, bounds_center.Y);

            //Get smallest size to scale to fit boundsrect
            float scale = Math.Min(boundsrect.Width / fitrect.Width, boundsrect.Height / fitrect.Height);

            T.Scale(scale, scale);

            //Move fitrect to center of boundsrect
            T.Translate(bounds_center.X - fitrect.X - fitrect.Width / 2f, bounds_center.Y - fitrect.Y - fitrect.Height / 2f);

            //Restore translation point
            T.Translate(-bounds_center.X, -bounds_center.Y);

            return T;

        }
        private static void InitializeRulesAndAxioms()
        {
            axiomAndRules = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("F","F+F−F−F+F"),
                new KeyValuePair<string, string>("F-F-F-F","F+FF-FF-F-F+F+F"),
                new KeyValuePair<string, string>("-F","F+F-F-F+F"),
                new KeyValuePair<string, string>("F-F-F-F","FF-F-F-F-F-F+F"),
                new KeyValuePair<string, string>("F-F-F-F","FF-F-F-F-FF"),
                new KeyValuePair<string, string>("F-F-F-F","FF-F+F-F-FF"),
                new KeyValuePair<string, string>("F-F-F-F","FF-F--F-F"),
                new KeyValuePair<string, string>("F-F-F-F","F-FF--F-F"),
                new KeyValuePair<string, string>("F-F-F-F","F-F+F-F-F"),
                new KeyValuePair<string, string>("F+F+F+F","F+F-F-FF+F+F-F"),
                new KeyValuePair<string, string>("F+F+F+F","FF+F-F+F+FF"),
            };
        }
        private static string GenerateContent(string initialString, int expLength, Random rand)
        {
            //Generate Axiom
            while (initialString.Length < 1)
            {
                //Initialize first character of string
                if (rand.NextDouble() <= 0.5)
                    initialString = "F";
                else if (rand.NextDouble() <= 0.5)
                    initialString = "-";
                else if (rand.NextDouble() <= 0.5)
                    initialString = "+";
            }
            while (initialString.Length < expLength)
            {
                switch (initialString[initialString.Length - 1])
                {
                    case 'F':
                        if (rand.NextDouble() <= 0.45)
                            initialString += '-';
                        if (rand.NextDouble() <= 0.45)
                            initialString += '+';
                        break;
                    case '+':
                    case '-':
                        if (rand.NextDouble() <= 0.8)
                            initialString += 'F';
                        else if (rand.NextDouble() <= 0.1)
                            initialString += '-';
                        else if (rand.NextDouble() <= 0.1)
                            initialString += '+';
                        break;
                }
            }
            return initialString;

        }
        struct CustomPoint
        {
            public float x1;
            public float x2;
            public float y1;
            public float y2;

            public CustomPoint(float x1, float x2, float y1, float y2)
            {
                this.x1 = x1;
                this.x2 = x2;
                this.y1 = y1;
                this.y2 = y2;
            }
        }
    }
}