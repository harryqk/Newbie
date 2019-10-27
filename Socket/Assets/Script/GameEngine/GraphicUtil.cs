using UnityEngine;

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

        //矩形是否相交
        public static bool isOverlap(Rectangle a, Rectangle b)
        {
            return !(a.origin.x + a.width < b.origin.x
                || a.origin.x > b.origin.x + b.width
                || a.origin.y + a.height < b.origin.y
                || a.origin.y > b.origin.y + b.height);
        }

        /// <summary>
        /// Draws the quard tree.
        /// </summary>
        /// <param name="tree">Tree.</param>
        public static void drawQuardTree(QuardTree tree)
        {
            Debug.DrawLine(new Vector3(tree.bound.origin.x, tree.bound.origin.y, 0), new Vector3(tree.bound.origin.x + tree.bound.width, tree.bound.origin.y, 0));
            Debug.DrawLine(new Vector3(tree.bound.origin.x, tree.bound.origin.y, 0), new Vector3(tree.bound.origin.x, tree.bound.origin.y + tree.bound.height, 0));
            if(tree.trees.Count > 0)
            {
                for(int i = 0; i < tree.trees.Count; i++)
                {
                    drawQuardTree(tree.trees[i]);
                }
            }
        }

        public static void drawRect(Rectangle rect) 
        {
            Vector3 v0 = new Vector3(rect.origin.x, rect.origin.y, 0);
            Vector3 v1 = new Vector3(rect.origin.x + rect.width, rect.origin.y, 0);
            Vector3 v2 = new Vector3(rect.origin.x + rect.width, rect.origin.y + rect.height, 0);
            Vector3 v3 = new Vector3(rect.origin.x, rect.origin.y + rect.height, 0);

            Debug.DrawLine(v0, v1, Color.green);
            Debug.DrawLine(v1, v2, Color.green);
            Debug.DrawLine(v2, v3, Color.green);
            Debug.DrawLine(v3, v0, Color.green);
        }

    }
}
