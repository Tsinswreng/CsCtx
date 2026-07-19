namespace Tsinswreng.CsCtx;


public interface ICtxKey{

}

public interface ICtxKey<T>: ICtxKey{

}

public class CtxKey<T>: ICtxKey<T>{
	public str? DisplayName{get;set;}
	public CtxKey(){

	}
	public CtxKey(str DisplayName){
		this.DisplayName = DisplayName;
	}
}

public interface IFnCtx:IDictionary<obj, obj?>{

}

public partial class FnCtx:IFnCtx{
	[BeaKona.AutoInterface(IncludeBaseInterfaces = true)]
	public IDictionary<obj, obj?> InnerDict{get;set;} = new Dictionary<obj, obj?>();
}
