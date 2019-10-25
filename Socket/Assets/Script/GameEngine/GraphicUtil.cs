namespace GameEngine
{
    public static class GraphicUtil
    {
        //矩形rect是否在边界bound内
        public static bool isInner(Rectangle rect, Rectangle bound)
        {
            return rect.origin.x >= bound.origin.x &&
                rect.origin.x + rect.width <= bound.origin.x + bound.width &&
                rect.origin.y >= bound.origin.y &&
                rect.origin.y + rect.height <= bound.origin.y + bound.height;
        }
    }
}
