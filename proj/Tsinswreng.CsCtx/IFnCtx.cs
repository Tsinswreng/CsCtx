using System.Collections;
using System.Diagnostics.CodeAnalysis;

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

public class FnCtx:IFnCtx{
	
	public object? this[object key] {
		get=>InnerDict[key];
		set=>InnerDict[key]=value;
	}

	public IDictionary<obj, obj?> InnerDict{get;set;} = new Dictionary<obj, obj?>();

	public ICollection<object> Keys => InnerDict.Keys;

	public ICollection<object?> Values => InnerDict.Values;

	public int Count => InnerDict.Count;

	public bool IsReadOnly => throw new NotImplementedException();

	public void Add(object key, object? value) {
		throw new NotImplementedException();
	}

	public void Add(KeyValuePair<object, object?> item) {
		throw new NotImplementedException();
	}

	public void Clear() {
		throw new NotImplementedException();
	}

	public bool Contains(KeyValuePair<object, object?> item) {
		throw new NotImplementedException();
	}

	public bool ContainsKey(object key) {
		throw new NotImplementedException();
	}

	public void CopyTo(KeyValuePair<object, object?>[] array, int arrayIndex) {
		throw new NotImplementedException();
	}

	public IEnumerator<KeyValuePair<object, object?>> GetEnumerator() {
		throw new NotImplementedException();
	}

	public bool Remove(object key) {
		throw new NotImplementedException();
	}

	public bool Remove(KeyValuePair<object, object?> item) {
		throw new NotImplementedException();
	}

	public bool TryGetValue(object key, [MaybeNullWhen(false)] out object? value) {
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
}
