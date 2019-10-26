
namespace GameEngine
{
    public class Rectangle
    {
        public ICollision collision;
        public Rectangle(int x, int y, int w, int h)
        {
            if(origin == null)
            {
                origin = new Point();
            }
            if(center == null)
            {
                center = new Point();
            }
            origin.x = x;
            origin.y = y;
            width = w;
            height = h;
            center.x = origin.x + w / 2;
            center.y = origin.y + h / 2;
        }
        public Point origin;//left bottom
        public int width;
        public int height;


        /// <summary>
        /// 刷新坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void updatePos(int x, int y)
        {
            origin.x = x;
            origin.y = y;
            center.x = origin.x + width / 2;
            center.y = origin.y + height / 2;
        }

        public Point center;

        /// <summary>
        /// 将矩形对半切割
        /// </summary>
        /// <returns></returns>
        public Rectangle[] slice()
        {
            int sWidth = width / 2;
            int sHeight = height / 2;
            int col = 1;
            int raw = 1;

            if (sWidth < 1)
            {
                sWidth = 1;
                col = 1;
            }
            else
            {
                col = 2;
            }

            if (sHeight < 1)
            {
                sHeight = 1;
                raw = 1;
            }
            else
            {
                raw = 2;
            }
            Rectangle[] rectangles = new Rectangle[col * raw];
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < raw; j++)
                {
                    rectangles[col * i + raw * j] = new Rectangle(origin.x + sWidth * (i + 1), origin.y + sHeight * (j + 1), sWidth, sHeight);
                }
            }
            return rectangles;
        }
    }

}
