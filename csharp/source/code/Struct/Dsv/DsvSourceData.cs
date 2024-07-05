namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// DSV形式要素情報インターフェースです。
/// <para>DSV情報の一行情報を保持するします。</para>
/// </summary>
public interface DsvSourceData<TValue> : IReadOnlyList<TValue> {
	#region 継承メソッド定義
	/// <summary>
	/// 反復処理を取得します。
	/// </summary>
	/// <returns>反復処理</returns>
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		using var source = GetEnumerator();
		while (source.MoveNext()) {
			yield return source.Current;
		}
	}
	#endregion 継承メソッド定義
}
