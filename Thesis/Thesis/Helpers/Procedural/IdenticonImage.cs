using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;
using System;
using Thesis.Infrastructure;

namespace Thesis.Helpers.Procedural
{
    public class IdenticonImage
    {
        ///https://stackoverflow.com/questions/17208254/how-to-change-pixel-color-of-an-image-in-c-net
        public static Bitmap ChangeColor(Bitmap scrBitmap, Color newColorToApply)
        {
            Color actualColor;
            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = scrBitmap.GetPixel(i, j);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.A > 150)
                        newBitmap.SetPixel(i, j, newColorToApply);
                    else
                        newBitmap.SetPixel(i, j, actualColor);
                }
            }

            return newBitmap;
        }

        public static Bitmap GenerateSimpleImage(string email)
        {
            //Prepare main image 
            Bitmap mainImage = new Bitmap(100, 100);
            Graphics newGraphics = Graphics.FromImage(mainImage);
            newGraphics.Clear(Color.White); //Set background color to white




            //create hash
            string nameOfUser = email;
            string hash = "";
            using (MD5 md5 = MD5.Create())
            {
                hash = GetMd5Hash(md5, nameOfUser);
            }

            List<int> hashedValues = new List<int>();

            for (int i = 6; i < 30; i++)
            {
                if (hashedValues.Count == 16) break;
                hashedValues.Add(int.Parse(hash.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
            }


            //Create Main colour for pixels
            var color1 = int.Parse(hash.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var color2 = int.Parse(hash.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var color3 = int.Parse(hash.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            Color newColor = Color.FromArgb(color1, color2, color3);

            // Get Pixel
            Image imageFile = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/forIdenticon/16.png"));
            Graphics newGraphics1 = Graphics.FromImage(imageFile);
            newGraphics1.Clear(newColor); //Set background color to white


            using (Graphics g = Graphics.FromImage(mainImage))
            {
                int vertical = 0;
                int horizontal = 0;

                hashedValues.ForEach(a =>
                {
                    if (a % 2 == 0 && horizontal < 60)
                    {
                        g.DrawImage(imageFile, horizontal, vertical);
                        if (horizontal != 60)
                            g.DrawImage(imageFile, 80 - horizontal, vertical);
                    }

                    if (horizontal >= 60)
                    {
                        vertical += 20;
                        horizontal = 0;
                    }
                    else
                        horizontal += 20;

                });
            }

            return mainImage;
        }

        public static Bitmap GenerateCustomImage(string email)
        {
            //Prepare main image 
            Bitmap mainImage = new Bitmap(60, 60);
            Graphics newGraphics = Graphics.FromImage(mainImage);
            newGraphics.Clear(Color.White); //Set background color to white

            for (int czo = 0; czo < 100; czo++)
            {
                //create hash
                string nameOfUser = email + czo.ToString();
                string hash = "";
                using (MD5 md5 = MD5.Create())
                {
                    hash = GetMd5Hash(md5, nameOfUser);
                }
                var source = hash.GetHashCode();


                int centerindex = source & 3; // 2 lowest bits
                int sideindex = (source >> 2) & 15; // next 4 bits for side shapes
                int cornerindex = (source >> 6) & 15; // next 4 for corners
                int siderot = (source >> 10) & 3; // 2 bits for side offset rotation
                int cornerrot = (source >> 12) & 3; // 2 bits for corner offset rotation

                //Make sure that rotation of side images is diffrent from corent images
                if (siderot == cornerrot)
                {
                    cornerrot++;
                    if (cornerrot > 16)
                    {
                        cornerrot -= 2;
                    }
                }

                //Create Main colour for pixels
                var color1 = int.Parse(hash.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                var color2 = int.Parse(hash.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                var color3 = int.Parse(hash.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                Color newColor = Color.FromArgb(color1, color2, color3);

                var imagesToPlace = new Bitmap[16];
                var centerImages = new Bitmap[4];

                int countForCenter = 0;
                var folderWithIcons = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/forIdenticon/");

                for (int i = 1; i <= 16; i++)
                {

                    Bitmap imageFile = (Bitmap)Image.FromFile(folderWithIcons + i.ToString() + ".png");
                    imageFile = ChangeColor(imageFile, newColor);
                    imagesToPlace[i - 1] = imageFile;

                    if (i == 1 || i == 5 || i == 9 || i == 16)
                    {
                        centerImages[countForCenter] = imageFile;
                        countForCenter++;
                    }
                }


                using (Graphics g = Graphics.FromImage(mainImage))
                {
                    int centerHor = 20;
                    int centerVer = 20;
                    g.DrawImage(centerImages[centerindex], centerVer, centerHor);


                    var sideImage = imagesToPlace[sideindex];
                    sideImage = InitalizeFirstRotation(sideImage, siderot);
                    g.DrawImage(sideImage, 20, 0);
                    var sideImageNextRot = new Bitmap(sideImage);
                    sideImageNextRot.RotateFlip(RotateFlipType.Rotate90FlipNone); //Rotate image to right
                    g.DrawImage(sideImageNextRot, 40, 20);
                    sideImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(sideImage, 20, 40);
                    sideImageNextRot.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(sideImageNextRot, 0, 20);


                    var cornerImage = imagesToPlace[cornerindex];
                    cornerImage = InitalizeFirstRotation(cornerImage, cornerrot);
                    g.DrawImage(cornerImage, 0, 0);



                    var cornerImageNextRot = new Bitmap(cornerImage);
                    cornerImageNextRot.RotateFlip(RotateFlipType.Rotate90FlipNone); //Rotate image to right
                    g.DrawImage(cornerImageNextRot, 40, 0);


                    cornerImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(cornerImage, 40, 40);

                    cornerImageNextRot.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(cornerImageNextRot, 0, 40);
                }
            }

            return mainImage;
        }

        public static Bitmap GenerateNormalImage(string email)
        {
            //Prepare main image 
            Bitmap mainImage = new Bitmap(60, 60);
            Graphics newGraphics = Graphics.FromImage(mainImage);
            newGraphics.Clear(Color.White); //Set background color to white


            //create hash
            string nameOfUser = email;
            string hash = "";
            using (MD5 md5 = MD5.Create())
            {
                hash = GetMd5Hash(md5, nameOfUser);
            }
            var source = hash.GetHashCode();


            int centerindex = source & 3; // 2 lowest bits
            int sideindex = (source >> 2) & 15; // next 4 bits for side shapes
            int cornerindex = (source >> 6) & 15; // next 4 for corners
            int siderot = (source >> 10) & 3; // 2 bits for side offset rotation
            int cornerrot = (source >> 12) & 3; // 2 bits for corner offset rotation

            //Make sure that rotation of side images is diffrent from corent images
            if (siderot == cornerrot)
            {
                cornerrot++;
                if (cornerrot > 16)
                {
                    cornerrot -= 2;
                }
            }

            //Create Main colour for pixels
            var color1 = int.Parse(hash.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var color2 = int.Parse(hash.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var color3 = int.Parse(hash.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            Color newColor = Color.FromArgb(color1, color2, color3);

            var imagesToPlace = new Bitmap[16];
            var centerImages = new Bitmap[4];

            int countForCenter = 0;
            var folderWithIcons = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/forIdenticon/");
            for (int i = 1; i <= 16; i++)
            {
                Bitmap imageFile = (Bitmap)Image.FromFile(folderWithIcons + i.ToString() + ".png");
                imageFile = ChangeColor(imageFile, newColor);
                imagesToPlace[i - 1] = imageFile;

                if (i == 1 || i == 5 || i == 9 || i == 16)
                {
                    centerImages[countForCenter] = imageFile;
                    countForCenter++;
                }
            }


            using (Graphics g = Graphics.FromImage(mainImage))
            {
                int centerHor = 20;
                int centerVer = 20;
                g.DrawImage(centerImages[centerindex], centerVer, centerHor);


                var sideImage = imagesToPlace[sideindex];
                sideImage = InitalizeFirstRotation(sideImage, siderot);
                g.DrawImage(sideImage, 20, 0);
                var sideImageNextRot = new Bitmap(sideImage);
                sideImageNextRot.RotateFlip(RotateFlipType.Rotate90FlipNone); //Rotate image to right
                g.DrawImage(sideImageNextRot, 40, 20);
                sideImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                g.DrawImage(sideImage, 20, 40);
                sideImageNextRot.RotateFlip(RotateFlipType.Rotate180FlipNone);
                g.DrawImage(sideImageNextRot, 0, 20);


                var cornerImage = imagesToPlace[cornerindex];
                cornerImage = InitalizeFirstRotation(cornerImage, cornerrot);
                g.DrawImage(cornerImage, 0, 0);



                var cornerImageNextRot = new Bitmap(cornerImage);
                cornerImageNextRot.RotateFlip(RotateFlipType.Rotate90FlipNone); //Rotate image to right
                g.DrawImage(cornerImageNextRot, 40, 0);


                cornerImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                g.DrawImage(cornerImage, 40, 40);

                cornerImageNextRot.RotateFlip(RotateFlipType.Rotate180FlipNone);
                g.DrawImage(cornerImageNextRot, 0, 40);
            }


            return mainImage;
        }

        private static Bitmap InitalizeFirstRotation(Bitmap tmp, int siderot)
        {
            for (int i = 0; i < siderot; i++)
            {
                tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            return tmp;
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
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
    }
}