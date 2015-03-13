using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qlearning
{
    class QLearning
    {
        private double[,] q, r;

        private double gamma = 0.8, esp = 1e-4;

        public double Esp
        {
            get { return esp; }
            set { esp = value; }
        }

        public double Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        public double[,] R
        {
            get { return r; }
            set { r = value; }
        }

        public double[,] Q
        {
            get { return q; }
            set { q = value; }
        }

        private int states;

        public int States
        {
            get { return states; }
            set { states = value; }
        }

        private int currState;

        private bool converged = false;

        public bool Converged
        {
            get { return converged; }
            set { converged = value; }
        }
        
        public QLearning(int statesCount)
        {
            states = statesCount;
            q = new double[states, states];
            r = new double[states, states];
        }

        public QLearning(int statesCount, double[,] rMatrix)
        {
            states = statesCount;
            q = new double[states, states];
            r = rMatrix;
        }

        private double train(int initState)
        {
            double change = 0.0;
            currState = initState;
            for (int next = 0; next < states; ++next)
            {
                if (r[currState, next] < 0) continue;
                double qr = 0;
                for (int act = 0; act < states; ++act)
                    if (qr < q[next, act]) qr = q[next, act];
                qr = (1 - gamma) * r[currState, next] + gamma * qr;
                change = Math.Abs(qr - q[currState, next]);
                q[currState, next] = qr;                
            }
            return change;
        }

        public void Train(int loops)
        {
            double change = 0.0;
            Random rnd = new Random();
            while (loops-- > 0)
            {
                change += train(rnd.Next(states));
            }
            converged = change < esp;
        }

        public void Train()
        {
            while (!converged)
                Train(100);
        }

        public int GetNextState(int current)
        {
            int p = current;
            for(int i=0;i<states;++i)
                if (i != current)
                {
                    if (q[current, p] <= q[current, i]) p = i;
                }
            return p;
        }
    }
}
