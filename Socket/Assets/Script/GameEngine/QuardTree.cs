using System.Collections.Generic;

namespace GameEngine
{
    public class QuardTree
    {
        const int MAX_RECTS = 10;
        const int MAX_LEVEL = 2;
        public Rectangle bound;
        public int level;

        public LinkedList<Rectangle> objs = new LinkedList<Rectangle>();
        public List<QuardTree> trees = new List<QuardTree>();
        public QuardTree(Rectangle rect, int lvl)
        {
            bound = rect;
            level = lvl;
        }

        /// <summary>
        /// 获取象限，右上0,1,2,3，逆时针
        /// </summary>
        /// <returns></returns>
        int getIndex(Rectangle rect)
        {
            bool bottom = rect.origin.y + rect.height <= bound.center.y;
            bool top = rect.origin.y >= bound.center.y;
            bool left = rect.origin.x + rect.width <= bound.center.x;
            bool right = rect.origin.x >= bound.center.x;
            if (top)
            {
                if (right)
                {
                    return QuadrantType.RIGHT_TOP;
                }
                else if (left)
                {
                    return QuadrantType.LEFT_TOP;
                }
            }
            else if (bottom)
            {
                if (left)
                {
                    return QuadrantType.LEFT_BOTTOM;
                }
                else if (right)
                {
                    return QuadrantType.RIGHT_BOTTOM;
                }
            }

            //bool outSide = rect.origin.x > bound.origin.x + bound.width;
            //if (outSide)
            //{
            //    return QuadrantType.OUTSIDE;
            //}
            //outSide = rect.origin.x + rect.width < bound.origin.x;
            //if (outSide)
            //{
            //    return QuadrantType.OUTSIDE;
            //}
            //outSide = rect.origin.y > bound.origin.y + bound.width;
            //if (outSide)
            //{
            //    return QuadrantType.OUTSIDE;
            //}
            //outSide = rect.origin.y + rect.width < bound.origin.y;
            //if (outSide)
            //{
            //    return QuadrantType.OUTSIDE;
            //}

            // 如果物体跨越多个象限或超出区域，则放回-1
            return QuadrantType.Other;

        }

        //拆分成新的四叉树
        void split()
        {
            int sWidth = bound.center.x - bound.origin.x;
            int sHeight = bound.center.y - bound.origin.y;

            if (sWidth > 0
                && sHeight > 0)
            {
                trees.Add(new QuardTree(new Rectangle(bound.center.x, bound.center.y, sWidth, sHeight), level + 1));
                trees.Add(new QuardTree(new Rectangle(bound.origin.x, bound.center.y, sWidth, sHeight), level + 1));
                trees.Add(new QuardTree(new Rectangle(bound.origin.x, bound.origin.y, sWidth, sHeight), level + 1));
                trees.Add(new QuardTree(new Rectangle(bound.center.x, bound.origin.y, sWidth, sHeight), level + 1));

            }
        }

        //插入结点
        public void insert(Rectangle rect)
        {
            //如果超出边界

            // 如果该节点下存在子节点
            if (trees.Count > 0)
            {
                int index = getIndex(rect);
                if (index != -1)
                {
                    trees[index].insert(rect);
                    return;
                }
            }

            // 否则存储在当前节点下
            objs.AddLast(rect);

            // 如果当前节点存储的数量超过了MAX_OBJECTS
            if (trees.Count <= 0 &&
                objs.Count > MAX_RECTS &&
                level < MAX_LEVEL)
            {
                split();
                if (trees.Count > 0)
                {
                    List<Rectangle> removed = new List<Rectangle>();
                    foreach (Rectangle rectangle in objs)
                    {
                        int index = getIndex(rectangle);
                        if (index != -1)
                        {
                            trees[index].insert(rectangle);
                            removed.Add(rectangle);
                        }
                    }

                    for (int i = 0; i < removed.Count; i++)
                    {
                        objs.Remove(removed[i]);
                    }
                    removed.Clear();
                }
            }
        }

        /// <summary>
        /// 获取可能碰撞的对象
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        LinkedList<Rectangle> getCheckList(Rectangle rect)
        {
            LinkedList<Rectangle> ret = new LinkedList<Rectangle>();
            if (trees.Count > 0)
            {
                int index = getIndex(rect);
                if (index != -1)
                {
                    LinkedList<Rectangle> innerRet = trees[index].getCheckList(rect);
                    foreach (Rectangle rectangle in innerRet)
                    {
                        ret.AddLast(rectangle);
                    }
                }
                else
                {
                    Rectangle[] arr = rect.slice();
                    for (int i = arr.Length - 1; i >= 0; i--)
                    {
                        index = getIndex(arr[i]);
                        LinkedList<Rectangle> innerRet = trees[index].getCheckList(rect);
                        foreach (Rectangle rectangle in innerRet)
                        {
                            ret.AddLast(rectangle);
                        }
                    }
                }
            }
            foreach (Rectangle rectangle in objs)
            {
                ret.AddLast(rectangle);
            }

            return ret;

        }


        /// <summary>
        /// 刷新
        /// </summary>
        public void refresh(QuardTree root)
        {
            if (root == null)
            {
                root = this;
            }
            List<Rectangle> removed1 = new List<Rectangle>();
            List<Rectangle> removed2 = new List<Rectangle>();
            foreach (Rectangle rectangle in objs)
            {
                int index = getIndex(rectangle);
                // 如果矩形不属于该象限，则将该矩形重新插入
                if (!GraphicUtil.isInner(rectangle, bound))
                {
                    if (this != root)
                    {
                        removed1.Add(rectangle);
                    }

                    // 如果矩形属于该象限 且 该象限具有子象限，则
                    // 将该矩形安插到子象限中
                }
                else if (trees.Count > 0 &&
                    index != -1)
                {
                    trees[index].insert(rectangle);
                    removed2.Add(rectangle);
                }
            }

            for (int i = 0; i < removed1.Count; i++)
            {
                root.insert(removed1[i]);
                objs.Remove(removed1[i]);
            }
            removed1.Clear();

            for (int i = 0; i < removed2.Count; i++)
            {
                objs.Remove(removed2[i]);
            }
            removed2.Clear();
            foreach (QuardTree quardTree in trees)
            {
                quardTree.refresh(root);
            }
        }

        //narrow phase 碰撞
        public void narrowPhase()
        {
            foreach (Rectangle a in objs)
            {
                foreach (Rectangle b in objs)
                {
                    if (ReferenceEquals(a, b))
                    {
                        continue;
                    }
                    else
                    {
                        if (GraphicUtil.isOverlap(a, b))
                        {
                            if (a.collision != null)
                            {
                                a.collision.onEnter();
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < trees.Count; i++)
            {
                trees[i].narrowPhase();
            }
        }

        /// <summary>
        /// 获取边界内物体
        /// </summary>
        /// <returns>The inner count.</returns>
        public int getInnerCount()
        {
            int ret = 0;
            foreach (Rectangle rectangle in objs)
            {
                int index = getIndex(rectangle);
                if(index != -1)
                {
                    ret += 1;
                }
            }
            return ret;
        }

    }
}
