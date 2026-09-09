using Tsinswreng.CsCtx;
using Tsinswreng.CsTreeTest;

namespace Cs.Test.Domains.Ctx;

// null 相關語義:IFnCtx 的值類型是 obj?，key 不能為 null
// 注意:字典成員只能透過 IFnCtx 接口訪問(生成的是顯式接口實現)
public partial class TestFnCtx {
	public void RegisterNullSemantics(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestFnCtx),
			[typeof(IFnCtx)],
			[nameof(IFnCtx.Add), nameof(IFnCtx.TryGetValue), nameof(IFnCtx.ContainsKey)],
			"NullSemantics"
		);
		var R = register.Register;
		var T = Assert.IsTrue;

		R("值可為 null", async (o) => {
			IFnCtx ctx = new FnCtx();
			ctx["n"] = null;
			T(ctx.ContainsKey("n"));
			T(ctx["n"] == null);
			T(ctx.TryGetValue("n", out var V));
			T(V == null);
			return null;
		});
		R("Add 可存 null 值", async (o) => {
			IFnCtx ctx = new FnCtx();
			ctx.Add("n", null);
			T(ctx.ContainsKey("n"));
			return null;
		});
		R("key 不能為 null", async (o) => {
			IFnCtx ctx = new FnCtx();
			var Threw = false;
			try{
				ctx.Add(null!, 1);
			}catch(ArgumentNullException){
				Threw = true;
			}
			T(Threw);
			return null;
		});
		R("索引器 null key 拋錯", async (o) => {
			IFnCtx ctx = new FnCtx();
			var Threw = false;
			try{
				ctx[null!] = 1;
			}catch(ArgumentNullException){
				Threw = true;
			}
			T(Threw);
			return null;
		});
	}
}