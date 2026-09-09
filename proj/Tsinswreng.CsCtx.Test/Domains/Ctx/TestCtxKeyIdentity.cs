using Tsinswreng.CsCtx;
using Tsinswreng.CsTreeTest;

namespace Cs.Test.Domains.Ctx;

// CtxKey<T> 身份鍵語義:引用即身份(與 JS Symbol 同理),同引用取回同值、新實例即使同名也是不同鍵
public partial class TestFnCtx {
	public void RegisterCtxKeyIdentity(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestFnCtx),
			[typeof(CtxKey<object>)],
			[nameof(CtxKey<object>.DisplayName)],
			"CtxKeyIdentity"
		);
		var R = register.Register;
		var T = Assert.IsTrue;

		R("同一引用取回同一值", async (o) => {
			IFnCtx ctx = new FnCtx();
			var key = new CtxKey<int>("counter");
			ctx[key] = 42;
			T(ctx[key] is 42);
			return null;
		});
		R("新實例即使同名也是不同鍵", async (o) => {
			IFnCtx ctx = new FnCtx();
			var key1 = new CtxKey<str>("k");
			var key2 = new CtxKey<str>("k");
			ctx[key1] = "one";
			T(ctx.ContainsKey(key1));
			T(!ctx.ContainsKey(key2));
			return null;
		});
		R("不同 T 的鍵互不影響", async (o) => {
			IFnCtx ctx = new FnCtx();
			var kInt = new CtxKey<int>("k");
			var kStr = new CtxKey<str>("k");
			ctx[kInt] = 1;
			ctx[kStr] = "s";
			T(ctx[kInt] is 1);
			T(ctx[kStr] is "s");
			T(ctx.Count == 2);
			return null;
		});
		R("DisplayName 只是標籤", async (o) => {
			var key = new CtxKey<int>("display");
			T(key.DisplayName == "display");
			key.DisplayName = "changed";
			T(key.DisplayName == "changed");
			return null;
		});
	}
}