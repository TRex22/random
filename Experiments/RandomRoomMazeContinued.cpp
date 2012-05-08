// RandomRoomMazeExperiment.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <stdlib.h>
#include <time.h>
#include <fstream>

using namespace std;

struct Rectangle
{
public:
	int X, Y, W, H;
};

bool Inside(Rectangle *rect, int x, int y)
{
	return (x >= rect->X && 
		x <= rect->X + rect->W && 
		y >= rect->Y && 
		y <= rect->Y + rect->H);
}

bool ListInside(Rectangle *rects[16], int x, int y, int count)
{
	for (int i = 0; i < count; i++)
	{
		if (x >= rects[i]->X && 
			x <= rects[i]->X + rects[i]->W && 
			y >= rects[i]->Y && 
			y <= rects[i]->Y + rects[i]->H)
		{
			//cout<<"\t"<<x<<","<<y<<" inside #"<<(i + 1)<<"\n";
			return true;
		}
	}
	return false;
}

bool Intersects(Rectangle *rect1, Rectangle *rect2)
{
	return (rect1->X < rect2->X + rect2->W && 
		rect1->X + rect1->W > rect2->X &&
		rect1->Y < rect2->Y + rect2->H && 
		rect1->Y + rect1->H > rect2->Y); 
}

bool ListIntersects(Rectangle *rects[16], Rectangle *rect, int count)
{
	for (int i = 0; i < count; i++)
	{
		if (Intersects(rects[i],rect))
		{
			return true;
		}
	}

	return false;
}

int Next(int max)
{
	return rand() % max;
}

int _tmain(int argc, _TCHAR* argv[])
{
	Rectangle *rectangles[16] = {};
	int count;
	srand(time(NULL));
	while (true)
	{
		ofstream myfile;
		myfile.open("output.txt");

		int map[128][128];

		int x, y, r;

		//first rectangle
		x = Next(128);
		y = Next(128);
		r = Next(32) + 32;
		rectangles[0] =  new Rectangle();
		rectangles[0]->X = x - r/2;
		rectangles[0]->Y = y - r/2;
		rectangles[0]->H = r;
		rectangles[0]->W = r;

		count = 1;

		//additional rectangles
		// at least 6 more
		// at most 15 more
		int iterations = 6 + Next(9);
		for (int i = 0; i < iterations; i++)
		{
			x = Next(128);
			y = Next(128);
			r = Next(16) + 16;

			rectangles[count] = new Rectangle();
			rectangles[count]->X = x - r/2;
			rectangles[count]->Y = y - r/2;
			rectangles[count]->H = r;
			rectangles[count]->W = r;

			int maxiter = 100;
			while (ListIntersects(rectangles, rectangles[count], count) && maxiter >= 0)
			{
				rectangles[count]->X = Next(128) - r/2;
				rectangles[count]->Y = Next(128) - r/2;
				maxiter--;
			}
			count++;
		}

		for (int i = 0; i < 128; i++)
		{
			for (int j = 0; j < 128; j++)
			{
				if (ListInside(rectangles,i+1,j+1,count))
				{
					map[i][j] = 1;
				}
			}
		}

		bool alt = false;
		for (int i = 0; i < count - 1; i++)
		{
			Rectangle *r1 = rectangles[i];
			Rectangle *r2 = rectangles[i+1];
			int lg_y, sm_y;
			int sm_x, lg_x;

			if (alt)
			{
				if ((r1->X + r1->W/2) < (r2->X + r2->W/2))
				{
					sm_x = (r1->X + r1->W/2);
					lg_x = (r2->X + r2->W/2);
				}
				else
				{
					sm_x = (r2->X + r2->W/2);
					lg_x = (r1->X + r1->W/2);
				}
				for (int i = sm_x - 1; i < lg_x + 1; i++)
				{
					map[i][r1->Y + r1->W/2 + 1] = 1;
					map[i][r1->Y + r1->W/2 - 1] = 1;
					map[i][r1->Y + r1->W/2] = 1;
				}
				if ((r1->Y + r1->H/2) < (r2->Y + r2->H/2))
				{
					sm_y = (r1->Y + r1->H/2);
					lg_y = (r2->Y + r2->H/2);
				}
				else
				{
					sm_y = (r2->Y + r2->H/2);
					lg_y = (r1->Y + r1->H/2);
				}
				for (int j = sm_y - 1; j < lg_y + 1; j++)
				{
					map[r2->X + r2->W/2 + 1][j] = 1;
					map[r2->X + r2->W/2 - 1][j] = 1;
					map[r2->X + r2->W/2][j] = 1;
				}


			}
			else
			{
				if ((r2->X + r2->W/2) < (r1->X + r1->W/2))
				{
					sm_x = (r2->X + r2->W/2);
					lg_x = (r1->X + r1->W/2);
				}
				else
				{
					sm_x = (r1->X + r1->W/2);
					lg_x = (r2->X + r2->W/2);
				}
				for (int i = sm_x - 1; i < lg_x + 1; i++)
				{
					map[i][r2->Y + r2->W/2 + 1] = 1;
					map[i][r2->Y + r2->W/2 - 1] = 1;
					map[i][r2->Y + r2->W/2] = 1;
				}
				if ((r2->Y + r2->H/2) < (r1->Y + r1->H/2))
				{
					sm_y = (r2->Y + r2->H/2);
					lg_y = (r1->Y + r1->H/2);
				}
				else
				{
					sm_y = (r1->Y + r1->H/2);
					lg_y = (r2->Y + r2->H/2);
				}
				for (int j = sm_y - 1; j < lg_y + 1; j++)
				{
					map[r1->X + r1->W/2 + 1][j] = 1;
					map[r1->X + r1->W/2 - 1][j] = 1;
					map[r1->X + r1->W/2][j] = 1;
				}
			}

			alt = !alt;
		}




		for (int i = 0; i < 128; i++)
		{
			for (int j = 0; j < 128; j++)
			{
				if (map[i][j] == 1)
				{
					myfile<<"1";
				}
				else
				{
					myfile<<"0";
				}
			}
			myfile<<"\n";
		}

		myfile<<"\n\n\n\n\n";
		cout<<"DONE\n\n";
		myfile.close();

		std::cin.get();
		for (int i = 0; i < 128; i++)
			for (int j = 0; j < 128; j++)
				map[i][j] = 0;
	}


	return 0;
}

