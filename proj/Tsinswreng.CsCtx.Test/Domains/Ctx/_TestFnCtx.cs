using Tsinswreng.CsTreeTest;

namespace Cs.Test.Domains.Ctx;

// 一個 tester 類對應一個 testee 類（此處是 FnCtx/IFnCtx/CtxKey<T>），放在獨立文件夾。
// 主文件以下劃線開頭：負責組裝各分片的測試用例，不在主文件裏寫測試。
public partial class TestFnCtx : ITester {
	public TestFnCtx() {
		// FnCtx 是普通具體類，測試中直接 new，不依賴 DI。
	}

	public ITestNode RegisterTestsInto(ITestNode? Node) {
		Node ??= new TestNode();
		// 無副作用的純函數測試，可並行。
		Node.Ordered = true;
		Node.IsParallelRecursive = false;
		RegisterDictSemantics(Node);
		RegisterNullSemantics(Node);
		RegisterCtxKeyIdentity(Node);
		RegisterSubclassForwarding(Node);
		RegisterConcurrentSemantics(Node);
		return Node;
	}
}