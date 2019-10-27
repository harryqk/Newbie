
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
                    rectangles[col * i + j] = new Rectangle(origin.x + sWidth * (i + 1), origin.y + sHeight * (j + 1), sWidth, sHeight);
                }
            }



            return rectangles;
        }

        public Rectangle[] slice(Rectangle bound)
        {
            Rectangle[] horizontal = sliceHorizontal(bound);
            Rectangle[] vertical = sliceVertial(bound);
            if (horizontal != null
                && vertical != null)
            {
                Rectangle[] ret = new Rectangle[4];
                Rectangle[] sub = horizontal[0].sliceVertial(bound);
                Rectangle[] sub1 = horizontal[1].sliceVertial(bound);
                ret[0] = sub[0];
                ret[1] = sub[1];
                ret[2] = sub1[0];
                ret[3] = sub1[1];
                return ret;
            }
            else if(horizontal != null)
            {
                return horizontal;
            }
            else if (vertical != null)
            {
                return vertical;
            }
            Rectangle[] me = new Rectangle[1];
            me[0] = this;
            return me;
        }

        public Rectangle[] sliceHorizontal(Rectangle bound)
        {
            if(origin.x < bound.center.x
                && origin.x + width > bound.center.x)
            {

                return sliceInTwoHorizontal(bound.center.x);
            }
            return null;
        }

        public Rectangle[] sliceInTwoHorizontal(int x) 
        {
            Rectangle[] ret = new Rectangle[2];
            ret[0] = new Rectangle(origin.x, origin.y, x - origin.x, height);
            ret[1] = new Rectangle(x, origin.y, origin.x + width - x, height);
            return ret;
        }



        public Rectangle[] sliceVertial(Rectangle bound)
        {
            if (origin.y < bound.center.y
                && origin.y + height > bound.center.y)
            {

                return sliceInTwoVertical(bound.center.y);
            }
            return null;
        }

        Rectangle[] sliceInTwoVertical(int y)
        {
            Rectangle[] ret = new Rectangle[2];
            ret[0] = new Rectangle(origin.x, origin.y, width, y - origin.y);
            ret[1] = new Rectangle(origin.x, y, width, origin.y + height - y);
            return ret;
        }

    }

}
