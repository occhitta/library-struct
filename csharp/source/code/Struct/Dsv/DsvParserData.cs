namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// DSV形式解析情報クラスです。
/// <para>当該情報は不変情報となります。</para>
/// </summary>
/// <typeparam name="TValue">項目種別</typeparam>
public sealed class DsvParserData<TValue> : DsvSourceData<TValue> {
	#region メンバー変数定義
	/// <summary>
	/// 項目配列
	/// </summary>
	private readonly TValue[] source;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 項目個数を取得します。
	/// </summary>
	/// <value>項目個数</value>
	public int Count => this.source.Length;
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <param name="index">項目番号</param>
	/// <returns>項目内容</returns>
	public TValue this[int index] => this.source[index];
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// DSV形式解析情報を生成します。
	/// </summary>
	/// <param name="source">項目集合</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" />自体が<c>Null</c>となっている場合</exception>
	internal DsvParserData(IEnumerable<TValue> source) {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		} else {
			this.source = [..source];
		}
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義:ToString
	/// <summary>
	/// 引数配列を表現文字列へ変換します。
	/// </summary>
	/// <param name="source">引数配列</param>
	/// <param name="escape">制御文字</param>
	/// <param name="anchor">区切文字</param>
	/// <returns>表現文字列</returns>
	private static string ToString(TValue[] source, char escape, char anchor) {
		var result = new string[source.Length];
		for (var index = 0; index < result.Length; index ++) {
			var cache1 = source[index];
			var cache2 = cache1?.ToString();
			var cache3 = cache2?.Replace($"{escape}", $"{escape}{escape}");
			result[index] = $"{escape}{cache3}{escape}";
		}
		return String.Join(anchor, result);
	}
	#endregion 内部メソッド定義:ToString

	#region 実装メソッド定義:IReadOnlyList
	/// <summary>
	/// 反復処理を取得します。
	/// </summary>
	/// <returns>反復処理</returns>
	IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() {
		foreach (var choose in this.source) {
			yield return choose;
		}
	}
	#endregion 実装メソッド定義:IReadOnlyList

	#region 継承メソッド定義
	/// <summary>
	/// 当該情報を表現文字列へ変換します。
	/// </summary>
	/// <returns>表現文字列</returns>
	public override string ToString() =>
		'[' + ToString(this.source, '"', ',') + ']';
	#endregion 継承メソッド定義
}
