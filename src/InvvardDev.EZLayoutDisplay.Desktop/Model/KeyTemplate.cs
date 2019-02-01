namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class KeyTemplate
    {
        private const int KeyUnitSize = 54;

        public double BaseRelativeLeft { get; set; }
        public double BaseRelativeTop { get; set; }
        public double BaseRelativeWidth { get; set; }
        public double BaseRelativeHeight { get; set; }
        public double VerticalOffset { get; set; }
        public double HorizontalOffset { get; set; }
        public int Rotate { get; set; }
        public bool IsTopRightRotationOrigin { get; set; }
        public bool IsGlowing { get; set; }

        public double BaseKeyX => HorizontalOffset * KeyUnitSize + BaseRelativeLeft * KeyUnitSize;
        public double BaseKeyY => VerticalOffset * KeyUnitSize + BaseRelativeTop * KeyUnitSize;
        public double TopKeyX => BaseKeyX + 3;
        public double TopKeyY => BaseKeyY + 3;

        public double BaseKeyWidth => BaseRelativeWidth * KeyUnitSize;
        public double BaseKeyHeight => BaseRelativeHeight * KeyUnitSize;
        public double TopKeyWidth => BaseKeyWidth - 6;
        public double TopKeyHeight => BaseKeyHeight - 11;

        public KeyTemplate()
        {
            
        }

        public KeyTemplate(double x, double y, double width = 1, double height = 1, double vOffset = 0, double hOffset = 0, int rotate = 0, bool isOriginTopRight = false, bool isGlowing = false)
        {
            BaseRelativeLeft = x;
            BaseRelativeTop = y;
            BaseRelativeWidth = width;
            BaseRelativeHeight = height;
            VerticalOffset = vOffset;
            HorizontalOffset = hOffset;
            Rotate = rotate;
            IsTopRightRotationOrigin = isOriginTopRight;
            IsGlowing = isGlowing;
        }
    }
}