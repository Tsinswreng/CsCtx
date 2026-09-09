using Tsinswreng.CsCtx;
using Tsinswreng.CsTreeTest;

namespace Cs.Test.Domains.Ctx;

// 字典語義:FnCtx 繼承自 IDictionary<obj,obj?>、內層是 ConcurrentDictionary
// 注意:AutoInterface 生成的是顯式接口實現,字典成員須透過 IFnCtx 接口訪問
public partial class TestFnCtx {
	public void RegisterDictSemantics(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestFnCtx), // tester type
			[typeof(IFnCtx)], // testee types
			[nameof(IFnCtx.Add), nameof(IFnCtx.ContainsKey), nameof(IFnCtx.Remove)], // testee fn names
			"DictSemantics" // optional
		);
		var R = register.Register;
		var T = Assert.IsTrue;

		R("新增/讀取/刪除", async (o) => {
			IFnCtx ctx = new FnCtx();
			ctx["k"] = 1;
			T(ctx["k"] is 1);
			T(ctx.ContainsKey("k"));
			ctx.Remove("k");
			T(!ctx.ContainsKey("k"));
			T(ctx.Count == 0);
			return null;
		});
		R("Clear 清空", async (o) => {
			IFnCtx ctx = new FnCtx();
			ctx["a"] = "v";
			ctx.Clear();
			T(ctx.Count == 0);
			return null;
		});
		R("重複 key 覆寫", async (o) => {
			IFnCtx ctx = new FnCtx();
			ctx["k"] = 1;
			ctx["k"] = 2;
			T(ctx["k"] is 2);
			T(ctx.Count == 1);
			return null;
		});
		R("Keys/Values 枚舉", async (o) => {
			IFnCtx ctx = new FnCtx();
			ctx["a"] = 1;
			ctx["b"] = 2;
			var Keys = new List<object>();
			var Values = new List<object?>();
			foreach(var kv in ctx){
				Keys.Add(kv.Key);
				Values.Add(kv.Value);
			}
			T(Keys.Count == 2);
			T(Values.Count == 2);
			T(Keys.Contains("a") && Keys.Contains("b"));
			return null;
		});
	}
}