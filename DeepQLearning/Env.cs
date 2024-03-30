﻿namespace DeepQLearning
{
    public class Env
    {
        private int gridSize;
        private int[] catPosition;
        private int[] mousePosition;
        private int[] dogPosition;
        private Random rand;

        public Env(int gridSize)
        {
            this.gridSize = gridSize;
            rand = new Random();
            Reset();
        }

        public void Reset()
        {
            int catX = -1, catY = -1, mouseX = -1, mouseY = -1, dogX = -1, dogY = -1;

            do
            {
                catX = rand.Next(gridSize);
                catY = rand.Next(gridSize);
                mouseX = rand.Next(gridSize);
                mouseY = rand.Next(gridSize);
                dogX = rand.Next(gridSize);
                dogY = rand.Next(gridSize);
            } while ((catX == mouseX && catY == mouseY) || (catX == dogX && catY == dogY) || (mouseX == dogX && mouseY == dogY));

            catPosition = [catX, catY];
            mousePosition = [mouseX, mouseY];
            dogPosition = [dogX, dogY];
        }

        public int[] GetStateVector()
        {
            return
            [
                catPosition[0], catPosition[1],
                mousePosition[0], mousePosition[1],
                dogPosition[0], dogPosition[1]
            ];
        }

        public bool Step(AgentAction action, out int[] nextState, out double reward)
        {
            NextAction nextAction = MoveAgent(catPosition, action);
            reward = GetReward([nextAction.X, nextAction.Y]);

            if (reward == 1 || reward == -1)
            {
                Reset();
                nextState = GetStateVector();
                return true;
            }

            catPosition = [nextAction.X, nextAction.Y];
            nextState = GetStateVector();
            return false;
        }

        private NextAction MoveAgent(int[] position, AgentAction action)
        {
            int x = position[0];
            int y = position[1];

            switch (action)
            {
                case AgentAction.LEFT: // left
                    x = Math.Max(x - 1, 0);
                    break;
                case AgentAction.RIGHT: // right
                    x = Math.Min(x + 1, gridSize - 1);
                    break;
                case AgentAction.UP: // up
                    y = Math.Max(y - 1, 0);
                    break;
                case AgentAction.DOWN: // down
                    y = Math.Min(y + 1, gridSize - 1);
                    break;
            }

            return new NextAction
            {
                Action = action,
                X = x,
                Y = y
            };
        }

        private double GetReward(int[] newCatPosition)
        {
            if (newCatPosition[0] == mousePosition[0] && newCatPosition[1] == mousePosition[1])
                return 1; // Cat caught the mouse
            else if (newCatPosition[0] == dogPosition[0] && newCatPosition[1] == dogPosition[1])
                return -1; // Cat ran into the dog
            else
                return -0.1; // No reward
        }
    }
}