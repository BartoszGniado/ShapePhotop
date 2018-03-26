using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapePhoto
{
    class Aprox
    {
        List<Rectangle> rectangles;
        List<SolidBrush> brushes;
        int ImageWidth;
        int ImageHeight;
        public Bitmap madeImage;
        public int score;
        public int parentID;
        int ShapeNum;
		Random r;
       public int Generation;
        public Aprox(Aprox apx)
        {
            score = apx.score;
            Generation = apx.Generation;
            rectangles = new List<Rectangle>();
            brushes = new List<SolidBrush>();
            r = new Random(apx.r.Next());
            this.parentID = apx.parentID;
            ShapeNum = apx.ShapeNum;
            ImageWidth = apx.ImageWidth;
            ImageHeight = apx.ImageHeight;
            madeImage = new Bitmap(apx.madeImage);
            // rectangles =apx.rectangles;
            rectangles.Clear();
            foreach (Rectangle re in apx.rectangles)
                rectangles.Add(new Rectangle(re.X, re.Y, re.Width, re.Height));
            //    brushes = apx.brushes;
            brushes.Clear();
            foreach (SolidBrush sb in apx.brushes)
                brushes.Add(new SolidBrush(sb.Color));

        }

        public Aprox(int ShapeNum,Image orginal, int seed, int id)
        {
            Generation = 0;
			r=new Random(seed);
            this.parentID = id;
            this.ShapeNum = ShapeNum;
            ImageWidth = orginal.Width;
            ImageHeight = orginal.Height;
            madeImage = new Bitmap(ImageWidth, ImageHeight);
            rectangles = new List<Rectangle>();
            brushes = new List<SolidBrush>();
            int i; 
            rectangles.Add(new Rectangle(0, 0,ImageWidth,ImageHeight));
            brushes.Add(new SolidBrush(Color.FromArgb(r.Next(255), r.Next(255), r.Next(255))));
            for (i = 0; i < ShapeNum; i++)
            {
                int sw = r.Next(ImageWidth);
                int sh = r.Next(ImageHeight);
                rectangles.Add(new Rectangle(sw, sh, r.Next(Math.Min(ImageWidth - sw, ImageWidth / 7)), r.Next(Math.Min(ImageHeight - sh, ImageHeight / 7))));
                brushes.Add(new SolidBrush(Color.FromArgb(r.Next(255), r.Next(255), r.Next(255))));
                using (Graphics g = Graphics.FromImage(madeImage))
                {
                    g.FillRectangle(brushes[i], rectangles[i]);
                }
            }

            SetScore(orginal);
        }
       public Aprox(Aprox apx, Image orginal, int seed,int id)
        {
            Generation = apx.Generation++;
            rectangles = new List<Rectangle>();
            brushes = new List<SolidBrush>();
            r =new Random(seed);
            this.parentID = id;
            ShapeNum = apx.ShapeNum;
            ImageWidth = apx.ImageWidth;
            ImageHeight = apx.ImageHeight;
            madeImage = new Bitmap(ImageWidth, ImageHeight);
           // rectangles =apx.rectangles;
            rectangles.Clear();
            foreach (Rectangle re in apx.rectangles)
                rectangles.Add(new Rectangle(re.X,re.Y,re.Width,re.Height));
        //    brushes = apx.brushes;
            brushes.Clear();
            foreach (SolidBrush sb in apx.brushes)
                brushes.Add(new SolidBrush(sb.Color));

            // modify
            int k;
            for (k = 0; k < 1 + r.Next(3); k++)
            {
                double tmp = r.NextDouble();
                if (tmp < 0.33 || tmp > 0.98)
                {
                    int IDMod = r.Next(ShapeNum - 1) + 1;
                    if (r.NextDouble() > 0.2)
                    {
                        rectangles[IDMod] = ChangeRectangle(rectangles[IDMod], ImageWidth, ImageHeight, r);
                    }
                    else
                    {
                        int sw = r.Next(ImageWidth);
                        int sh = r.Next(ImageHeight);
                        rectangles[IDMod] = new Rectangle(sw, sh, r.Next(Math.Min(ImageWidth - sw, ImageWidth/ 7)), r.Next(Math.Min(ImageHeight - sh, ImageHeight/ 7)));
                    }
                }
                if (tmp < 0.66 && tmp > 0.3)
                {
                    int IDMod = r.Next(ShapeNum);
                    if (r.NextDouble() > 0.8)
                    {
                        brushes[IDMod].Color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
                    }
                    else
                    {
                        brushes[IDMod].Color = ChangeColour(brushes[IDMod].Color, r);
                    }
                }
                if (tmp > 0.6 || tmp < 0.02)
                {
                    int IDMod1 = r.Next(ShapeNum - 1) + 1;
                    int IDMod2 = r.Next(ShapeNum - 1) + 1;
                    Rectangle tempR = rectangles[IDMod1];
                    SolidBrush tempB = brushes[IDMod1];
                    rectangles[IDMod1] = rectangles[IDMod2];
                    brushes[IDMod1] = brushes[IDMod2];
                    rectangles[IDMod2] = tempR;
                    brushes[IDMod2] = tempB;
                }
            }

            int i; 
            for (i = 0; i < ShapeNum; i++)
            {
          //      int sw = r.Next(ImageWidth);
          //      int sh = r.Next(ImageHeight);
          //      rectangles.Add(new Rectangle(sw, sh, r.Next(ImageWidth - sw), r.Next(ImageHeight - sh)));
          //      brushes.Add(new SolidBrush(Color.FromArgb(r.Next(255), r.Next(255), r.Next(255))));
                using (Graphics g = Graphics.FromImage(madeImage))
                {
                    g.FillRectangle(brushes[i], rectangles[i]);
                }
            }
            SetScore(orginal);
        }

        private Color ChangeColour(Color color, Random rr)
        {
            Color colour =color;
            
            int change;
            double rnd = r.NextDouble();
            if (rnd < 0.333||rnd>0.98)
            {

                do {
                    change = (int)(r.NextDouble() + 0.5) * color.R;
                }while(change>255);
                colour = Color.FromArgb(change, color.G, color.B);
            }
            if(rnd<0.666&&rnd>0.3)
            {
                do
                {
                    change = (int)(r.NextDouble() + 0.5) * color.G;
                } while (change > 255);
                colour = Color.FromArgb(color.R, change, color.B);
            }
             if(rnd>0.6||rnd<0.02)
            {
                do
                {
                    change = (int)(r.NextDouble() + 0.5) * color.B;
                } while (change > 255);
                colour = Color.FromArgb(color.R, color.G, change);
            }
            return colour;
        }

        private Rectangle ChangeRectangle(Rectangle rectangle, int ImageWidth, int ImageHeight, Random rr)
        {
            Rectangle rec = rectangle;
            double tmp=r.NextDouble();
            if (tmp > 0.3)
            {
                if (r.NextDouble() > 0.5)
                {
                    if (r.NextDouble() > 0.5)
                    {
                        do
                        {
                            rec.Height = (int)Math.Floor(rec.Height * (r.NextDouble() + 0.5));
                        } while (rec.Height + rec.Y > ImageHeight);
                    }
                    else
                    {
                        do
                        {
                            int change = (int)Math.Floor(rec.Height * (r.NextDouble() + 0.5));
                            if (r.NextDouble() > 0.5)
                            {
                                rec.Height += change;
                                rec.Y -= change;
                            }
                            else
                            {
                                rec.Height -= change;
                                rec.Y += change;
                            }
                        } while (rec.Height + rec.Y > ImageHeight && rec.Y>0);
                    }
                }
                else
                {
                    if (r.NextDouble() > 0.5)
                    {
                        do
                        {
                            rec.Width = (int)Math.Floor(rec.Width * (r.NextDouble() + 0.5));
                        } while (rec.Width + rec.X > ImageWidth);
                    }
                    else
                    {
                        do
                        {
                            int change = (int)Math.Floor(rec.Width * (r.NextDouble() + 0.5));
                            if (r.NextDouble() > 0.5)
                            {
                                rec.Width += change;
                                rec.X -= change;
                            }
                            else
                            {
                                rec.Width -= change;
                                rec.X += change;
                            }
                        } while (rec.Width + rec.X > ImageWidth && rec.X>0);
                    }
                }
            }
            if (tmp < 0.33)
            {
                if (r.NextDouble() > 0.5)
                {
                    do
                    {
                        rec.X= (int)Math.Floor(rec.X * (r.NextDouble() + 0.5));
                    } while (rec.Width + rec.X > ImageWidth);
                }
                else
                {
                    do
                    {
                        rec.Y = (int)Math.Floor(rec.Y * (r.NextDouble() + 0.5));
                    } while (rec.Height + rec.Y > ImageHeight);
                }
            }
            return rec;
        }

        private void SetScore(Image orginal)
        {
            score = 0;
            int w, h;
            for (w = 0; w < ImageWidth; w += 5)
                for (h = 0; h < ImageHeight; h += 5)
                {
                    Color px1 = madeImage.GetPixel(w, h);
                    Color px2 = ((Bitmap)orginal).GetPixel(w, h);
                    score += Math.Abs(px2.R - px1.R);
                    score += Math.Abs(px2.G - px1.G);
                    score += Math.Abs(px2.B - px1.B);
                }
        }
        //public Aprox(Image orginal)
        //{
        //    ImageWidth = orginal.Width;
        //    ImageHeight = orginal.Height;
        //    madeImage = new Bitmap(orginal);
            
        //    score = 0;
        //    int w, h;
        //    for (w = 0; w < ImageWidth; w += 3)
        //        for (h = 0; h < ImageHeight; h += 2)
        //        {
        //            Color px1 = madeImage.GetPixel(w, h);
        //            Color px2 = ((Bitmap)orginal).GetPixel(w, h);
        //            score += Math.Abs(px2.R - px1.R);
        //            score += Math.Abs(px2.G - px1.G);
        //            score += Math.Abs(px2.B - px1.B);
        //        }
        //}
    }
}
