using System.Collections.Generic;

namespace FuSM
{
    public class FuSM_Machine
    {
        private readonly List<FuzzyState> States, ActiveStates;

        public FuSM_Machine()
        {
            States = new List<FuzzyState>();
            ActiveStates = new List<FuzzyState>();
        }

        public void Update()
        {
            // Update State Machine
            States.ForEach(s =>
            {
                UpdateStateStatus(s, (s.ActivationLevel() > 0.0f));

                if (IsActive(s))
                    s.Update();
            });
        }

        private void UpdateStateStatus(FuzzyState state, bool status)
        {
            if (status)
            {
                if (!ActiveStates.Contains(state))
                {
                    ActiveStates.Add(state);
                    state.Enter();
                }
            }
            else
            {
                if (ActiveStates.Contains(state))
                {
                    ActiveStates.Remove(state);
                    state.Exit();
                }
            }
        }

        public void InitializeStates(FuSM_AI context)
        {
            States.ForEach(s => s.Init(context));
        }

        public void AddState(FuzzyState state)
        {
            States.Add(state);
        }

        public bool IsActive(FuzzyState state)
        {
            return ActiveStates.Contains(state);
        }

        /*   
        public enum Priority
        {
            Low,
            Medium, 
            High
        } 

        public Priority GetPriority(float fuzzyValue)
        {
            float low = TriangularFuzzyNumber(fuzzyValue, 0.0f, 0.0f, 0.3f);
            float medium = TriangularFuzzyNumber(fuzzyValue, 0.3f, 0.5f, 0.3f);
            float high = TriangularFuzzyNumber(fuzzyValue, 0.3f, 1.0f, 0.0f);

            float max = Mathf.Max(low, Mathf.Max(medium, high)); // Select the one of highest value (highest priority)

            if (low == max)
                return Priority.Low;
            if (medium == max)
                return Priority.Medium;
            if (high == max)
                return Priority.High;

            return Priority.Low;
        }

        private float TriangularFuzzyNumber(float x, float lhs, float med, float rhs)
        {
            // Formula from https://ijfs.usb.ac.ir/article_359_0038d4fb0f550de224041cbbbd77caf6.pdf
            // Where x = input, lhs = left-length, med = median, rhs = right-length

            if (x > (med - lhs) && x < med)
                return 1.0f - ((med - x) / lhs);

            if (x == med)
                return 1.0f;

            if (x > med && x < (med + rhs))
                return 1.0f - ((x - med) / rhs);         

            return 0.0f;
        }
        */
    }
}
