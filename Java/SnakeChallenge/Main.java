import java.io.IOException;
import java.util.Scanner;


public class Main {

    final int UP = 0;
    final int DOWN = 1;
    final int LEFT = 2;
    final int RIGHT = 3;
    final int STRAIGHT = 5;
    int numSnakes;
    int width;
    int height;
    int mode;
    Scanner reader;
    
    int[][] gameMap;
    int apple_x, apple_y;
    int head_x, head_y;

    public static void main(String args[]) throws IOException {
        Main m = new Main();
        m.run();
    }

    public Main() throws IOException {
        reader = new Scanner(System.in);
        readGameProperties();
        run();
    }

    public void run() throws IOException {
        while (true) {
            int move = getMove();
            System.out.println(move);
        }
    }
    int rand_x = 0, rand_y = 0;
    public int getMove() {
    	clearMap();
    	//System.out.println("log NEW MAP");
        String fruitString = reader.nextLine();
        apple_x = Integer.parseInt(fruitString.split(" ")[0]);
        apple_y = Integer.parseInt(fruitString.split(" ")[1]);
        int mySnake = Integer.parseInt(reader.nextLine());
        String[] snakeStates = new String[4];
        int counter = 2;
        shortestDistance = 9999;
        shortestDistanceSnake = 0;
        for (int i=0; i<4;i++){
            snakeStates[i]=reader.nextLine();
            if (i == mySnake)
            {
            	addSnake(snakeStates[i],1);
            	addHead(snakeStates[i]);
            }
            else
            {
            	addSnake(snakeStates[i], counter);
            	counter++;
            }
            //System.out.println("log "+snakeStates[i]);
        }
        //logMap();
        if (shortestDistanceSnake != 1)
        {
        	if (Math.random() < 0.05)
        	{
        		rand_x = (int)(Math.random()*width);
        		rand_y = (int)(Math.random()*height);
        	}
        	apple_x = rand_x;
        	apple_y = rand_y;
        	if (CheapDistance(head_x, head_y) < 7.5)
        	{
        		rand_x = (int)(Math.random()*width);
        		rand_y = (int)(Math.random()*height);
        	}
        }
        return BestMoveStart(head_x, head_y);
        
        //return STRAIGHT;
    }
    
    private int BestMoveStart(int head_x2, int head_y2) {
    	//return BestMove(head_x2, head_y2, 0);
    	Path p = new Path(gameMap, head_x2, head_y2, apple_x, apple_y, 5, heads_x, heads_y);
    	return p.BestPath();
	}

	/*private int BestMove(int head_x2, int head_y2, int depth) {
		float[] moves = new float[4];
		int[] moves_dir = {UP, DOWN, LEFT, RIGHT};
		//System.out.println("log BESTMOVE: ("+head_x2+","+head_y2+") to ("+apple_x+","+apple_y+")");
		moves[0] = CheapDistance(head_x2, head_y2 - 1);
		moves[1] = CheapDistance(head_x2, head_y2 + 1);
		moves[2] = CheapDistance(head_x2 - 1, head_y2);
		moves[3] = CheapDistance(head_x2 + 1, head_y2);
		if (!isOpen(head_x2, head_y2 - 1)) moves[0] = 9999;
		if (!isOpen(head_x2, head_y2 + 1)) moves[1] = 9999;
		if (!isOpen(head_x2 - 1, head_y2)) moves[2] = 9999;
		if (!isOpen(head_x2 + 1, head_y2)) moves[3] = 9999;
		
		for (int i = 0; i < 3; i++)
			for (int j = i; j < 4; j++)
			{
				if (moves[j] < moves[i])
				{
					float a = moves[j];
					moves[j] = moves[i];
					moves[i] = a;
					int b = moves_dir[i];
					moves_dir[i] = moves_dir[j];
					moves_dir[j] = b;
				}
			}
		
		//for (int i = 0; i < 4; i++) System.out.println("log " + moves[i] + " " + moves_dir[i]);
		return moves_dir[0];
	}*/
	
	private float CheapDistance(int x, int y)
	{
		int dx, dy;
		dx = x - apple_x;
		dy = y - apple_y;
		return (float)(Math.sqrt(dx*dx + dy*dy));
	}

	public boolean isOpen(int x, int y)
    {
		if ((x < 0) || (y < 0) || (x >= width) || (y >= height)) return false;
    	if (gameMap[x][y] != 0) return false;
    	for (int i = 0; i < 3; i++)
    		if (CheapDistance(heads_x[i], heads_y[i]) <= 1.4) return false;
    	return true;
    }
    
    
    private void readGameProperties() {
        String line = reader.nextLine();
        String split[] = line.split(" ");

        numSnakes = Integer.parseInt(split[0]);
        width = Integer.parseInt(split[1]);
        height = Integer.parseInt(split[2]);
        gameMap = new int[width][height];
        mode = Integer.parseInt(split[3]);
    }
    
    public void logMap()
    {
    	for (int i = 0; i < height; i++)
    	{
    		String out = "log ";
    		for (int j = 0; j < width; j++)
    		{
    			out = out + gameMap[j][i];
    		}
    		System.out.println(out);
    	}
    }
    
    public void clearMap()
    {
    	for (int i = 0; i < width; i++)
    		for (int j = 0; j < height; j++)
    		{
    			gameMap[i][j] = 0;
    		}
    }
    
    public void addHead(String s)
    {
    	String[] parts = s.split(" ");
    	String[] comp = parts[3].split(",");
    	head_x = Integer.parseInt(comp[0]);
    	head_y = Integer.parseInt(comp[1]);
    }
    
    float shortestDistance;
    int shortestDistanceSnake;
    int[] heads_x = new int[3];
    int[] heads_y = new int[3];
    public void addSnake(String sn, int snnum)
    {
    	String[] parts = sn.split(" ");
    	if (parts[0].equalsIgnoreCase("alive"))
    	{
    		int last_x, last_y;
    		String[] sp = parts[3].split(",");
    		
    		last_x = Integer.parseInt(sp[0]);
    		last_y = Integer.parseInt(sp[1]);
    		
    		if (snnum != 1)
    		{
    			heads_x[snnum-2] = last_x;
    			heads_y[snnum-2] = last_y;
    		}
    			
    		if (CheapDistance(last_x, last_y) < shortestDistance)
    		{
    			shortestDistance = CheapDistance(last_x, last_y);
    			shortestDistanceSnake = snnum;
    		}
    		for (int i = 4; i < parts.length; i++)
    		{
    			int cur_x, cur_y;
    			sp = parts[i].split(",");
    			
    			cur_x = Integer.parseInt(sp[0]);
    			cur_y = Integer.parseInt(sp[1]);
    			
    			int s_y, e_y, s_x, e_x;
    			if (cur_y > last_y)
    			{
    				s_y = last_y;
    				e_y = cur_y;
    			}
    			else
    			{
    				e_y = last_y;
    				s_y = cur_y;
    			}
    			if (cur_x > last_x)
    			{
    				s_x = last_x;
    				e_x = cur_x;
    			}
    			else
    			{
    				e_x = last_x;
    				s_x = cur_x;
    			}
    			for (int j = s_y; j < e_y + 1; j++)
    				for (int k = s_x; k < e_x + 1; k++)
    					if (!((k < 0) || (j < 0) || (k >= width) || (j >= height)))
    						gameMap[k][j] = snnum;
    			
    			last_x = cur_x;
    			last_y = cur_y;
    		}
    	}
    }

}
