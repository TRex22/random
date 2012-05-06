public class Path {

	private int s_x, s_y, e_x, e_y;
	private int[][] _map;
	private boolean[][] _used;
	private int d;
	private int[] h_x, h_y;
	public Path(int[][] gameMap, int start_x, int start_y, int target_x, int target_y, int depth, int[] heads_x, int[] heads_y)
	{
		_map = gameMap;
		s_x = start_x;
		s_y = start_y;
		e_x = target_x;
		e_y = target_y;
		h_x = heads_x;
		h_y = heads_y;
		d = depth;
		_used = new boolean[_map.length][_map[0].length];
		for (int i = 0; i < _map.length; i++)
		{
			for (int j = 0; j < _map[0].length; j++)
			{
				if (_map[i][j] != 0)
					_used[i][j] = true;
			}
		}
	}
	
    final int UP = 0;
    final int DOWN = 1;
    final int LEFT = 2;
    final int RIGHT = 3;
    final int STRAIGHT = 5;

    Vector2[][] potentials = new Vector2[5000][50];
    int curr_dep = 1;
    int curr_cou = 0;
    int last_dep = 0;
    public int BestPath()
	{
		if (!isUsed(s_x-1,s_y))
		{
			potentials[curr_cou][0] = new Vector2(s_x-1,s_y, LEFT);
			curr_cou++;
		}
		if (!isUsed(s_x+1,s_y))
		{
			potentials[curr_cou][0] = new Vector2(s_x+1, s_y, RIGHT);
			curr_cou++;
		}
		if (!isUsed(s_x,s_y-1))
		{
			potentials[curr_cou][0] = new Vector2(s_x, s_y-1, UP);
			curr_cou++;
		}
		if (!isUsed(s_x,s_y+1))
		{
			potentials[curr_cou][0] = new Vector2(s_x, s_y+1, DOWN);
			curr_cou++;
		}
		if (curr_cou == 0)
			return STRAIGHT;
		
		return BestGeneratedPath();
	}
    
    public boolean isUsed(int x, int y)
    {
    	if ((x < 0) || (x >= _map.length) || (y < 0) || (y >= _map[0].length)) return true;
    	for (int i = 0; i < h_x.length; i++)
    	{
    		if (CheapDistance(x, h_x[i], y, h_y[i]) <= 1.4)
    		{
    			return false;
    		}
    	}
    	if (!_used[x][y])
    	{
    		//_used[x][y] = true;
    		return false;
    	}
    	return _used[x][y];
    }
    
	private float CheapDistance(int x, int y)
	{
		int dx, dy;
		dx = x - e_x;
		dy = y - e_y;
		return (float)(Math.sqrt(dx*dx + dy*dy));
	}
	private float CheapDistance(int x, int x2, int y, int y2)
	{
		int dx, dy;
		dx = x - x2;
		dy = y - y2;
		return (float)(Math.sqrt(dx*dx + dy*dy));
	}
    
	private int BestGeneratedPath() {

		for (int i = 0; i < d; i++)
		{
			int cc = curr_cou;
			for (int j = last_dep; j < cc; j++)
			{
				if (!isUsed(potentials[j][curr_dep - 1].x - 1, potentials[j][curr_dep - 1].y))
				{
					Duplicate(j);
					potentials[curr_cou-1][curr_dep] = new Vector2(potentials[j][curr_dep - 1].x - 1, potentials[j][curr_dep - 1].y, LEFT);
				}
				if (!isUsed(potentials[j][curr_dep - 1].x + 1, potentials[j][curr_dep - 1].y))
				{
					Duplicate(j);
					potentials[curr_cou-1][curr_dep] = new Vector2(potentials[j][curr_dep - 1].x + 1, potentials[j][curr_dep - 1].y, RIGHT);
				}
				if (!isUsed(potentials[j][curr_dep - 1].x, potentials[j][curr_dep - 1].y - 1))
				{
					Duplicate(j);
					potentials[curr_cou-1][curr_dep] = new Vector2(potentials[j][curr_dep - 1].x, potentials[j][curr_dep - 1].y - 1, UP);
				}
				if (!isUsed(potentials[j][curr_dep - 1].x, potentials[j][curr_dep - 1].y + 1))
				{
					Duplicate(j);
					potentials[curr_cou-1][curr_dep] = new Vector2(potentials[j][curr_dep - 1].x, potentials[j][curr_dep - 1].y + 1, DOWN);
				}
				for (int q = cc; q < curr_cou; q++)
				{
					if (potentials[q][curr_dep].x == e_x && potentials[q][curr_dep].y == e_y)
						return potentials[q][0].direction;
				}
			}
			last_dep = cc;
			curr_dep += 1;
		}
		float short_d = 9999;
		int lane = 0;
		for (int i = last_dep; i < curr_cou; i++)
		{
			for (int j = 0; j < curr_dep + 1; j++)
			{
				if (potentials[i][j] == null)
				{
					if (short_d > CheapDistance(potentials[i][j-1].x, potentials[i][j-1].y))
					{
						short_d = CheapDistance(potentials[i][j-1].x, potentials[i][j-1].y);
						lane = i;
					}
				}
			}
		}
		return potentials[lane][0].direction;
	}
	
	private void Duplicate(int line)
	{
		for (int i = 0; i < curr_dep; i++)
		{
			potentials[curr_cou][i] = potentials[line][i];
		}
		curr_cou++;
	}
}
