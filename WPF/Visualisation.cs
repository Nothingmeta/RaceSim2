using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace WPF
{
    public static class Visualisation
    {

        #region graphics
        /*
        private static readonly string _start = ".\\Images\\Track\\Start.png";
        private static readonly string _finish = ".\\Images\\Track\\Finish.png";
        private static readonly string _corner = ".\\Images\\Track\\Corner.png";
        private static readonly string _straight = ".\\Images\\Track\\Straight.png";
        private static readonly string _redbull = ".\\Images\\Cars\\Cars\\Redbull.png";
        private static readonly string _aston = ".\\Images\\Cars\\Cars\\Aston.png";
        private static readonly string _mclaren = ".\\Images\\Cars\\Cars\\Mclaren.png";
        private static readonly string _ferrari = ".\\Images\\Cars\\Cars\\Ferrari.png";
        private static readonly string _blueBroken = ".\\Images\\Cars\\Broken\\Redbull.png";
        private static readonly string _greenBroken = ".\\Images\\Cars\\Broken\\Aston.png";
        private static readonly string _orangeBroken = ".\\Images\\Cars\\Broken\\Mclaren.png";
        private static readonly string _redBroken = ".\\Images\\Cars\\Broken\\Ferrari.png";
        */

        private static readonly string _start = @"Images\Track\Start.png";
        private static readonly string _finish = @"Images\Track\Finish.png";
        private static readonly string _corner = @"Images\Track\Corner.png";
        private static readonly string _straight = @"Images\Track\Straight.png";
        private static readonly string _redbull = @"Images\Cars\Redbull-transparent.png";
        private static readonly string _aston = @"Images\Cars\Aston-transparent.png";
        private static readonly string _mclaren = @"Images\Cars\Mclaren-transparent.png";
        private static readonly string _ferrari = @"Images\Cars\Ferrari-transparent.png";
        private static readonly string _blueBroken = @"Images\Cars\Broken\Redbull-broken.png";
        private static readonly string _greenBroken = @"Images\Cars\Broken\Aston-broken.png";
        private static readonly string _orangeBroken = @"Images\Cars\Broken\Mclaren-broken.png";
        private static readonly string _redBroken = @"Images\Cars\Broken\Ferrari-broken.png";
        #endregion

        private const int trackSizePx = 128;
        private static int _globalX, _globalY, minX, minY;
        public static BitmapSource DrawTrack(Model.Track track)
        {
            if (track != null)
            {
                
                CalculateWidthAndHeight(track);
                Bitmap noTrack = ImageCache.CreateEmptyBitmap(_globalX * trackSizePx, _globalY * trackSizePx);
                Bitmap emptyTrack = PutTrack(noTrack, track);
                Bitmap filledTrack = PutParticipants(emptyTrack, track);
                return ImageCache.CreateBitmapSourceFromGdiBitmap(filledTrack);
            }

            Bitmap finished = ImageCache.CreateEmptyBitmap(512, 512);
            return ImageCache.CreateBitmapSourceFromGdiBitmap(finished);
        }

        public static void CalculateWidthAndHeight(Model.Track track)
        {
            int compass = 1;
            int x, y, maxX, maxY;
            x = y = minX = minY = maxX = maxY = 0;
            foreach (Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.LeftCorner:
                        compass = (compass < 1) ? 3 : compass -= 1;
                        break;
                    case SectionTypes.RightCorner:
                        compass = (compass > 2) ? 0 : compass += 1;
                        break;
                }
                switch (compass)
                {
                    case 0:
                        y--;
                        break;
                    case 1:
                        x++;
                        break;
                    case 2:
                        y++;
                        break;
                    case 3:
                        x--;
                        break;
                }

                if (maxX < x)
                {
                    maxX = x;
                }
                else if (minX > x)
                {
                    minX = x;
                }

                if (maxY < y)
                {
                    maxY = y;
                }
                else if (minY > y)
                {
                    minY = y;
                }
            }
            _globalX = maxX - minX + 1;
            _globalY = maxY - minY + 1;
        }

        public static Bitmap PutTrack(Bitmap bitmap, Model.Track track)
        {

            int currentX, currentY;
            currentX = -minX;
            currentY = -minY;
            int compass = 1;
            Graphics g = Graphics.FromImage(bitmap);
            foreach (Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        Bitmap startGrid = new Bitmap(ImageCache.GetImgBitmap(_start));
                        g.DrawImage(RotateAsset(startGrid, compass, "straight"), new Point(currentX * trackSizePx, currentY * trackSizePx));
                        break;
                    case SectionTypes.Finish:
                        Bitmap finish = new Bitmap(ImageCache.GetImgBitmap(_finish));
                        g.DrawImage(RotateAsset(finish, compass, "straight"), new Point(currentX * trackSizePx, currentY * trackSizePx));
                        break;
                    case SectionTypes.LeftCorner:
                        Bitmap leftCorner = new Bitmap(ImageCache.GetImgBitmap(_corner));
                        g.DrawImage(RotateAsset(leftCorner, compass, "leftCorner"), new Point(currentX * trackSizePx, currentY * trackSizePx));
                        compass = (compass < 1) ? 3 : compass -= 1;
                        break;
                    case SectionTypes.RightCorner:
                        Bitmap rightCorner = new Bitmap(ImageCache.GetImgBitmap(_corner));
                        g.DrawImage(RotateAsset(rightCorner, compass, "rightCorner"), new Point(currentX * trackSizePx, currentY * trackSizePx));
                        compass = (compass > 2) ? 0 : compass += 1;
                        break;
                    case SectionTypes.Straight:
                        Bitmap straight = new Bitmap(ImageCache.GetImgBitmap(_straight));
                        g.DrawImage(RotateAsset(straight, compass, "straight"), new Point(currentX * trackSizePx, currentY * trackSizePx));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                switch (compass)
                {
                    case 0:
                        currentY--;
                        break;
                    case 1:
                        currentX++;
                        break;
                    case 2:
                        currentY++;
                        break;
                    case 3:
                        currentX--;
                        break;
                }
            }

            return bitmap;
        }

        public static Bitmap RotateAsset(Bitmap asset, int compass, string type)
        {
            switch (type)
            {
                case "straight":
                    switch (compass)
                    {
                        case 0:
                            asset.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            return asset;
                        case 1:
                            return asset;
                        case 2:
                            asset.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            return asset;
                        case 3:
                            asset.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            return asset;
                    }
                    break;
                case "leftCorner":
                    switch (compass)
                    {
                        case 0:
                            asset.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            return asset;
                        case 1:
                            asset.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            return asset;
                        case 2:
                            return asset;
                        case 3:
                            asset.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            return asset;
                    }
                    break;
                case "rightCorner":
                    switch (compass)
                    {
                        case 0:
                            asset.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            return asset;
                        case 1:
                            asset.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            return asset;
                        case 2:
                            asset.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            return asset;
                        case 3:
                            return asset;
                    }
                    break;
            }

            return asset;
        }
        public static Bitmap PutParticipants(Bitmap bitmap, Model.Track track)
        {

            int currentX, currentY;
            currentX = -minX;
            currentY = -minY;
            int compass = 1;
            Graphics g = Graphics.FromImage(bitmap);
            foreach (Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                    case SectionTypes.Finish:
                    case SectionTypes.Straight:
                        PrintCar(bitmap, section, currentX, currentY, compass);
                        break;
                    case SectionTypes.LeftCorner:
                        PrintCar(bitmap, section, currentX, currentY, compass);
                        compass = (compass < 1) ? 3 : compass -= 1;
                        break;
                    case SectionTypes.RightCorner:
                        PrintCar(bitmap, section, currentX, currentY, compass);
                        compass = (compass > 2) ? 0 : compass += 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                switch (compass)
                {
                    case 0:
                        currentY--;
                        break;
                    case 1:
                        currentX++;
                        break;
                    case 2:
                        currentY++;
                        break;
                    case 3:
                        currentX--;
                        break;
                }
            }

            return bitmap;
        }

        public static Bitmap PrintCar(Bitmap bitmap, Section section, int x, int y, int compass)
        {
            if (Data.CurrentRace != null)
            {
                Graphics g = Graphics.FromImage(bitmap);
                SectionData sd = Data.CurrentRace.GetSectionData(section);
                int xLeft, yLeft, xRight, yRight;
                xLeft = yLeft = xRight = yRight = 0;
                DeterminePos(ref xLeft, ref yLeft, ref xRight, ref yRight, compass);
                if (sd.Left != null)
                {
                    string leftColour = GetTeamColour(sd.Left.TeamColor, sd.Left.Equipment.IsBroken);
                    Bitmap leftCar = new Bitmap(ImageCache.GetImgBitmap(leftColour));
                    g.DrawImage(RotateAsset(leftCar, compass, "straight"), new Point(x * trackSizePx + xLeft, y * trackSizePx + yLeft));
                }
                if (sd.Right != null)
                {
                    string rightColour = GetTeamColour(sd.Right.TeamColor, sd.Right.Equipment.IsBroken);
                    Bitmap rightCar = new Bitmap(ImageCache.GetImgBitmap(rightColour));
                    g.DrawImage(RotateAsset(rightCar, compass, "straight"), new Point(x * trackSizePx + xRight, y * trackSizePx + yRight));
                }

                return bitmap;
            }
            else
            {
                return null;
            }
        }

        public static string GetTeamColour(TeamColors teamColor, bool brokenStatus)
        {
            return teamColor switch
            {
                TeamColors.Red => brokenStatus ? _redBroken : _ferrari,
                TeamColors.Green => brokenStatus ? _greenBroken : _aston,
                TeamColors.Blue => brokenStatus ? _blueBroken : _redbull,
                TeamColors.Orange => brokenStatus ? _orangeBroken : _mclaren,
                _ => throw new ArgumentOutOfRangeException(nameof(teamColor), teamColor, null)
            };
        }

        public static void DeterminePos(ref int xLeft, ref int yLeft, ref int xRight, ref int yRight, int compass)
        {
            switch (compass)
            {
                case 0:
                    xLeft = 20;
                    yLeft = 20;
                    xRight = 50;
                    yRight = 50;
                    break;
                case 1:
                    xLeft = 50;
                    yLeft = 20;
                    xRight = 20;
                    yRight = 50;
                    break;
                case 2:
                    xLeft = 50;
                    yLeft = 50;
                    xRight = 20;
                    yRight = 20;
                    break;
                case 3:
                    xLeft = 20;
                    yLeft = 50;
                    xRight = 50;
                    yRight = 20;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
