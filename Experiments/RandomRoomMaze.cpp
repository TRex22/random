// RandomRoomMazeExperiment.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <stdlib.h>
#include <time.h>

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

bool Intersects(Rectangle *rect1, Rectangle *rect2)
{
	return (rect1->X < rect2->X + rect2->W && 
		    rect1->X + rect1->W > rect2->X &&
			rect1->Y < rect2->Y + rect2->H && 
			rect1->Y + rect1->H > rect2->Y); 
}

int Next(int max)
{
	return rand() % max;
}

int _tmain(int argc, _TCHAR* argv[])
{
	srand(time(NULL));
	while (true)
	{
		int map[64][64];

		int x, y;

		//first rectangle
		Rectangle r1;
		x = Next(64);
		y = Next(64);
		r1.X = x - 16;
		r1.Y = y - 16;
		r1.H = 32;
		r1.W = 32;

		//second rectangle
		Rectangle r2;
		r2.X = r1.X + 1;
		r2.Y = r1.Y + 1;
		r2.W = 16;
		r2.H = 16;
		//make sure that there is no intersection with r1
		while (Intersects(&r1,&r2))
		{
			r2.X = Next(64) - 8;
			r2.Y = Next(64) - 8;
		}

		//third triangle
		Rectangle r3;
		r3.X = r1.X + 1;
		r3.Y = r1.Y + 1;
		r3.W = 8;
		r3.H = 8;
		//make sure there is no intersection with r1 or r2
		while (Intersects(&r1,&r3) || Intersects(&r2,&r3))
		{
			r3.X = Next(64) - 4;
			r3.Y = Next(64) - 4;
		}

		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				if (Inside(&r1,i+1,j+1) || Inside(&r2,i+1,j+1) || Inside(&r3,i+1,j+1))
				{
					map[i][j] = 1;
				}
			}
		}


		int sm_x, lg_x;
		if ((r1.X + r1.W/2) < (r2.X + r2.W/2))
		{
			sm_x = (r1.X + r1.W/2);
			lg_x = (r2.X + r2.W/2);
		}
		else
		{
			sm_x = (r2.X + r2.W/2);
			lg_x = (r1.X + r1.W/2);
		}
		for (int i = sm_x - 1; i < lg_x + 1; i++)
		{
			map[i][r1.Y + r1.W/2 + 1] = 1;
			map[i][r1.Y + r1.W/2 - 1] = 1;
			map[i][r1.Y + r1.W/2] = 1;
		}

		if ((r2.X + r2.W/2) < (r3.X + r3.W/2))
		{
			sm_x = (r2.X + r2.W/2);
			lg_x = (r3.X + r3.W/2);
		}
		else
		{
			sm_x = (r3.X + r3.W/2);
			lg_x = (r2.X + r2.W/2);
		}
		for (int i = sm_x - 1; i < lg_x + 1; i++)
		{
			map[i][r2.Y + r2.W/2 + 1] = 1;
			map[i][r2.Y + r2.W/2 - 1] = 1;
			map[i][r2.Y + r2.W/2] = 1;
		}

		int lg_y, sm_y;
		if ((r1.Y + r1.H/2) < (r2.Y + r2.H/2))
		{
			sm_y = (r1.Y + r1.H/2);
			lg_y = (r2.Y + r2.H/2);
		}
		else
		{
			sm_y = (r2.Y + r2.H/2);
			lg_y = (r1.Y + r1.H/2);
		}
		for (int j = sm_y - 1; j < lg_y + 1; j++)
		{
			map[r2.X + r2.W/2 + 1][j] = 1;
			map[r2.X + r2.W/2 - 1][j] = 1;
			map[r2.X + r2.W/2][j] = 1;
		}

		if ((r2.Y + r2.H/2) < (r3.Y + r3.H/2))
		{
			sm_y = (r2.Y + r2.H/2);
			lg_y = (r3.Y + r3.H/2);
		}
		else
		{
			sm_y = (r3.Y + r3.H/2);
			lg_y = (r2.Y + r2.H/2);
		}
		for (int j = sm_y - 1; j < lg_y + 1; j++)
		{
			map[r3.X + r3.W/2 + 1][j] = 1;
			map[r3.X + r3.W/2 - 1][j] = 1;
			map[r3.X + r3.W/2][j] = 1;
		}

	


		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				if (map[i][j] == 1)
				{
					std::cout<<"1";
				}
				else
				{
					std::cout<<"0";
				}
			}
			std::cout<<"\n";
		}

		std::cin.get();
		for (int i = 0; i < 64; i++)
			for (int j = 0; j < 64; j++)
				map[i][j] = 0;
	}


	return 0;
}

