using System.Collections.Concurrent;
using Tsinswreng.CsCtx;
using Tsinswreng.CsTreeTest;

namespace Cs.Test.Domains.Ctx;

// 併發語義:內層是 ConcurrentDictionary,異步並行寫入同一 FnCtx 實例必須安全
// 注意:字典成員只能透過 IFnCtx 接口訪問;GetOrAdd 是 ConcurrentDictionary 特有的,
// 要透過 FnCtx.InnerDict 的實際類型才能調用,所以這裡用 Unsafe.As 取出真實類型
public partial class TestFnCtx {
	public void RegisterConcurrentSemantics(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestFnCtx),
			[typeof(FnCtx)],
			[nameof(IFnCtx.Add), nameof(IFnCtx.ContainsKey), nameof(IFnCtx.TryGetValue)],
			"Concurrent"
		);
		var R = register.Register;
		var T = Assert.IsTrue;

		R("多任務並行寫入不同 key 安全", async (o) => {
			IFnCtx ctx = new FnCtx();
			const int TaskCnt = 8;
			const int PerTask = 1000;
			var tasks = new List<Task>();
			for(int t = 0; t < TaskCnt; t++){
				var t0 = t;
				tasks.Add(Task.Run(()=>{
					for(int i = 0; i < PerTask; i++){
						ctx[t0 * PerTask + i] = i;
					}
				}));
			}
			await Task.WhenAll(tasks);
			T(ctx.Count == TaskCnt * PerTask);
			T(ctx[0] is 0);
			T(ctx[(TaskCnt * PerTask) - 1] is PerTask - 1);
			return null;
		});
		R("並行寫入同一 key 最後至少一個值生效且不崩", async (o) => {
			IFnCtx ctx = new FnCtx();
			var tasks = new List<Task>();
			for(int t = 0; t < 8; t++){
				var t0 = t;
				tasks.Add(Task.Run(()=>{
					for(int i = 0; i < 1000; i++){
						ctx["same"] = t0;
					}
				}));
			}
			await Task.WhenAll(tasks);
			T(ctx.ContainsKey("same"));
			T(ctx.Count == 1);
			return null;
		});
		R("枚舉期間允許並行寫入(快照枚舉不拋)", async (o) => {
			IFnCtx ctx = new FnCtx();
			for(int i = 0; i < 100; i++){
				ctx[i] = i;
			}
			var writeTask = Task.Run(()=>{
				for(int i = 100; i < 500; i++){
					ctx[i] = i;
				}
			});
			// 枚舉期間另一任務在寫,ConcurrentDictionary 快照枚舉不應拋異常
			var cnt = 0;
			foreach(var kv in ctx){
				cnt++;
			}
			T(cnt >= 100);
			await writeTask;
			T(ctx.Count == 500);
			return null;
		});
		R("GetOrAdd 原子性", async (o) => {
			// IFnCtx 接口面上沒有 GetOrAdd;透過 InnerDict 的真實類型(ConcurrentDictionary)調用
			var f = new FnCtx();
			var cd = (ConcurrentDictionary<obj, obj?>)f.InnerDict;
			var tasks = new List<Task<object?>>();
			for(int t = 0; t < 4; t++){
				tasks.Add(Task.Run(()=>{
					return cd.GetOrAdd("g", _ => "created");
				}));
			}
			var results = await Task.WhenAll(tasks);
			foreach(var r in results){
				T(r is "created");
			}
			T(cd.Count == 1);
			return null;
		});
	}
}