using System.Collections;

namespace Occhitta.Libraries.Common.UnitTest;

/// <summary>
/// <see cref="IEnumerable{T}" />検証クラスです。
/// </summary>
/// <typeparam name="TValue">要素種別</typeparam>
public abstract class IEnumerableTest<TValue> {
	#region 内部メソッド定義:IEnumerator
	/// <summary>
	/// <see cref="IEnumerator" />を検証します。
	/// </summary>
	/// <param name="actual">実質情報</param>
	/// <param name="expect">想定情報</param>
	private static void Test1(IEnumerator actual, TValue[] expect) {
		var offset = 0;
		while (actual.MoveNext()) {
			var value1 = actual.Current;
			var value2 = actual.Current;
			var value3 = expect[offset];
			Assert.Multiple(() => {
				Assert.That(value1, Is.EqualTo(value2),  "index={0}", offset);
				Assert.That(value1, Is.EqualTo(value3), "index={0}", offset);
			});
			offset++;
		}
	}
	/// <summary>
	/// <see cref="IEnumerator{T}" />を検証します。
	/// </summary>
	/// <param name="actual">実質情報</param>
	/// <param name="expect">想定情報</param>
	private static void Test2(IEnumerator<TValue> actual, TValue[] expect) {
		var offset = 0;
		while (actual.MoveNext()) {
			var value1 = actual.Current;
			var value2 = actual.Current;
			var value3 = expect[offset];
			Assert.Multiple(() => {
				Assert.That(value1, Is.EqualTo(value2),  "index={0}", offset);
				Assert.That(value1, Is.EqualTo(value3), "index={0}", offset);
			});
			offset++;
		}
	}
	#endregion 内部メソッド定義:IEnumerator

	#region 内部メソッド定義:IEnumerable
	/// <summary>
	/// <see cref="IEnumerable" />を検証します。
	/// </summary>
	/// <param name="actual">実質情報</param>
	/// <param name="expect">想定情報</param>
	private static void Test1(IEnumerable actual, TValue[] expect) {
		var choose = actual.GetEnumerator();
		Test1(choose, expect);
		if (choose is IDisposable change) {
			change.Dispose();
		}
	}
	/// <summary>
	/// <see cref="IEnumerable{T}" />を検証します。
	/// </summary>
	/// <param name="actual"></param>
	/// <param name="expect"></param>
	private static void Test2(IEnumerable<TValue> actual, TValue[] expect) {
		using var choose = actual.GetEnumerator();
		Test2(choose, expect);
	}
	#endregion 内部メソッド定義:IEnumerable

	#region 検証メソッド定義:IEnumerable
	/// <summary>
	/// <see cref="IEnumerable{T}" />を検証します。
	/// </summary>
	/// <param name="actual">実質情報</param>
	/// <param name="expect">想定情報</param>
	protected void Test(IEnumerable<TValue> actual, TValue[] expect) {
		Test1(actual, expect);
		Test2(actual, expect);
	}
	#endregion 検証メソッド定義:IEnumerable
}
