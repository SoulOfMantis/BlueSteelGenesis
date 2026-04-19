public class ActionContext
{
    public Character acting { get; }
    public Character target { get; }
    public ActionContext prevActionContext { get; }
    public string actionName { get; }
    public ActionContext(Character acting, string actionName, ActionContext prevActionContext = null, Character target = null)
    {
        this.acting = acting;
        this.actionName = actionName;
        this.target = target;
        this.prevActionContext = prevActionContext;
    }
}
