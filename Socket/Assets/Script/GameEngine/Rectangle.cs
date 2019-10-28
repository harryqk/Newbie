
namespace GameEngine
{
    public class Rectangle
    {
        public ICollision collision;
        public Rectangle(float x, float y, float w, float h)
        {
            if (origin == null)
            {
                origin = new Point();
            }
            if (center == null)
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
        public float width;
        public float height;


        /// <summary>
        /// 刷新坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdatePos(float x, float y)
        {
            origin.x = x;
            origin.y = y;
            center.x = origin.x + width / 2;
            center.y = origin.y + height / 2;
        }

        public Point center;


        public Rectangle[] Slice(Rectangle bound)
        {
            Rectangle[] horizontal = SliceHorizontal(bound);
            Rectangle[] vertical = SliceVertial(bound);
            if (horizontal != null
                && vertical != null)
            {
                Rectangle[] ret = new Rectangle[4];
                Rectangle[] sub = horizontal[0].SliceVertial(bound);
                Rectangle[] sub1 = horizontal[1].SliceVertial(bound);
                ret[0] = sub[0];
                ret[1] = sub[1];
                ret[2] = sub1[0];
                ret[3] = sub1[1];
                return ret;
            }
            else if (horizontal != null)
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

        public Rectangle[] SliceHorizontal(Rectangle bound)
        {
            if (origin.x < bound.center.x
                && origin.x + width > bound.center.x)
            {

                return SliceInTwoHorizontal(bound.center.x);
            }
            return null;
        }

        public Rectangle[] SliceInTwoHorizontal(float x)
        {
            Rectangle[] ret = new Rectangle[2];
            ret[0] = new Rectangle(origin.x, origin.y, x - origin.x, height);
            ret[1] = new Rectangle(x, origin.y, origin.x + width - x, height);
            return ret;
        }



        public Rectangle[] SliceVertial(Rectangle bound)
        {
            if (origin.y < bound.center.y
                && origin.y + height > bound.center.y)
            {

                return SliceInTwoVertical(bound.center.y);
            }
            return null;
        }

        Rectangle[] SliceInTwoVertical(float y)
        {
            Rectangle[] ret = new Rectangle[2];
            ret[0] = new Rectangle(origin.x, origin.y, width, y - origin.y);
            ret[1] = new Rectangle(origin.x, y, width, origin.y + height - y);
            return ret;
        }

    }

}
