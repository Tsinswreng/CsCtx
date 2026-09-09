using System.Collections.Concurrent;

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

// Function Context。
// 契約:一個可釋放的袋。同步用 `using`、異步用 `await using` 皆可;
// 具體釋放行為由實現者決定(默認實現 FnCtx 用鉤子,不掛就甚麼都不發生)。
public interface IFnCtx
	:IDictionary<obj, obj?>
	,IDisposable
	,IAsyncDisposable
{
}

public partial class FnCtx:IFnCtx{
	[BeaKona.AutoInterface(IncludeBaseInterfaces = true)]
	public IDictionary<obj, obj?> InnerDict{get;set;} = new ConcurrentDictionary<obj, obj?>();
	
	public Action? FnDispose{get;set;}
	public Func<ValueTask>? FnDisposeAsy{get;set;}
	
	// 同步釋放:僅調用 FnDispose,不阻塞等待異步鉤子
	public virtual void Dispose(){
		FnDispose?.Invoke();
	}
	
	// 異步釋放:僅調用 FnDisposeAsy
	public virtual ValueTask DisposeAsync()
		=> FnDisposeAsy is not null ? FnDisposeAsy() : ValueTask.CompletedTask;
}