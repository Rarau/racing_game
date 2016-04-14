public interface State<A> 
{
	void enter(A agent);
	void exit(A agent);
	void execute(A agent, StateMachine<A> fsm);
}