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
    }
}
