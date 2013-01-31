using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArcadeShmup;

namespace QuadTreeTest
{
    public class IQuadItem
    {
        public Rectangle Position;
    }

    public class QuadTreeLeaf
    {
        public QuadTreeLeaf[] Children = new QuadTreeLeaf[4];
        public Rectangle Region;
        public int X { get { return Region.X; } }
        public int Y { get { return Region.Y; } }
        public int Width { get { return Region.Width; } }
        public int Height { get { return Region.Height; } }
        

        List<IQuadItem> Items = new List<IQuadItem>();
        public int Count { get { return Items.Count; } }

        private QuadTree Parent;

        public QuadTreeLeaf(int start_x, int start_y, int size_x, int size_y, QuadTree parent)
        {
            Region = new Rectangle(start_x, start_y, size_x, size_y);
            Parent = parent;
        }

        /* Is Clearning Necessary?
        public List<IQuadItem> Clear()
        {
            return Items;
        }
        */

        /* Is Checking if an area intersects something necessary?
        public bool Query(Rectangle Area)
        {
            if (Region.Intersects(Area))
            {
                if (Children[0] == null)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (Items[i].Position.Intersects(Area))
                            return true;
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Children[i].Query(Area))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        */

        private bool CustomContains(Rectangle a, Rectangle b)
        {
            return (a.Contains(b.Location) ||
                    b.Contains(a.Location) ||
                    a.Contains(new Point(b.X,b.Y + b.Height)) ||
                    b.Contains(new Point(a.X, a.Y + a.Height)) ||
                    a.Contains(new Point(b.X + b.Width, b.Y)) ||
                    b.Contains(new Point(a.X + a.Width, a.Y)) ||
                    a.Contains(new Point(a.X + a.Width, a.Y + Height)) ||
                    b.Contains(new Point(b.X + b.Width, b.Y + b.Height)));
        }

        public List<IQuadItem> RetrieveObjects(Rectangle Area)
        {
            List<IQuadItem> toReturn = new List<IQuadItem>();

            if (CustomContains(Region,Area))
            {
                if (Children[0] == null)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (CustomContains(Items[i].Position,Area))
                        {
                            toReturn.Add(Items[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        toReturn.AddRange(Children[i].RetrieveObjects(Area));
                    }
                }
            }

            return toReturn;
        }

        public void AddItem(IQuadItem toAdd)
        {
            //if the item isnt in the super region it isnt in the child regions either
            if (toAdd.Position.Intersects(Region))
            {
                if (Children[0] == null)
                {
                    //if (!Items.Contains(toAdd))
                    Items.Add(toAdd);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                        Children[i].AddItem(toAdd);
                }
            }

            //if this leaf gets too big, we split it up
            if ((Count > Parent.ChildSize) && (Width > Parent.RegionWidthMinimum))
            {
                for (int i = 0; i < 4; i++)
                {
                    MakeSplit(i);
                    Children[i] = new QuadTreeLeaf(split_rect.X, split_rect.Y, split_rect.Width, split_rect.Height, this.Parent);
                }
                for (int i = 0; i < Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Children[j].AddItem(Items[i]);
                    }
                }
                Items.Clear();
            }
        }

        public void Update()
        {
            //things that move out the area get removed from that area and readded to the tree
            if (Children[0] == null)
            {
                for (int i = Count - 1; i > -1; i--)
                {
                    if (!Items[i].Position.Intersects(Region))
                    {
                        //GameState.quadTree.AddItem(Items[i]);
                        Items.RemoveAt(i);
                    }
                }
                //when we force an update of the quad tree remove duplicates
                List<IQuadItem> dupe_free = new List<IQuadItem>();
                for (int i = 0; i < Count; i++)
                    if (!dupe_free.Contains(Items[i]))
                        dupe_free.Add(Items[i]);
                Items = dupe_free;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Children[i].Update();
                }
            }

