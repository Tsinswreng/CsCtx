using Tsinswreng.CsCtx;
using Tsinswreng.CsTreeTest;

namespace Cs.Test.Domains.Ctx;

// 子類繼承轉發:模擬 CsSql 的 DbFnCtx : FnCtx 用法,子類直接繼承由 AutoInterface 生成的 IDictionary 轉發
public partial class TestFnCtx {
	public void RegisterSubclassForwarding(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestFnCtx),
			[typeof(FnCtx)],
			[nameof(IFnCtx.ContainsKey), nameof(IFnCtx.Add)],
			"Subclass"
		);
		var R = register.Register;
		var T = Assert.IsTrue;

		R("子類直接繼承字典轉發", async (o) => {
			IFnCtx sub = new DerivedFnCtx();
			sub["x"] = "y";
			T(sub["x"] is "y");
			T(sub.ContainsKey("x"));
			return null;
		});
		R("子類可當 IFnCtx 用", async (o) => {
			IFnCtx sub = new DerivedFnCtx();
			sub["a"] = 1;
			T(sub["a"] is 1);
			return null;
		});
	}

	// 模仿 CsSql 的 DbFnCtx:繼承 FnCtx 並自行擴展屬性
	public class DerivedFnCtx : FnCtx {
		public str? Tag{get;set;}
	}
}