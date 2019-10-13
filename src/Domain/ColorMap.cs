namespace Domain
{
    public interface IColorMap
    {
        FaceColor MapX(int x);
        FaceColor MapY(int y);
        FaceColor MapZ(int z);
    }
    
    internal class ColorMap : IColorMap
    {   
        public FaceColor MapX(int x)
        {
            if (x == 0)
                return FaceColor.None;

            return x > 0 ? FaceColor.Red : FaceColor.Blue;
        }

        public FaceColor MapY(int y)
        {
            if (y == 0)
                return FaceColor.None;

            return y > 0 ? FaceColor.Green : FaceColor.White;
        }

        public FaceColor MapZ(int z)
        {
            if (z == 0)
                return FaceColor.None;

            return z > 0 ? FaceColor.Orange : FaceColor.Yellow;
        }
    }
}