            //if this leaf gets too big, we split it up
            if ((Count > Parent.ChildSize) && (Width > Parent.RegionWidthMinimum))
            {
                for (int i = 0; i < 4; i++)
                {
                    MakeSplit(i);
                    Children[i] = new QuadTreeLeaf(split_rect.X, split_rect.Y, split_rect.Width, split_rect.Height, this.Parent);
                }
                for (int i = 0; i < Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Children[j].AddItem(Items[i]);
                    }
                }
                Items.Clear();
            }
        }

        private Rectangle split_rect;
        private void MakeSplit(int num)
        {
            split_rect = new Rectangle(0, 0, (int)(Region.Width / 2), (int)(Region.Height / 2));
            switch (num)
            {
                case 0: split_rect.X = Region.X; split_rect.Y = Region.Y; break;
                case 1: split_rect.X = Region.X + (int)(Region.Width / 2); split_rect.Y = Region.Y; break;
                case 2: split_rect.X = Region.X; split_rect.Y = Region.Y + (int)(Region.Height / 2); break;
                default: split_rect.X = Region.X + (int)(Region.Width / 2); split_rect.Y = Region.Y + (int)(Region.Height / 2); break;
            }
        }

        public void DelItem(IQuadItem item)
        {
            if (!Items.Contains(item))
            {
                for (int i = 0; i < Children.Count(); i++)
                {
                    if (Children[i].Region.Intersects(item.Position))
                    {
                        Children[i].DelItem(item);
                    }
                }
            }
            else
            {
                Items.Remove(item);
            }
        }
    }

    public class QuadTree
    {
        private List<QuadTreeLeaf> Leaves = new List<QuadTreeLeaf>();
        public int ChildSize = 100;
        public int RegionWidth = 1024, RegionHeight = 1024;
        public int RegionWidthMinimum = 64;
        public void AddItem(IQuadItem toAdd)
        {
            int px, py;
            px = (int)(toAdd.Position.X / RegionWidth);
            py = (int)(toAdd.Position.Y / RegionHeight);

            //add super regions if they are needed.
            //TODO: clean
            if (!HaveRegion(new Rectangle(px * RegionWidth + 1, py * RegionHeight + 1, 1, 1)))
            {
                Leaves.Add(new QuadTreeLeaf(px * RegionWidth, py * RegionHeight, RegionWidth, RegionHeight, this));
            }
            if (!HaveRegion(new Rectangle((px + 1) * RegionWidth + 1, py * RegionHeight + 1, 1, 1)))
            {
                Leaves.Add(new QuadTreeLeaf((px + 1) * RegionWidth, py * RegionHeight, RegionWidth, RegionHeight, this));
            }
            if (!HaveRegion(new Rectangle(px * RegionWidth + 1, (py + 1) * RegionHeight + 1, 1, 1)))
            {
                Leaves.Add(new QuadTreeLeaf(px * RegionWidth, (py + 1) * RegionHeight, RegionWidth, RegionHeight, this));
            }
            if (!HaveRegion(new Rectangle((px + 1) * RegionWidth + 1, (py + 1) * RegionHeight + 1, 1, 1)))
            {
                Leaves.Add(new QuadTreeLeaf((px + 1) * RegionWidth, (py + 1) * RegionHeight, RegionWidth, RegionHeight, this));
            }

            for (int i = 0; i < Leaves.Count; i++)
            {
                if (Leaves[i].Region.Intersects(toAdd.Position))
                {
                    Leaves[i].AddItem(toAdd);
                }
            }

        }

        private bool HaveRegion(Rectangle region)
        {
            for (int i = 0; i < Leaves.Count; i++)
            {
                if (region.Intersects(Leaves[i].Region))
                {
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            for (int i = 0; i < Leaves.Count; i++)
            {
                Leaves[i].Update();
            }
        }

        public List<IQuadItem> FindItems(Rectangle region)
        {
            List<IQuadItem> toReturn = new List<IQuadItem>(), dupe_free = new List<IQuadItem>();
            for (int i = 0; i < Leaves.Count; i++)
            {
                toReturn.AddRange(Leaves[i].RetrieveObjects(region));
            }
            for (int i = 0; i < toReturn.Count; i++)
            {
                if (!dupe_free.Contains(toReturn[i]))
                    dupe_free.Add(toReturn[i]);
            }
            return dupe_free;
        }

        public void DelItem(IQuadItem item)
        {
            for (int i = 0; i < Leaves.Count; i++)
            {
                if (Leaves[i].Region.Intersects(item.Position))
                {
                    Leaves[i].DelItem(item);
                }
            }
        }
    }
}
