namespace DT
{
    public abstract class Action : Node
    {
        protected DecisionTree_AI Context { get; }

        public Action(DecisionTree_AI context)
        {
            Context = context;
        }
    }
}