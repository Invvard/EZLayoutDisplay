namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class KeyTemplate
    {
        public double BaseX { get; set; }
        public double BaseY { get; set; }
        public double TopX => BaseX + 3;
        public double TopY => BaseY + 3;
        public double BaseWidth { get; set; }
        public double BaseHeight { get; set; }
        public double TopWidth => BaseWidth - 6;
        public double TopHeight => BaseHeight - 11;
        public double VerticalOffset { get; set; }
        public double HorizontalOffset { get; set; }
        public int Rotate { get; set; }
        public bool IsTopRightRotationOrigin { get; set; }
        public bool IsGlowing { get; set; }
    }
}