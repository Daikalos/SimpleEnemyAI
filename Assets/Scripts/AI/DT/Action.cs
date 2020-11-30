namespace DT
{
    public abstract class Action : Node
    {
        protected DT_AI Context { get; }

        public Action(DT_AI context)
        {
            Context = context;
        }
    }
}